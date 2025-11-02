import { Component, inject, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '@shared/services/auth-service';
import { LoggerService } from '@shared/services/logger-service';

@Component({
  selector: 'app-signin-callback',
  templateUrl: './signin-callback.html',
  host: { 'class': 'd-flex flex-column flex-center flex-column-fluid' },
  standalone: false
})
export class SigninCallback implements OnInit {
  private loggerService = inject(LoggerService);
  private authService = inject(AuthService);
  private router = inject(Router);

  async ngOnInit(): Promise<void> {
    try {
      const user = await this.authService.signinCallback();
      this.loggerService.info('Sign in completed successfully', user);

      // Check for return URL from state or session storage
      const returnUrl = sessionStorage.getItem('redirectUrl') || '/';
      sessionStorage.removeItem('redirectUrl');

      this.router.navigate([returnUrl]);
    } catch (error) {
      this.loggerService.error('Error completing sign in:', error);
      this.router.navigate(['/']);
    }
  }
}
