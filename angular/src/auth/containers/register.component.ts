import { Component, Injector, Input } from '@angular/core';
import { Router } from '@angular/router';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/app-component-base';
import {
  AuthServiceProxy,
  RegisterInput,
  RegisterOutput
} from '@shared/service-proxies/service-proxies';
import { accountModuleAnimation } from '@shared/animations/routerTransition';
import { AppAuthService } from '@shared/auth/app-auth.service';

@Component({
    templateUrl: './register.component.html',
    animations: [accountModuleAnimation()],
    standalone: false
})
export class RegisterComponent extends AppComponentBase {
  model: RegisterInput = new RegisterInput();
  protected submitting: boolean = false;

  constructor(
    injector: Injector,
    private _authService: AuthServiceProxy,
    private _router: Router,
    private authService: AppAuthService
  ) {
    super(injector);
  }

  submit(): void {
    // this.saving = true;
    // this._authService
    //   .register(this.model)
    //   .pipe(
    //     finalize(() => {
    //       this.saving = false;
    //     })
    //   )
    //   .subscribe((result: RegisterOutput) => {
    //     if (!result.canLogin) {
    //       this.notify.success(this.l('SuccessfullyRegistered'));
    //       this._router.navigate(['/login']);
    //       return;
    //     }

    //     // Autheticate
    //     this.saving = true;
    //     this.authService.loginInput.userNameOrEmailAddress = this.model.userName;
    //     this.authService.loginInput.password = this.model.password;
    //      aw this.authService.login();
    //   });
  }
}
