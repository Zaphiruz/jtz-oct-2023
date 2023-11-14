import { NestFactory } from '@nestjs/core';
import { CharacterModule } from './modules/character.module';

async function bootstrap() {
	const app = await NestFactory.create(CharacterModule);
	app.enableCors();
	await app.listen(3010);
}
bootstrap();
