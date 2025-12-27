import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterModule } from '@angular/router';
import { provideRouter } from '@angular/router';

import { RootComponent } from './root';
import { ComponentsModule } from '@components/components-module';

describe('Components.Root', () => {
  let component: RootComponent;
  let fixture: ComponentFixture<RootComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [RootComponent],
      imports: [
        RouterModule,
        ComponentsModule,
      ],
      providers: [provideRouter([])]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RootComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should have the correct host class', () => {
    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.classList.contains('d-flex')).toBeTruthy();
    expect(compiled.classList.contains('flex-column')).toBeTruthy();
    expect(compiled.classList.contains('flex-root')).toBeTruthy();
  });

  it('should render router-outlet', () => {
    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.querySelector('router-outlet')).toBeTruthy();
  });
});
