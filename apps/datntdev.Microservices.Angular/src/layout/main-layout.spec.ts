import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { PopoverModule } from 'ngx-bootstrap/popover';
import { MainLayout } from './main-layout';
import { AuthService } from '@shared/services/auth-service';
import { LoggerService } from '@shared/services/logger-service';

describe('Components.MainLayout', () => {
  let component: MainLayout;
  let fixture: ComponentFixture<MainLayout>;

  beforeEach(async () => {
    const authServiceSpy = jasmine.createSpyObj('AuthService', ['signinCallback']);
    const routerSpy = jasmine.createSpyObj('Router', ['navigate']);

    await TestBed.configureTestingModule({
      imports: [RouterTestingModule, PopoverModule.forRoot()],
      providers: [
        { provide: LoggerService },
        { provide: AuthService, useValue: authServiceSpy },
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(MainLayout);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should set body attributes on init', () => {
    component.ngOnInit();

    expect(document.body.getAttribute('data-kt-app-layout')).toBe('dark-sidebar');
    expect(document.body.getAttribute('data-kt-app-header-fixed')).toBe('true');
    expect(document.body.getAttribute('data-kt-app-sidebar-fixed')).toBe('true');
    expect(document.body.getAttribute('data-kt-app-sidebar-push-header')).toBe('true');
  });
});