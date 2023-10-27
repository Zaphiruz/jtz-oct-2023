import { Controller, Post, Body } from '@nestjs/common';
import { LoginRequest } from '../models/login.request';
import { ChallengeUserRequest } from '../models/challenge-user.request';
import { AwsCognitoAuthService } from '../services/aws-cognito-auth.service';

@Controller('aws')
export class AwsController {
  constructor(private readonly awsService: AwsCognitoAuthService) {}

  @Post('authenticateUser')
  async authenticateUser(@Body() loginRequest: LoginRequest) {
    return this.awsService.authenticateUser(loginRequest);
  }

  @Post('challengeUser')
  async challengeUser(@Body() challengeAnswer: ChallengeUserRequest) {
    return this.awsService.challengeUser(challengeAnswer);
  }
}
