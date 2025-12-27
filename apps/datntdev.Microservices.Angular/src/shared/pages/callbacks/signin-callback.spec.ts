import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';
import { provideRouter } from '@angular/router';

import { SigninCallback } from './signin-callback';
import { AuthService } from '@shared/services/auth-service';
import { LoggerService } from '@shared/services/logger-service';

describe('Pages.SigninCallback', () => {
  let component: SigninCallback;
  let fixture: ComponentFixture<SigninCallback>;
  let authService: jasmine.SpyObj<AuthService>;
  let router: jasmine.SpyObj<Router>;

  beforeEach(async () => {
    const authServiceSpy = jasmine.createSpyObj('AuthService', ['signinCallback']);
    const routerSpy = jasmine.createSpyObj('Router', ['navigate']);

    await TestBed.configureTestingModule({
      declarations: [SigninCallback],
      providers: [
        provideRouter([]),
        { provide: LoggerService },
        { provide: AuthService, useValue: authServiceSpy },
        { provide: Router, useValue: routerSpy }
      ]
    })
    .compileComponents();

    authService = TestBed.inject(AuthService) as jasmine.SpyObj<AuthService>;
    router = TestBed.inject(Router) as jasmine.SpyObj<Router>;
    
    fixture = TestBed.createComponent(SigninCallback);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should navigate to root on successful signin', async () => {
    sessionStorage.setItem('redirectUrl', '/protected');
    authService.signinCallback.and.returnValue(Promise.resolve(null as any));

    await component.ngOnInit();

    expect(authService.signinCallback).toHaveBeenCalled();
    expect(router.navigate).toHaveBeenCalledWith(['/protected']);
  });

  it('should navigate to root on signin error', async () => {
    authService.signinCallback.and.returnValue(Promise.reject('Error'));

    await component.ngOnInit();

    expect(authService.signinCallback).toHaveBeenCalled();
    expect(router.navigate).toHaveBeenCalledWith(['/']);
  });
});
