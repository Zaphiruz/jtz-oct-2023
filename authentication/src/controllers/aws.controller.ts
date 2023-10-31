import { Controller, Post, Body } from '@nestjs/common';
import { AwsCognitoAuthService } from '../services/aws-cognito-auth.service';
import {
	InitiateAuthCommandOutput,
	RespondToAuthChallengeResponse,
	GetUserCommandOutput,
} from '@aws-sdk/client-cognito-identity-provider';

import { LoginRequest } from '../models/login.request';
import { ChallengeUserRequest } from '../models/challenge-user.request';
import { GetUserRequest } from '../models/get-user.request';
import { VerifyTokenRequest } from '../models/verify-token.requests';

@Controller('aws')
export class AwsController {
	constructor(private readonly awsService: AwsCognitoAuthService) {}

	@Post('authenticateUser')
	async authenticateUser(
		@Body() loginRequest: LoginRequest,
	): Promise<InitiateAuthCommandOutput> {
		return this.awsService.authenticateUser(loginRequest);
	}

	@Post('challengeUser')
	async challengeUser(
		@Body() challengeAnswer: ChallengeUserRequest,
	): Promise<RespondToAuthChallengeResponse> {
		return this.awsService.challengeUser(challengeAnswer);
	}

	@Post('getUser')
	async getUser(
		@Body() getUserRequest: GetUserRequest,
	): Promise<GetUserCommandOutput> {
		return this.awsService.getUser(getUserRequest);
	}

	@Post('verifyToken')
	async verifyToken(
		@Body() verifyTokenRequest: VerifyTokenRequest,
	): Promise<any> {
		return this.awsService.verifyToken(verifyTokenRequest);
	}
}