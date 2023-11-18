import { Controller, Post, Body, Version, UseGuards } from '@nestjs/common';
import { AwsCognitoAuthService } from '../services/aws-cognito-auth.service';
import { AuthGuard } from '../guards/auth.guard';

import {
	GetUserCommandOutput,
} from '@aws-sdk/client-cognito-identity-provider';

import { GetUserRequest } from '../models/get-user.request';

@Controller('private/aws')
@UseGuards(AuthGuard)
export class PrivateAwsController {
	constructor(private readonly awsService: AwsCognitoAuthService) {}

	@Post('getUser')
	@Version('1')
	async getUser(
		@Body() getUserRequest: GetUserRequest,
	): Promise<GetUserCommandOutput> {
		return this.awsService.getUser(getUserRequest);
	}
}
