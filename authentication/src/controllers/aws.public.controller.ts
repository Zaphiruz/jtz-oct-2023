import { Controller, Post, Body, Version } from '@nestjs/common';
import { AwsCognitoAuthService } from '../services/aws-cognito-auth.service';
import {
	InitiateAuthCommandOutput,
	RespondToAuthChallengeResponse,
} from '@aws-sdk/client-cognito-identity-provider';

import { LoginRequest } from '../models/login.request';
import { ChallengeUserRequest } from '../models/challenge-user.request';
import { VerifyTokenRequest } from '../models/verify-token.requests';

@Controller('public/aws')
export class PublicAwsController {
	constructor(private readonly awsService: AwsCognitoAuthService) {}

	@Post('authenticateUser')
	@Version('1')
	async authenticateUser(
		@Body() loginRequest: LoginRequest,
	): Promise<InitiateAuthCommandOutput> {
		return this.awsService.authenticateUser(loginRequest);
	}

	@Post('challengeUser')
	@Version('1')
	async challengeUser(
		@Body() challengeAnswer: ChallengeUserRequest,
	): Promise<RespondToAuthChallengeResponse> {
		return this.awsService.challengeUser(challengeAnswer);
	}

	@Post('verifyToken')
	@Version('1')
	async verifyToken(
		@Body() verifyTokenRequest: VerifyTokenRequest,
	): Promise<any> {
		return this.awsService.verifyToken(verifyTokenRequest);
	}
}
