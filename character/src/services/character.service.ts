import { Injectable } from '@nestjs/common';
import { ConfigService } from '@nestjs/config';
import { InjectModel } from '@nestjs/mongoose';
import { Model } from 'mongoose';

import { CharacterConfig } from '../configs/character.config.interface';
import { UpdateCharacterRequest } from '../models/update-character.request';
import { NewCharacterRequest } from '../models/new-character.request';
import { Character } from '../schemas/character.schema';

@Injectable()
export class CharacterService {
	private readonly characterConfig: CharacterConfig;

	constructor(@InjectModel(Character.name) private characterModel: Model<Character>, private readonly configService: ConfigService) {
		this.characterConfig = configService.get<CharacterConfig>(
			'CharacterConfig',
		);
	}

	async findOne(username: string): Promise<Character> {
		username = username.toLowerCase();
		return this.characterModel.findOne({ username }).exec();
	}

	async create(newCharacterRequest: NewCharacterRequest): Promise<Character> {
		newCharacterRequest.username = newCharacterRequest.username.toLowerCase();
		return this.characterModel.create(newCharacterRequest)
			.catch(error => {
				if (error.code === 11000) {
					return this.characterModel.findOne({ username: newCharacterRequest.username }).exec();
				} else {
					throw error;
				}
			});
	}

	async update(username: string, updateCharacterRequest: UpdateCharacterRequest): Promise<Character> {
		username = username.toLowerCase();
		return this.characterModel.findOneAndUpdate({ username }, updateCharacterRequest).exec();
	}
}
