import { WebSocketGateway, SubscribeMessage, MessageBody, ConnectedSocket, WebSocketServer, WsResponse, OnGatewayConnection, OnGatewayDisconnect } from '@nestjs/websockets';
import { Server, Socket } from 'socket.io';

import { CharacterService } from '../services/character.service';
import { Character } from '../schemas/character.schema';

import { IdentityGatewayRequest } from '../models/identity.gateway.request';
import { IdentityGatewayResponse } from '../models/identity.gateway.response';
import { UpdateMapIdGatewayRequest } from '../models/update-map-id.gateway.request';
import { UpdateMapIdGatewayResponse } from '../models/update-map-id.gateway.response';

type SocketIdMap = {
	[key: string]: {
		socket: Socket,
		validationHandler: NodeJS.Timeout,
		isValid: boolean
	}
}

@WebSocketGateway({ serveClient: false })
export class CharacterWebsocketGateway implements OnGatewayConnection, OnGatewayDisconnect {
	constructor(private readonly characterService: CharacterService) { }

	connectedSockets: SocketIdMap = {};

	@WebSocketServer()
	server: Server;

	isClientValid(client: Socket): boolean {
		return this.connectedSockets[client.id].isValid;
	}

	handleConnection(client: Socket) {
		this.connectedSockets[client.id] = {
			socket: client,
			isValid: false,
			validationHandler: setTimeout(() => {
				if (!this.isClientValid(client)) {
					client.disconnect(true);
				}
			}, 5 * 1000),
		};
	}

	handleDisconnect(client: Socket) {
		delete this.connectedSockets[client.id];
	}

	@SubscribeMessage('identity')
	async identity(@MessageBody() data: IdentityGatewayRequest, @ConnectedSocket() client: Socket): Promise<IdentityGatewayResponse> {
		this.connectedSockets[client.id].isValid = true;
		clearTimeout(this.connectedSockets[client.id].validationHandler);
		return { ok: true };
	}

	@SubscribeMessage('updateMapId')
	async newMap(@MessageBody() data: UpdateMapIdGatewayRequest, @ConnectedSocket() client: Socket): Promise<UpdateMapIdGatewayResponse> {
		if (!this.isClientValid(client)) { return { ok: false } };

		let character: Character = await this.characterService.update(data.username, data.update);
		return { ok: true };
	}
}