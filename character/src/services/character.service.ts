import { Injectable } from '@nestjs/common';
import { ConfigService } from '@nestjs/config';

import { CharacterConfig } from '../configs/character.config.interface';
import { NewCharacterRequest } from '../models/new-character.request';


@Injectable()
export class CharacterService {
	private readonly characterConfig: CharacterConfig;

	constructor(private readonly configService: ConfigService) {
		this.characterConfig = configService.get<CharacterConfig>(
			'CharacterConfig',
		);
	}

	async getCharacter(username: string, authToken: string) {
		// await this.verifyToken(authToken);
		return {};
	}

	async newCharacter(newCharacterRequest: NewCharacterRequest, authToken: string) {
		// await this.verifyToken(authToken);
		return {};
	}

	async verifyToken(authToken: string) {
		return fetch(`${this.characterConfig.authServerHost}/verify-token`, {
			method: "POST",
			body: JSON.stringify({ token: authToken }),
			cache: "no-cache",
			headers: {
				"Content-Type": "application/json",
				"Accept": "application/json"
			}
		});
	}
}
