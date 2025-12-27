import { ComponentFixture, TestBed } from '@angular/core/testing';
import { DialogService } from '@components/dialog/dialog-service';
import { PaginatedResultOfTenantListDto, SrvAdminClient } from '@shared/proxies/admin-proxies';
import { of } from 'rxjs';
import { TenancyModule } from '../tenancy-module';
import { TenantsPage } from './tenants';

describe('Pages.Tenants', () => {
  let component: TenantsPage;
  let fixture: ComponentFixture<TenantsPage>;
  let mockSrvAdminClient: jasmine.SpyObj<SrvAdminClient>;
  let mockDialogService: Partial<DialogService>;

  beforeEach(async () => {
    // Create mock for SrvAdminClient with the methods used in TenantsPage
    mockSrvAdminClient = jasmine.createSpyObj('SrvAdminClient', [
      'tenants_GetAll',
      'tenants_Get',
      'tenants_Create',
      'tenants_Update',
      'tenants_Delete'
    ]);

    // Create mock for DialogService
    mockDialogService = {
      confirmDelete: jasmine.createSpy('confirmDelete').and.returnValue(of(true))
    };

    // Set default return values for the mocks
    const mockTenantsResult = new PaginatedResultOfTenantListDto({
      items: [],
      total: 0,
      offset: 0,
      limit: 10
    });

    mockSrvAdminClient.tenants_GetAll.and.returnValue(of(mockTenantsResult));

    await TestBed.configureTestingModule({
      imports: [
        TenancyModule,
      ],
      providers: [
        { provide: SrvAdminClient, useValue: mockSrvAdminClient },
        { provide: DialogService, useValue: mockDialogService }
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TenantsPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load tenants on init', () => {
    expect(mockSrvAdminClient.tenants_GetAll).toHaveBeenCalled();
  });
});
