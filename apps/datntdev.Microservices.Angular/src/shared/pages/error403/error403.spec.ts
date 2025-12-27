import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Error403Page } from './error403';

describe('Pages.Error403', () => {
  let component: Error403Page;
  let fixture: ComponentFixture<Error403Page>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [Error403Page]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Error403Page);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
