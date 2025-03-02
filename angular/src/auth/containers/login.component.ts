import { Component, Injector } from '@angular/core';
import { accountModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/app-component-base';
import { AppAuthService } from '@shared/auth/app-auth.service';

@Component({
  templateUrl: './login.component.html',
  animations: [accountModuleAnimation()],
  standalone: false
})
export class LoginComponent extends AppComponentBase {
  protected submitting: boolean = false;

  constructor(
    injector: Injector,
    protected authService: AppAuthService,
  ) {
    super(injector);
  }

  submit(): void {
    this.submitting = true;
    this.authService.login().finally(() => this.submitting = false);
  }
}
