import { ErrorHandler, Injectable, Injector, NgZone, provideBrowserGlobalErrorListeners } from '@angular/core';
import { ToastService } from '@components/toast/toast-service';
import { ErrorResponse } from '@shared/models/proxies';

@Injectable({ providedIn: 'root' })
export class GlobalErrorHandler implements ErrorHandler {
  constructor(private injector: Injector, private zone: NgZone) {}

  handleError(error: any): void {

    if (error instanceof ErrorResponse) {
      this.zone.run(() => {
        const toastService = this.injector.get(ToastService);
        toastService.error(`Error ${error.statusCode}`, error.message);
      });
    }
    else {
      // Log the error to the console (or send it to a logging server)
      console.error('An unexpected error occurred:', error);
    }
  }
}

export function provideGlobalErrorHandler() {
  return [
    provideBrowserGlobalErrorListeners(),
    { provide: ErrorHandler, useClass: GlobalErrorHandler }
  ];
}