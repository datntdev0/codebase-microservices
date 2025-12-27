import { TestBed } from '@angular/core/testing';
import { SrvIdentityClient, PermissionDto } from '@shared/proxies/identity-proxies';
import { of } from 'rxjs';
import { PermissionService } from './permission-service';

describe('Services.PermissionService', () => {
  let service: PermissionService;
  let mockSrvIdentityClient: jasmine.SpyObj<SrvIdentityClient>;

  beforeEach(() => {
    mockSrvIdentityClient = jasmine.createSpyObj('SrvIdentityClient', [
      'permissions_GetAll'
    ]);

    const mockPermissions: PermissionDto[] = [
      new PermissionDto({ permissionName: 'Users.View', permission: 1, parent: 0 }),
      new PermissionDto({ permissionName: 'Users.Create', permission: 2, parent: 1 }),
      new PermissionDto({ permissionName: 'Roles.View', permission: 3, parent: 0 })
    ];

    mockSrvIdentityClient.permissions_GetAll.and.returnValue(of(mockPermissions));

    TestBed.configureTestingModule({
      providers: [
        PermissionService,
        { provide: SrvIdentityClient, useValue: mockSrvIdentityClient }
      ]
    });

    service = TestBed.inject(PermissionService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should fetch permissions from API', (done) => {
    service.getPermissions().subscribe(permissions => {
      expect(permissions.length).toBe(3);
      expect(mockSrvIdentityClient.permissions_GetAll).toHaveBeenCalled();
      done();
    });
  });

  it('should cache permissions after first call', (done) => {
    service.getPermissions().subscribe(() => {
      service.getPermissions().subscribe(() => {
        expect(mockSrvIdentityClient.permissions_GetAll).toHaveBeenCalledTimes(1);
        done();
      });
    });
  });
});
