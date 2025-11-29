import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TenantsPage } from './tenants';

describe('Tenant', () => {
  let component: TenantsPage;
  let fixture: ComponentFixture<TenantsPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TenantsPage]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TenantsPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
