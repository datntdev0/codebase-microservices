import { ComponentFixture, TestBed } from '@angular/core/testing';
import { DialogComponent } from './dialog';

describe('Components.Dialog', () => {
  let component: DialogComponent;
  let fixture: ComponentFixture<DialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [DialogComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DialogComponent);
    component = fixture.componentInstance;
    
    // Set required inputs
    component.title = 'Test Title';
    component.message = 'Test Message';
    
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should display title and message', () => {
    const compiled = fixture.nativeElement;
    expect(compiled.textContent).toContain('Test Title');
    expect(compiled.textContent).toContain('Test Message');
  });

  it('should call onConfirm callback when confirm method is called', () => {
    const onConfirmSpy = jasmine.createSpy('onConfirm');
    component.onConfirm = onConfirmSpy;
    
    component.confirm();
    
    expect(onConfirmSpy).toHaveBeenCalled();
  });

  it('should call onCancel callback when cancel method is called', () => {
    const onCancelSpy = jasmine.createSpy('onCancel');
    component.onCancel = onCancelSpy;
    
    component.cancel();
    
    expect(onCancelSpy).toHaveBeenCalled();
  });
});
