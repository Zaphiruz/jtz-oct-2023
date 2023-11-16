import { Prop, SchemaFactory } from '@nestjs/mongoose';
import { HydratedDocument } from 'mongoose';

export type VectorDocument = HydratedDocument<Vector2>;

export class Vector2 {
	@Prop()
	x: number = 0;

	@Prop()
	y: number = 0;
}

export const VectorSchema = SchemaFactory.createForClass(Vector2);