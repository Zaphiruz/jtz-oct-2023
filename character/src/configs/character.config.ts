import { CharacterConfig } from './character.config.interface';
import { registerAs } from '@nestjs/config';

export default registerAs(
	'CharacterConfig',
	(): CharacterConfig => ({
		
	})
)