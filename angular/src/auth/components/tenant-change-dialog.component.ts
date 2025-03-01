import { Component, Injector } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { AppComponentBase } from '@shared/app-component-base';
import { AppTenantAvailabilityState } from '@shared/AppEnums';
import { AuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { firstValueFrom } from 'rxjs';

@Component({
  templateUrl: './tenant-change-dialog.component.html',
  standalone: false
})
export class TenantChangeDialogComponent extends AppComponentBase {
  protected tenancyName: string = '';
  protected saving: boolean = false;

  constructor(
    injector: Injector,
    private _authService: AuthServiceProxy,
    protected _modalRef: MatDialogRef<TenantChangeDialogComponent>
  ) {
    super(injector);
  }

  save(): void {
    if (!this.tenancyName) {
      abp.multiTenancy.setTenantIdCookie(undefined);
      location.reload();
    } else {
      this.saving = true;
      this._authService.getTenantStatus(this.tenancyName).subscribe(
        {
          next: (result) => {
            switch (result.state) {
              case AppTenantAvailabilityState.Available:
                abp.multiTenancy.setTenantIdCookie(result.tenantId);
                location.reload();
                return;
              case AppTenantAvailabilityState.InActive:
                this.message.warn(this.l('TenantIsNotActive', this.tenancyName));
                break;
              case AppTenantAvailabilityState.NotFound:
                this.message.warn(this.l('ThereIsNoTenantDefinedWithName{0}', this.tenancyName));
                break;
            }
          },
          complete: () => this.saving = false
        });
    }
  }
}
