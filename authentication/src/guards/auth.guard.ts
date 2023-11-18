import { Injectable, CanActivate, ExecutionContext } from '@nestjs/common';
import { Observable } from 'rxjs';
import { AwsCognitoAuthService } from '../services/aws-cognito-auth.service';
import { VerifyTokenRequest } from '../models/verify-token.requests';

@Injectable()
export class AuthGuard implements CanActivate {
	constructor(private readonly awsService: AwsCognitoAuthService) { }

	canActivate(
		context: ExecutionContext,
	): boolean | Promise<boolean> | Observable<boolean> {
		const request = context.switchToHttp().getRequest();
		return this.validateRequest(request);
	}

	async validateRequest(request: any) {
		let tokenRequest: VerifyTokenRequest = {
			token: request.header('authentication'),
			tokenUse: 'access'
		}
		let response = await this.verifyToken(tokenRequest);
		return response.ok;
	}

	async verifyToken(tokenRequest: VerifyTokenRequest) {
		return this.awsService.verifyToken(tokenRequest)
			.then(() => ({ ok: true }))
			.catch(() => ({ ok: false }))
	}
}