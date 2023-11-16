import { Injectable, BadRequestException } from '@nestjs/common';
import { ConfigService } from '@nestjs/config';
import { InjectModel } from '@nestjs/mongoose';
import { Model } from 'mongoose';

import { CharacterConfig } from '../configs/character.config.interface';
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

	async findOne(username: string, authToken: string): Promise<Character | BadRequestException> {
		let response = await this.verifyToken(authToken);
		if (!response.ok) {
			console.error("Bad Token");
			throw new BadRequestException('Invalid Token', { cause: new Error("Invalid Token"), description: "The authToken is not valid." });
		}
		username = username.toLowerCase();
		return this.characterModel.findOne({ username }).exec();
	}

	async create(newCharacterRequest: NewCharacterRequest, authToken: string): Promise<Character | BadRequestException> {
		let response = await this.verifyToken(authToken);
		if (!response.ok) {
			console.error("Bad Token");
			throw new BadRequestException('Invalid Token', { cause: new Error("Invalid Token"), description: "The authToken is not valid." });
		}
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

	async verifyToken(authToken: string) {
		return fetch(`${this.characterConfig.authServerHost}/verifyToken`, {
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
