import { Component, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { AppAuthService } from '@shared/auth/app-auth.service';

@Component({
    selector: 'app-navbar-user-menu',
    templateUrl: 'navbar-user-menu.component.html',
    standalone: false
})
export class NavbarUserMenuComponent extends AppComponentBase {
  constructor(injector: Injector, private _authService: AppAuthService) {
    super(injector);
  }

  logout(): void {
    this._authService.logout();
  }
}
