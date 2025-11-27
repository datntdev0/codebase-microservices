import { Component, OnInit, OnDestroy } from '@angular/core';
import { ToastService, ToastConfig } from './toast-service';
import { Subscription } from 'rxjs';

@Component({
  standalone: false,
  selector: 'app-toast',
  templateUrl: './toast.html',
})
export class ToastComponent implements OnInit, OnDestroy {
  private subscription?: Subscription;
  
  protected config: ToastConfig[] = [];
  
  constructor(private toastService: ToastService) {}

  ngOnInit(): void {
    this.subscription = this.toastService.toasts$.subscribe((toasts: ToastConfig[]) => {
      this.config = toasts;
    });
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  remove(id: string): void {
    this.toastService.remove(id);
  }

  getToastClass(type: string): string {
    const baseClass = 'toast';
    const typeClass = `toast-${type}`;
    return `${baseClass} ${typeClass}`;
  }
}
