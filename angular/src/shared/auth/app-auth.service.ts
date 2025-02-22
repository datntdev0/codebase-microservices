import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { finalize } from 'rxjs/operators';
import { AppConsts } from '@shared/AppConsts';
import { UrlHelper } from '@shared/helpers/UrlHelper';
import { AuthServiceProxy, LoginInput, LoginOutput } from '@shared/service-proxies/service-proxies';
import { UtilsService } from '@lib/services/utils/utils.service';
import { TokenService } from '@lib/services/auth/token.service';
import { LogService } from '@lib/services/log/log.service';

@Injectable()
export class AppAuthService {
    loginInput: LoginInput;
    loginOutput: LoginOutput;
    rememberMe: boolean;

    constructor(
        private _router: Router,
        private _authService: AuthServiceProxy,
        private _utilsService: UtilsService,
        private _tokenService: TokenService,
        private _logService: LogService
    ) {
        this.clear();
    }

    logout(reload?: boolean): void {
        abp.auth.clearToken();
        abp.utils.deleteCookie(AppConsts.authorization.encryptedAuthTokenName);
        
        if (reload !== false) {
            location.href = AppConsts.appBaseUrl;
        }
    }

    login(finallyCallback?: () => void): void {
        finallyCallback = finallyCallback || (() => { });

        this._authService
            .login(this.loginInput)
            .pipe(
                finalize(() => {
                    finallyCallback();
                })
            )
            .subscribe((result: LoginOutput) => {
                this.processAuthenticateResult(result);
            });
    }

    private processAuthenticateResult(
        authenticateResult: LoginOutput
    ) {
        this.loginOutput = authenticateResult;

        if (authenticateResult.accessToken) {
            // Successfully logged in
            this.handleLogin(
                authenticateResult.accessToken,
                authenticateResult.encryptedAccessToken,
                authenticateResult.expireInSeconds,
                this.rememberMe
            );
        } else {
            // Unexpected result!

            this._logService.warn('Unexpected authenticateResult!');
            this._router.navigate(['account/login']);
        }
    }

    private handleLogin(
        accessToken: string,
        encryptedAccessToken: string,
        expireInSeconds: number,
        rememberMe?: boolean
    ): void {
        const tokenExpireDate = rememberMe
            ? new Date(new Date().getTime() + 1000 * expireInSeconds)
            : undefined;

        this._tokenService.setToken(accessToken, tokenExpireDate);

        this._utilsService.setCookieValue(
            AppConsts.authorization.encryptedAuthTokenName,
            encryptedAccessToken,
            tokenExpireDate,
            abp.appPath
        );

        let initialUrl = UrlHelper.initialUrl;
        if (initialUrl.indexOf('/login') > 0) {
            initialUrl = AppConsts.appBaseUrl;
        }

        location.href = initialUrl;
    }

    private clear(): void {
        this.loginInput = new LoginInput();
        this.loginInput.rememberClient = false;
        this.loginOutput = null;
        this.rememberMe = false;
    }
}
