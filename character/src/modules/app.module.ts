import { Module } from '@nestjs/common';
import { MongooseModule } from '@nestjs/mongoose';

import { PrivateCharacterModule } from './character.private.module';
import { PublicCharacterModule } from './character.public.module';

@Module({
	imports: [
		MongooseModule.forRoot(process.env.MONGODB_HOST),
		PublicCharacterModule,
		PrivateCharacterModule,
	],
})
export class AppModule { }