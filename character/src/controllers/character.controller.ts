import { Controller, Get, Param, Headers, Post, Body } from '@nestjs/common';

import { CharacterService } from '../services/character.service';
import { NewCharacterRequest } from '../models/new-character.request';
import { Character } from '../schemas/character.schema';


@Controller('character')
export class CharacterController {
	constructor(private readonly characterService: CharacterService) { }

	@Get(':username')
	getCharacter(@Param('username') username: string, @Headers('authentication') authToken: string): Promise<Character> {
		return this.characterService.findOne(username, authToken);
	}

	@Post('')
	newCharacter(@Body() newCharacterRequest: NewCharacterRequest, @Headers('authentication') authToken: string): Promise<Character> {
		return this.characterService.create(newCharacterRequest, authToken);
	}
}