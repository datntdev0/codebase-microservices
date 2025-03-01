import { Component, Injector } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { AppComponentBase } from '@shared/app-component-base';
import { TenantChangeDialogComponent } from './tenant-change-dialog.component';

@Component({
  selector: 'tenant-change',
  templateUrl: './tenant-change.component.html',
  standalone: false
})
export class TenantChangeComponent extends AppComponentBase {

  get isMultiTenancyEnabled(): boolean {
    return abp.multiTenancy.isEnabled;
  }

  constructor(injector: Injector, private _dialogService: MatDialog) {
    super(injector);
  }

  openModal(): void {
    this._dialogService.open(TenantChangeDialogComponent);
  }
}
