import { ChallengeNameType } from '@aws-sdk/client-cognito-identity-provider';

// https://docs.aws.amazon.com/cognito-user-identity-pools/latest/APIReference/API_RespondToAuthChallenge.html#API_RespondToAuthChallenge_RequestSyntax

export class ChallengeUserRequest {
  challengeName: ChallengeNameType;
  challengeResponses: {
    username: string;
    session: string;
    authenticator_code?: string; // SOFTWARE_TOKEN_MFA
    challenge_answer?: string; //CUSTOM_CHALLENGE
    new_password?: string; // NEW_PASSWORD_REQUIRED
    SMS_code?: string; // SMS_MFA
    claim_signature?: string; // PASSWORD_VERIFIER, DEVICE_PASSWORD_VERIFIER
    secret_block?: string; // PASSWORD_VERIFIER, DEVICE_PASSWORD_VERIFIER
    timestamp?: string; // PASSWORD_VERIFIER, DEVICE_PASSWORD_VERIFIER
    device_key?: string; // DEVICE_SRP_AUTH, DEVICE_PASSWORD_VERIFIER
    srp_a?: string; // DEVICE_SRP_AUTH
    answer?: 'SMS_MFA' | 'SOFTWARE_TOKEN_MFA'; // SELECT_MFA_TYPE
  };
}
