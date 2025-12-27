import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DatatableComponent } from './datatable';
import { PaginatorComponent } from '@components/paginator/paginator';
import { FormsModule } from '@angular/forms';

describe('Components.Datatable', () => {
  let component: DatatableComponent;
  let fixture: ComponentFixture<DatatableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FormsModule],
      declarations: [DatatableComponent, PaginatorComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DatatableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
