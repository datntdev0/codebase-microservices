import { ComponentFixture, TestBed } from '@angular/core/testing';
import { DialogService } from '@components/dialog/dialog-service';
import { PaginatedResultOfRoleListDto, PaginatedResultOfUserListDto, PaginatedResultOfUserRoleListDto, SrvIdentityClient } from '@shared/proxies/identity-proxies';
import { of } from 'rxjs';
import { AuthorizationModule } from '../authorization-module';
import { PermissionService } from '../services/permission-service';
import { UsersPage } from './users';

describe('Pages.Users', () => {
  let component: UsersPage;
  let fixture: ComponentFixture<UsersPage>;
  let mockSrvIdentityClient: jasmine.SpyObj<SrvIdentityClient>;
  let mockDialogService: Partial<DialogService>;
  let mockPermissionService: jasmine.SpyObj<PermissionService>;

  beforeEach(async () => {
    // Create mock for SrvIdentityClient with the methods used in UsersPage
    mockSrvIdentityClient = jasmine.createSpyObj('SrvIdentityClient', [
      'users_GetAll',
      'users_GetAllRoles',
      'users_Get',
      'users_Create',
      'users_Update',
      'users_Delete',
      'roles_GetAll'
    ]);

    // Create mock for DialogService
    mockDialogService = {
      confirmDelete: jasmine.createSpy('confirmDelete').and.returnValue(of(true))
    };

    // Create mock for PermissionService
    mockPermissionService = jasmine.createSpyObj('PermissionService', [
      'getPermissions'
    ]);

    // Set default return values for the mocks
    const mockUsersResult = new PaginatedResultOfUserListDto({
      items: [],
      total: 0,
      offset: 0,
      limit: 10
    });

    const mockUserRolesResult = new PaginatedResultOfUserRoleListDto({
      items: [],
      total: 0,
      offset: 0,
      limit: 10
    });

    const mockRolesResult = new PaginatedResultOfRoleListDto({
      items: [],
      total: 0,
      offset: 0,
      limit: 10
    });

    mockSrvIdentityClient.users_GetAll.and.returnValue(of(mockUsersResult));
    mockSrvIdentityClient.users_GetAllRoles.and.returnValue(of(mockUserRolesResult));
    mockSrvIdentityClient.roles_GetAll.and.returnValue(of(mockRolesResult));
    mockPermissionService.getPermissions.and.returnValue(of([]));

    await TestBed.configureTestingModule({
      imports: [
        AuthorizationModule,
      ],
      providers: [
        { provide: SrvIdentityClient, useValue: mockSrvIdentityClient },
        { provide: DialogService, useValue: mockDialogService },
        { provide: PermissionService, useValue: mockPermissionService }
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UsersPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load users on init', () => {
    expect(mockSrvIdentityClient.users_GetAll).toHaveBeenCalled();
  });
});
