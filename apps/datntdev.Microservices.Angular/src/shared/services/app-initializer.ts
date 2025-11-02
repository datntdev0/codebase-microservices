import { Injectable, inject } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '@shared/services/auth-service';
import { LoggerService } from './logger-service';

@Injectable({ providedIn: 'root' })
export class AppInitializerService {
  private loggerService = inject(LoggerService);
  private authService = inject(AuthService);
  private router = inject(Router);

  /**
   * Initialize application
   * This method is called before the app starts
   * Add your initialization logic here (e.g., load config, check auth, etc.)
   */
  async initialize(): Promise<void> {
    try {
      this.loggerService.info('Application is initializing...');

      // Initialize authentication service
      await this.authService.initialize();

      this.loggerService.info('Application initialized successfully');
    } catch (error) {
      this.loggerService.error('Error during application initialization:', error);
      this.router.navigate(['/error/500']);
    } finally {
      // Remove splash screen element if present (browser-only)
      document.getElementById('splash-screen')?.remove();
    }
  }
}

/**
 * Factory function for APP_INITIALIZER
 * This function will be called during app initialization
 */
export function appInitializerFactory(): Promise<void> {
  return inject(AppInitializerService).initialize();
}
