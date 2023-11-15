import { Controller, Get, Param, Headers, Post, Body } from '@nestjs/common';
import { CharacterService } from '../services/character.service';

import { NewCharacterRequest } from '../models/new-character.request';

@Controller('character')
export class CharacterController {
	constructor(private readonly characterService: CharacterService) { }

	@Get(':username')
	getCharacter(@Param('username') username: string, @Headers('authentication') authToken: string) {
		return this.characterService.getCharacter(username, authToken);
	}

	@Post('')
	newCharacter(@Body() newCharacterRequest: NewCharacterRequest, @Headers('authentication') authToken: string) {
		this.characterService.newCharacter(newCharacterRequest, authToken);
	}
}