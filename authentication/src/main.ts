import { NestFactory } from '@nestjs/core';
import { AwsModule } from './modules/aws.module';

async function bootstrap() {
	const app = await NestFactory.create(AwsModule);
	app.enableCors();
	await app.listen(3000);
}
bootstrap();
