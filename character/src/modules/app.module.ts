import { Module } from '@nestjs/common';
import { MongooseModule } from '@nestjs/mongoose';
import { CharacterModule } from './character.module';

@Module({
	imports: [
		MongooseModule.forRoot(process.env.MONGODB_HOST),
		CharacterModule
	],
})
export class AppModule { }