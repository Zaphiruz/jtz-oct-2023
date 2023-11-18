import { VersioningType } from '@nestjs/common/enums';
import { NestFactory } from '@nestjs/core';
import { AwsModule } from './modules/aws.module';

async function bootstrap() {
	const app = await NestFactory.create(AwsModule);
	app.setGlobalPrefix('api');
	app.enableVersioning({
		type: VersioningType.URI,
		defaultVersion: '1',
	});
	app.enableCors();
	await app.listen(3000);
}
bootstrap();
