import { Module } from '@nestjs/common';
import { AppController } from '../controllers/app.controller';
import { AppService } from '../services/app.service';
import { AwsModule } from './aws.module';

@Module({
	imports: [AwsModule],
	controllers: [AppController],
	providers: [AppService],
})
export class AppModule {}
