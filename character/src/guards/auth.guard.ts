import { Injectable, CanActivate, ExecutionContext } from '@nestjs/common';
import { Observable } from 'rxjs';

@Injectable()
export class AuthGuard implements CanActivate {
	canActivate(
		context: ExecutionContext,
	): boolean | Promise<boolean> | Observable<boolean> {
		const request = context.switchToHttp().getRequest();
		return this.validateRequest(request);
	}

	async validateRequest(request: any) {
		let response = await this.verifyToken(request.header('authentication'));
		return response.ok;
	}

	async verifyToken(authToken: string) {
		if (!authToken) {
			return { ok: false };
		}
		return fetch(`${process.env.AUTH_SERVER_HOST}/verifyToken`, {
			method: "POST",
			body: JSON.stringify({ token: authToken, tokenUse: 'access' }),
			cache: "no-cache",
			headers: {
				"Content-Type": "application/json",
				"Accept": "application/json"
			}
		});
	}
}