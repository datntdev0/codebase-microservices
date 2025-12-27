import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HeaderComponent } from './header';
import { AuthService } from '@shared/services/auth-service';
import { User, UserManager } from 'oidc-client-ts';

describe('Components.Header', () => {
  let component: HeaderComponent;
  let fixture: ComponentFixture<HeaderComponent>;
  let userManagerSpy: jasmine.SpyObj<UserManager>;

  beforeEach(async () => {
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
        addUserLoaded: jasmine.createSpy('addUserLoaded').and.callFake((cb: (user: User) => void) => { }),
        addUserUnloaded: jasmine.createSpy('addUserUnloaded').and.callFake((cb: () => void) => { }),
        addAccessTokenExpired: jasmine.createSpy('addAccessTokenExpired'),
        addUserSignedOut: jasmine.createSpy('addUserSignedOut')
      }
    });

    await TestBed.configureTestingModule({
      imports: [HeaderComponent],
      providers: [
        { provide: UserManager, useValue: userManagerSpy },
        AuthService
      ]
    })
      .compileComponents();

    fixture = TestBed.createComponent(HeaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
