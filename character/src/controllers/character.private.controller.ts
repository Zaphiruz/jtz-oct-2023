import { Controller, Param, Post, Body, Put, Version, UseGuards } from '@nestjs/common';

import { AuthGuard } from '../guards/auth.guard';
import { CharacterService } from '../services/character.service';
import { NewCharacterRequest } from '../models/new-character.request';
import { UpdateCharacterRequest } from '../models/update-character.request';
import { Character } from '../schemas/character.schema';

@Controller('private/character')
@UseGuards(AuthGuard)
export class PrivateCharacterController {
	constructor(private readonly characterService: CharacterService) { }
	@Post('')
	@Version('1')
	newCharacter(@Body() newCharacterRequest: NewCharacterRequest): Promise<Character> {
		return this.characterService.create(newCharacterRequest);
	}

	@Put(':username')
	@Version('1')
	updateCharacter(@Param('username') username: string, @Body() updateCharacterRequest: UpdateCharacterRequest): Promise<Character> {
		return this.characterService.update(username, updateCharacterRequest);
	}
}