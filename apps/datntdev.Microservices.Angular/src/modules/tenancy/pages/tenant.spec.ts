import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TenantPage } from './tenant';

describe('Tenant', () => {
  let component: TenantPage;
  let fixture: ComponentFixture<TenantPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TenantPage]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TenantPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
