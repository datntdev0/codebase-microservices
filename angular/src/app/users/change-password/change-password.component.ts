import { Component, Injector } from '@angular/core';
import { Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/app-component-base';
import { AbpValidationError } from '@shared/components/validation/abp-validation.api';
import { ChangePasswordInput, SessionServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';

@Component({
  templateUrl: './change-password.component.html',
  animations: [appModuleAnimation()]
})
export class ChangePasswordComponent extends AppComponentBase {
  saving = false;
  changePasswordDto = new ChangePasswordInput();
  newPasswordValidationErrors: Partial<AbpValidationError>[] = [
    {
      name: 'pattern',
      localizationKey:
        'PasswordsMustBeAtLeast8CharactersContainLowercaseUppercaseNumber',
    },
  ];
  confirmNewPasswordValidationErrors: Partial<AbpValidationError>[] = [
    {
      name: 'validateEqual',
      localizationKey: 'PasswordsDoNotMatch',
    },
  ];

  constructor(
    injector: Injector,
    private sessionService: SessionServiceProxy,
    private router: Router
  ) {
    super(injector);
  }

  changePassword() {
    this.saving = true;

    this.sessionService
      .changePassword(this.changePasswordDto)
      .pipe(
        finalize(() => {
          this.saving = false;
        })
      )
      .subscribe((success) => {
        if (success) {
          abp.message.success('Password changed successfully', 'Success');
          this.router.navigate(['/']);
        }
      });
  }
}
