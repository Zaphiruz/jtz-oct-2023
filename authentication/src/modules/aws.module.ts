import { Module } from '@nestjs/common';
import { AwsCognitoAuthService } from '../services/aws-cognito-auth.service';
import { PrivateAwsController } from '../controllers/aws.private.controller';
import { PublicAwsController } from '../controllers/aws.public.controller';
import { ConfigModule } from '@nestjs/config';
import AwsConfig from '../configs/aws-congito-auth.config';

@Module({
	imports: [
		ConfigModule.forRoot({
			isGlobal: true,
			load: [AwsConfig],
		}),
	],
	controllers: [PrivateAwsController, PublicAwsController],
	providers: [AwsCognitoAuthService],
	exports: [AwsCognitoAuthService],
})
export class AwsModule {}
