import { TestBed } from '@angular/core/testing';
import { UserManager, User } from 'oidc-client-ts';
import { AuthService } from './auth-service';

describe('Services.AuthService', () => {
  let service: AuthService;
  let userManagerSpy: jasmine.SpyObj<UserManager>;

  beforeEach(() => {
    userManagerSpy = jasmine.createSpyObj('UserManager', [
      'clearStaleState',
      'metadataService',
      'getUser',
      'signinRedirect',
      'signinRedirectCallback',
      'signoutRedirect'
    ], {
      metadataService: { getMetadata: jasmine.createSpy('getMetadata') },
      events: {
        addUserLoaded: jasmine.createSpy('addUserLoaded').and.callFake((cb: (user: User) => void) => {}),
        addUserUnloaded: jasmine.createSpy('addUserUnloaded').and.callFake((cb: () => void) => {}),
        addAccessTokenExpired: jasmine.createSpy('addAccessTokenExpired'),
        addUserSignedOut: jasmine.createSpy('addUserSignedOut')
      }
    });

    TestBed.configureTestingModule({
      providers: [
        { provide: UserManager, useValue: userManagerSpy },
        AuthService
      ]
    });

    service = TestBed.inject(AuthService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should initialize correctly', async () => {
    const mockUser = { expired: false } as User;
    userManagerSpy.getUser.and.returnValue(Promise.resolve(mockUser));

    await service.initialize();

    expect(userManagerSpy.clearStaleState).toHaveBeenCalled();
    expect(userManagerSpy.metadataService.getMetadata).toHaveBeenCalled();
    expect(userManagerSpy.getUser).toHaveBeenCalled();
    expect(service.userSignal()).toBe(mockUser);
  });

  it('should call signinRedirect with correct arguments', async () => {
    const args = { state: { returnUrl: '/home' } };

    await service.signIn(args);

    expect(userManagerSpy.signinRedirect).toHaveBeenCalledWith(args);
  });

  it('should call signinRedirectCallback and return user', async () => {
    const mockUser = {} as User;
    userManagerSpy.signinRedirectCallback.and.returnValue(Promise.resolve(mockUser));

    const result = await service.signinCallback();

    expect(userManagerSpy.signinRedirectCallback).toHaveBeenCalled();
    expect(result).toBe(mockUser);
  });

  it('should call signoutRedirect with correct arguments', async () => {
    const args = { state: { returnUrl: '/logout' } };

    await service.signOut(args);

    expect(userManagerSpy.signoutRedirect).toHaveBeenCalledWith(args);
  });
});
