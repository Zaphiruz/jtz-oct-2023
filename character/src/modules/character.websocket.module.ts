import { Module } from '@nestjs/common';
import { MongooseModule } from '@nestjs/mongoose';

import { CharacterService } from '../services/character.service';
import { CharacterWebsocketGateway } from '../gateways/character.gateway';
import { Character, CharacterSchema } from '../schemas/character.schema';

@Module({
	imports: [
		MongooseModule.forFeature([{ name: Character.name, schema: CharacterSchema }]),
	],
	providers: [
		CharacterWebsocketGateway,
		CharacterService,
	],
	exports: [
		CharacterService,
	]
})
export class CharacterWebsocketModule { }