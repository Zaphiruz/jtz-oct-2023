import { Module } from '@nestjs/common';
import { ConfigModule } from '@nestjs/config';
import { MongooseModule } from '@nestjs/mongoose';

import { CharacterService } from '../services/character.service';
import { CharacterController } from '../controllers/character.controller';
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
	controllers: [CharacterController],
	providers: [CharacterService],
	exports: [CharacterService],
})
export class CharacterModule { }