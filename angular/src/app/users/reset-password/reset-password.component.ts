import { Component, OnInit, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { ResetPasswordInput, UsersServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html'
})
export class ResetPasswordDialogComponent extends AppComponentBase
  implements OnInit {
  public isLoading = false;
  public resetPasswordInput: ResetPasswordInput;
  id: number;

  constructor(
    injector: Injector,
    private _usersService: UsersServiceProxy,
    public bsModalRef: BsModalRef
  ) {
    super(injector);
  }

  ngOnInit() {
    this.isLoading = true;
    this.resetPasswordInput = new ResetPasswordInput();
    this.resetPasswordInput.userId = this.id;
    this.resetPasswordInput.newPassword = Math.random()
      .toString(36)
      .substr(2, 10);
    this.isLoading = false;
  }

  public resetPassword(): void {
    this.isLoading = true;
    this._usersService.resetPassword(this.resetPasswordInput).subscribe(
      () => {
        this.notify.info('Password Reset');
        this.bsModalRef.hide();
      },
      () => {
        this.isLoading = false;
      }
    );
  }
}
