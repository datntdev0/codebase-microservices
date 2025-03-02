import {
  Component,
  Injector,
  OnInit,
  Output,
  EventEmitter,
  ChangeDetectorRef
} from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/app-component-base';
import {
  CreateTenantInput,
  TenantsServiceProxy
} from '@shared/service-proxies/service-proxies';

@Component({
    templateUrl: 'create-tenant-dialog.component.html',
    standalone: false
})
export class CreateTenantDialogComponent extends AppComponentBase
  implements OnInit {
  saving = false;
  tenant: CreateTenantInput = new CreateTenantInput();

  @Output() onSave = new EventEmitter<any>();

  constructor(
    injector: Injector,
    public _tenantsService: TenantsServiceProxy,
    public bsModalRef: BsModalRef,
    private cd: ChangeDetectorRef
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.tenant.isActive = true;
    this.cd.detectChanges();
  }

  save(): void {
    this.saving = true;

    this._tenantsService.create(this.tenant).subscribe(
      () => {
        this.notify.info(this.l('SavedSuccessfully'));
        this.bsModalRef.hide();
        this.onSave.emit();
      },
      () => {
        this.saving = false;
      }
    );
  }
}
