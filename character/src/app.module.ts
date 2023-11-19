import { Module } from '@nestjs/common';
import { MongooseModule } from '@nestjs/mongoose';

import { PrivateCharacterModule } from './modules/character.private.module';
import { PublicCharacterModule } from './modules/character.public.module';
import { CharacterWebsocketModule } from './modules/character.websocket.module';

@Module({
	imports: [
		MongooseModule.forRoot(process.env.MONGODB_HOST),
		PublicCharacterModule,
		PrivateCharacterModule,
		CharacterWebsocketModule,
	],
})
export class AppModule { }