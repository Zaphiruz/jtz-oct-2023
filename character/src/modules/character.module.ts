import { Module } from '@nestjs/common';
import { CharacterService } from '../services/character.service';
import { CharacterController } from '../controllers/character.controller';
import { ConfigModule } from '@nestjs/config';
import CharacterConfig from '../configs/character.config';

@Module({
	imports: [
		ConfigModule.forRoot({
			isGlobal: true,
			load: [CharacterConfig],
		}),
	],
	controllers: [CharacterController],
	providers: [CharacterService],
	exports: [CharacterService],
})
export class CharacterModule { }