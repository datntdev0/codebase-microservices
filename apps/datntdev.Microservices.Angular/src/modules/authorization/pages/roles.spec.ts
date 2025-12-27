import { ComponentFixture, TestBed } from '@angular/core/testing';
import { DialogService } from '@components/dialog/dialog-service';
import { PaginatedResultOfRoleListDto, PaginatedResultOfRoleUserListDto, PaginatedResultOfUserListDto, SrvIdentityClient } from '@shared/proxies/identity-proxies';
import { of } from 'rxjs';
import { AuthorizationModule } from '../authorization-module';
import { PermissionService } from '../services/permission-service';
import { RolesPage } from './roles';

describe('Pages.Roles', () => {
  let component: RolesPage;
  let fixture: ComponentFixture<RolesPage>;
  let mockSrvIdentityClient: jasmine.SpyObj<SrvIdentityClient>;
  let mockDialogService: Partial<DialogService>;
  let mockPermissionService: jasmine.SpyObj<PermissionService>;

  beforeEach(async () => {
    // Create mock for SrvIdentityClient with the methods used in RolesPage
    mockSrvIdentityClient = jasmine.createSpyObj('SrvIdentityClient', [
      'roles_GetAll',
      'roles_GetAllUsers',
      'roles_Get',
      'roles_Create',
      'roles_Update',
      'roles_Delete',
      'users_GetAll'
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
    const mockRolesResult = new PaginatedResultOfRoleListDto({
      items: [],
      total: 0,
      offset: 0,
      limit: 10
    });

    const mockRoleUsersResult = new PaginatedResultOfRoleUserListDto({
      items: [],
      total: 0,
      offset: 0,
      limit: 10
    });

    const mockUsersResult = new PaginatedResultOfUserListDto({
      items: [],
      total: 0,
      offset: 0,
      limit: 10
    });

    mockSrvIdentityClient.roles_GetAll.and.returnValue(of(mockRolesResult));
    mockSrvIdentityClient.roles_GetAllUsers.and.returnValue(of(mockRoleUsersResult));
    mockSrvIdentityClient.users_GetAll.and.returnValue(of(mockUsersResult));
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

    fixture = TestBed.createComponent(RolesPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load roles on init', () => {
    expect(mockSrvIdentityClient.roles_GetAll).toHaveBeenCalled();
  });
});
