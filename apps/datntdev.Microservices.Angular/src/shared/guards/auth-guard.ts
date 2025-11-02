import { inject } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivateFn, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { AuthService } from '@shared/services/auth-service';
import { LoggerService } from '@shared/services/logger-service';

/**
 * Auth Guard - Protects routes that require authentication
 * Usage: Add to route configuration: { path: 'protected', component: ProtectedComponent, canActivate: [authGuard] }
 */
export const authGuard: CanActivateFn = async (
  route: ActivatedRouteSnapshot,
  state: RouterStateSnapshot
): Promise<boolean | UrlTree> => {
  const loggerService = inject(LoggerService);
  const authService = inject(AuthService);

  if (!authService.userSignal()) {
    loggerService.info('User not authenticated, redirecting to sign in...');

    // Store the attempted URL for redirecting after login
    sessionStorage.setItem('redirectUrl', state.url);

    // Redirect to identity server for authentication
    await authService.signIn({ state: { returnUrl: state.url } });

    return false;
  }

  return true;
};
