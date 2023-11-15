import { Prop, Schema, SchemaFactory } from '@nestjs/mongoose';
import { HydratedDocument } from 'mongoose';

export type CatDocument = HydratedDocument<Character>;

@Schema()
export class Character {
	@Prop({ required: true })
	username: string;
}

export const CharacterSchema = SchemaFactory.createForClass(Character);