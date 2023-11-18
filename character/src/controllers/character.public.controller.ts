import { Controller, Get, Param, Headers, Version } from '@nestjs/common';

import { CharacterService } from '../services/character.service';
import { Character } from '../schemas/character.schema';


@Controller('public/character')
export class PublicCharacterController {
	constructor(private readonly characterService: CharacterService) { }

	@Get(':username')
	@Version('1')
	getCharacter(@Param('username') username: string): Promise<Character> {
		return this.characterService.findOne(username);
	}
}