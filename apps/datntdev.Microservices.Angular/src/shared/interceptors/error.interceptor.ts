import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, throwError } from 'rxjs';
import { ToastService } from '../../components/toast/toast-service';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const toastService = inject(ToastService);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      let errorMessage = 'An unexpected error occurred';
      let errorTitle = 'Error';

      if (error.error instanceof ErrorEvent) {
        // Client-side error
        errorMessage = error.error.message;
        errorTitle = 'Client Error';
      } else {
        // Server-side error
        errorTitle = `Error ${error.status}`;
        
        if (error.status === 0) {
          errorMessage = 'Unable to connect to the server. Please check your internet connection.';
        } else if (error.status === 400) {
          errorMessage = error.error?.message || 'Bad request. Please check your input.';
        } else if (error.status === 401) {
          errorMessage = 'Unauthorized. Please log in again.';
        } else if (error.status === 403) {
          errorMessage = 'Access denied. You do not have permission to perform this action.';
        } else if (error.status === 404) {
          errorMessage = 'The requested resource was not found.';
        } else if (error.status === 500) {
          errorMessage = 'Internal server error. Please try again later.';
        } else if (error.status === 503) {
          errorMessage = 'Service unavailable. Please try again later.';
        } else {
          errorMessage = error.error?.message || error.message || `Server returned code ${error.status}`;
        }
      }

      // Show error toast
      toastService.error(errorTitle, errorMessage);

      // Re-throw the error so it can be handled by the caller if needed
      return throwError(() => error);
    })
  );
};
