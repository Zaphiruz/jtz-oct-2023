import {
  // BadRequestException,
  // HttpException,
  // HttpStatus,
  Injectable,
} from '@nestjs/common';
import { AwsCognitoAuthConfig } from '../configs/aws-congito-auth.config.interface';

// import { RegisterRequest } from '../models/register.request';
import { LoginRequest } from '../models/login.request';
// import { ForgotPasswordRequest } from '../models/forgot-password.request';
// import { ConfirmPasswordRequest } from '../models/confirm-password.request';
// import { VerifyUserRequest } from '../models/verify-user.request';
import { DeleteUserRequest } from '../models/delete-user.request';
import { ChallengeUserRequest } from 'src/models/challenge-user.request';
import {
  AdminDeleteUserCommand,
  CognitoIdentityProviderClient,
  // GetUserCommand,
  // ChangePasswordCommand,
  InitiateAuthCommand,
  InitiateAuthCommandInput,
  InitiateAuthCommandOutput,
  RespondToAuthChallengeCommand,
  RespondToAuthChallengeCommandInput,
  RespondToAuthChallengeResponse,
  AuthFlowType,
} from '@aws-sdk/client-cognito-identity-provider';
import { ConfigService } from '@nestjs/config';

@Injectable()
export class AwsCognitoAuthService {
  private readonly providerClient: CognitoIdentityProviderClient;
  private readonly awsConfig: AwsCognitoAuthConfig;

  constructor(private readonly configService: ConfigService) {
    this.awsConfig = configService.get<AwsCognitoAuthConfig>(
      'AwsCognitoAuthConfig',
    );

    this.providerClient = new CognitoIdentityProviderClient({
      region: this.awsConfig.region,
    });
  }

  // Add methods to manage user authentication, registration, and authorization here.
  async authenticateUser(
    loginRequest: LoginRequest,
  ): Promise<InitiateAuthCommandOutput> {
    const { username: USERNAME, password: PASSWORD } = loginRequest;

    const input: InitiateAuthCommandInput = {
      AuthFlow: AuthFlowType.USER_PASSWORD_AUTH,
      AuthParameters: {
        USERNAME,
        PASSWORD,
      },
      ClientId: this.awsConfig.clientId,
    };
    const command = new InitiateAuthCommand(input);
    return this.providerClient.send(command);
  }

  async challengeUser(
    challengeUserRequest: ChallengeUserRequest,
  ): Promise<RespondToAuthChallengeResponse> {
    const {
      challengeName: ChallengeName,
      challengeResponses: {
        username: USERNAME,
        authenticator_code: SOFTWARE_TOKEN_MFA_CODE,
        session: Session,
      },
    } = challengeUserRequest;

    const input: RespondToAuthChallengeCommandInput = {
      ClientId: this.awsConfig.clientId,
      ChallengeName,
      ChallengeResponses: {
        USERNAME,
        SOFTWARE_TOKEN_MFA_CODE,
      },
      Session,
    };
    const command = new RespondToAuthChallengeCommand(input);
    return this.providerClient.send(command);
  }

  async deleteUser(deleteUserRequest: DeleteUserRequest) {
    const adminDeleteUserCommand = new AdminDeleteUserCommand({
      Username: deleteUserRequest.username,
      UserPoolId: this.awsConfig.userPoolId,
    });

    await this.providerClient.send(adminDeleteUserCommand);
  }
}
