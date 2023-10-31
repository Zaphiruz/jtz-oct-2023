export class VerifyTokenRequest {
	token: string;
	tokenUse?: 'id' | 'access';
	groups?: [string];
}
