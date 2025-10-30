import { Injectable, inject } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '@shared/services/auth-service';

@Injectable({ providedIn: 'root' })
export class AppInitializerService {
  private authService = inject(AuthService);
  private router = inject(Router);

  /**
   * Initialize application
   * This method is called before the app starts
   * Add your initialization logic here (e.g., load config, check auth, etc.)
   */
  async initialize(): Promise<void> {
    try {
      console.log('Application is initializing...');

      // Initialize authentication service
      await this.authService.initialize();

      console.log('Application initialized successfully');
    } catch (error) {
      console.error('Error during application initialization:', error);
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
