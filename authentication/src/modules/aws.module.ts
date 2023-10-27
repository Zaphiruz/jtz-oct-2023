import { Module } from '@nestjs/common';
import { AwsCognitoAuthService } from '../services/aws-cognito-auth.service';
import { AwsController } from '../controllers/aws.controller';
import { ConfigModule } from '@nestjs/config';
import AwsConfig from '../configs/aws-congito-auth.config';

@Module({
  imports: [
    ConfigModule.forRoot({
      isGlobal: true,
      load: [AwsConfig],
    }),
  ],
  controllers: [AwsController],
  providers: [AwsCognitoAuthService],
  exports: [AwsCognitoAuthService],
})
export class AwsModule {}
