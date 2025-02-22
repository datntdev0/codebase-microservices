import {
  ChangeDetectorRef,
  Component,
  EventEmitter,
  Injector,
  OnInit,
  Output,
} from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import {
  PermissionDto,
  PermissionDtoListResultDto,
  RoleDto,
  RolesServiceProxy
} from '@shared/service-proxies/service-proxies';
import { forEach as _forEach, includes as _includes, map as _map } from 'lodash-es';
import { BsModalRef } from 'ngx-bootstrap/modal';

@Component({
    templateUrl: 'edit-role-dialog.component.html',
    standalone: false
})
export class EditRoleDialogComponent extends AppComponentBase
  implements OnInit {
  saving = false;
  id: number;
  role = new RoleDto();
  permissions: PermissionDto[];
  grantedPermissionNames: string[];
  checkedPermissionsMap: { [key: string]: boolean } = {};

  @Output() onSave = new EventEmitter<any>();

  constructor(
    injector: Injector,
    private _rolesService: RolesServiceProxy,
    public bsModalRef: BsModalRef,
    private cd: ChangeDetectorRef
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this._rolesService
      .get(this.id)
      .subscribe((result: RoleDto) => {
        this.role = result;
        this.grantedPermissionNames = result.grantedPermissions;
        this.setInitialPermissionsStatus();
        this.cd.detectChanges();
      });
    this._rolesService
      .getPermissions()
      .subscribe((result: PermissionDtoListResultDto) => {
        this.permissions = result.items;
      });
  }

  setInitialPermissionsStatus(): void {
    _map(this.permissions, (item) => {
      this.checkedPermissionsMap[item.name] = this.isPermissionChecked(
        item.name
      );
    });
  }

  isPermissionChecked(permissionName: string): boolean {
    return _includes(this.grantedPermissionNames, permissionName);
  }

  onPermissionChange(permission: PermissionDto, $event) {
    this.checkedPermissionsMap[permission.name] = $event.target.checked;
  }

  getCheckedPermissions(): string[] {
    const permissions: string[] = [];
    _forEach(this.checkedPermissionsMap, function (value, key) {
      if (value) {
        permissions.push(key);
      }
    });
    return permissions;
  }

  save(): void {
    this.saving = true;

    const role = new RoleDto();
    role.init(this.role);
    role.grantedPermissions = this.getCheckedPermissions();

    this._rolesService.update(role).subscribe(
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
