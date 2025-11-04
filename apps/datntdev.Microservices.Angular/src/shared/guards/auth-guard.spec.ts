import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';

import { authGuard } from './auth-guard';
import { Router } from '@angular/router';
import { AuthService } from '@shared/services/auth-service';
import { User } from 'oidc-client-ts';

describe('authGuard', () => {
  let authService: jasmine.SpyObj<AuthService>;
  let router: jasmine.SpyObj<Router>;

  const executeGuard: CanActivateFn = (...guardParameters) => 
      TestBed.runInInjectionContext(() => authGuard(...guardParameters));

  beforeEach(() => {
    authService = jasmine.createSpyObj('AuthService', ['userSignal', 'signIn']);
    router = jasmine.createSpyObj('Router', ['navigate']);

    TestBed.configureTestingModule({
      providers: [
        { provide: AuthService, useValue: authService },
        { provide: Router, useValue: router },
      ],
    });
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });

  it('should allow activation if user is authenticated', async () => {
    const mockUser = { access_token: "123" } as User;
    authService.userSignal.and.returnValue(mockUser);

    const result = await executeGuard({} as any, { url: '/protected' } as any);

    expect(result).toBe(true);
    expect(authService.userSignal).toHaveBeenCalled();
  });

  it('should redirect if user is not authenticated', async () => {
    authService.userSignal.and.returnValue(null);
    authService.signIn.and.returnValue(Promise.resolve());

    const result = await executeGuard({} as any, { url: '/protected' } as any);

    expect(result).toBe(false);
    expect(authService.userSignal).toHaveBeenCalled();
    expect(authService.signIn).toHaveBeenCalledWith({ state: { returnUrl: '/protected' } });
    expect(sessionStorage.getItem('redirectUrl')).toBe('/protected');
  });
});
