import { Module } from '@nestjs/common';
import { ConfigModule } from '@nestjs/config';
import { MongooseModule } from '@nestjs/mongoose';

import { CharacterService } from '../services/character.service';
import { PrivateCharacterController } from '../controllers/character.private.controller';
import { Character, CharacterSchema } from '../schemas/character.schema';
import CharacterConfig from '../configs/character.config';

@Module({
	imports: [
		ConfigModule.forRoot({
			isGlobal: true,
			load: [CharacterConfig],
		}),
		MongooseModule.forFeature([{ name: Character.name, schema: CharacterSchema }]),
	],
	controllers: [PrivateCharacterController],
	providers: [CharacterService],
	exports: [CharacterService],
})
export class PrivateCharacterModule { }