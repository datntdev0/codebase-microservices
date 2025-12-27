import { TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';
import { provideRouter } from '@angular/router';

import { AppInitializerService, appInitializerFactory } from './app-initializer';
import { AuthService } from './auth-service';

describe('AppInitializerService', () => {
  let service: AppInitializerService;
  let authService: jasmine.SpyObj<AuthService>;
  let router: jasmine.SpyObj<Router>;

  beforeEach(() => {
    const authServiceSpy = jasmine.createSpyObj('AuthService', ['initialize']);
    const routerSpy = jasmine.createSpyObj('Router', ['navigate']);

    TestBed.configureTestingModule({
      providers: [
        AppInitializerService,
        { provide: AuthService, useValue: authServiceSpy },
        { provide: Router, useValue: routerSpy }
      ]
    });

    service = TestBed.inject(AppInitializerService);
    authService = TestBed.inject(AuthService) as jasmine.SpyObj<AuthService>;
    router = TestBed.inject(Router) as jasmine.SpyObj<Router>;
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should initialize successfully', async () => {
    authService.initialize.and.returnValue(Promise.resolve());

    await service.initialize();

    expect(authService.initialize).toHaveBeenCalled();
  });

  it('should handle initialization error', async () => {
    const error = new Error('Init failed');
    authService.initialize.and.returnValue(Promise.reject(error));

    await service.initialize();

    expect(authService.initialize).toHaveBeenCalled();
    expect(router.navigate).toHaveBeenCalledWith(['/error/500']);
  });

  it('should remove splash screen element if present', async () => {
    authService.initialize.and.returnValue(Promise.resolve());
    
    // Create a fake splash screen element
    const splashScreen = document.createElement('div');
    splashScreen.id = 'splash-screen';
    document.body.appendChild(splashScreen);

    await service.initialize();

    expect(document.getElementById('splash-screen')).toBeNull();
  });

  it('should not error if splash screen element is not present', async () => {
    authService.initialize.and.returnValue(Promise.resolve());

    await expectAsync(service.initialize()).toBeResolved();
  });
});

describe('Services.AppInitializer', () => {
  it('should return a Promise', () => {
    TestBed.configureTestingModule({
      providers: [
        AppInitializerService,
        provideRouter([]),
        { 
          provide: AuthService, 
          useValue: jasmine.createSpyObj('AuthService', ['initialize']) 
        }
      ]
    });

    const result = TestBed.runInInjectionContext(() => appInitializerFactory());
    
    expect(result).toBeInstanceOf(Promise);
  });
});
