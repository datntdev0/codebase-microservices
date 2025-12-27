import { Component, inject, OnInit, ViewChild } from '@angular/core';
import { DialogService } from '@components/dialog/dialog-service';
import { DatatableColumn } from '@components/datatable/datatable';
import { SrvIdentityClient } from '@shared/proxies/identity-proxies';
import { LocalDateTimePipe } from '@shared/pipes/local-datetime.pipe';
import { RoleCreateModalComponent } from '../components/role-create-modal';
import { RoleUpdateModalComponent } from '../components/role-update-modal';
import { PermissionService } from '../services/permission-service';

@Component({
  standalone: false,
  templateUrl: './roles.html',
})
export class RolesPage implements OnInit {
  @ViewChild(RoleCreateModalComponent) createModalComponent!: RoleCreateModalComponent;
  @ViewChild(RoleUpdateModalComponent) updateModalComponent!: RoleUpdateModalComponent;

  private readonly clientIdentitySrv = inject(SrvIdentityClient);
  private readonly dialogSrv = inject(DialogService);
  private readonly permissionService = inject(PermissionService);
  private readonly localDateTimePipe = new LocalDateTimePipe();

  roles: any[] = [];

  columns: DatatableColumn[] = [
    {
      key: 'name',
      title: 'Name',
      template: (item) =>
        `<span class="text-gray-800 text-hover-primary mb-1 me-3">${item.name}</span>`
        + ((item.tenantId === null) ? `<span class="badge badge-primary">Host</span>` : ''),
    },
    {
      key: 'description',
      title: 'Description',
    },
    {
      key: 'createdAt',
      title: 'Created Date',
      minWidth: "125px",
      template: (item) => this.localDateTimePipe.transform(item.createdAt)
    },
    {
      key: 'updatedAt',
      title: 'Updated Date',
      minWidth: "125px",
      template: (item) => this.localDateTimePipe.transform(item.updatedAt)
    }
  ];

  ngOnInit(): void {
    // Preload permissions once for the session
    this.permissionService.getPermissions().subscribe();
    
    this.clientIdentitySrv.roles_GetAll(0, 10).subscribe(roles => this.roles = roles.items ?? []);
  }

  protected onCreateRole(): void {
    this.createModalComponent.show();
  }

  protected onRoleCreated(): void {
    this.ngOnInit();
  }

  protected onEdit(item: any): void {
    this.updateModalComponent.show(item);
  }

  protected onRoleUpdated(): void {
    this.ngOnInit();
  }

  protected onDelete(item: any): void {
    this.dialogSrv.confirmDelete(`Are you sure you want to delete role "${item.name}"?`)
      .subscribe(confirmed => {
        if (!confirmed) return;

        this.clientIdentitySrv.roles_Delete(item.id)
          .subscribe({next: () => this.ngOnInit() });
      });
  }
}
