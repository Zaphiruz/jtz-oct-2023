import { AwsCognitoAuthConfig } from './aws-congito-auth.config.interface';
import { registerAs } from '@nestjs/config';

export default registerAs(
  'AwsCognitoAuthConfig',
  (): AwsCognitoAuthConfig => ({
    userPoolId: process.env.USER_POOL_ID,
    clientId: process.env.CLIENT_ID,
    region: process.env.REGION,
  }),
);
