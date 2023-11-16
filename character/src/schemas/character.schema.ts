import { Prop, Schema, SchemaFactory } from '@nestjs/mongoose';
import { HydratedDocument } from 'mongoose';
import { Vector2 } from './vector2.schema';

export type CharacterDocument = HydratedDocument<Character>;

@Schema()
export class Character {
	@Prop({ required: true })
	username: string;

	@Prop({ type: Vector2 })
	globalPosition: Vector2

	@Prop()
	mapId: string;
}

export const CharacterSchema = SchemaFactory.createForClass(Character);