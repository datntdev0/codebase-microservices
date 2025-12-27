import { Component, inject, OnInit, ViewChild } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { DialogService } from '@components/dialog/dialog-service';
import { DatatableColumn } from '@components/datatable/datatable';
import { SrvIdentityClient } from '@shared/proxies/identity-proxies';
import { LocalDateTimePipe } from '@shared/pipes/local-datetime.pipe';
import { UserCreateModalComponent } from '../components/user-create-modal';
import { UserUpdateModalComponent } from '../components/user-update-modal';
import { PermissionService } from '../services/permission-service';

@Component({
  standalone: false,
  templateUrl: './users.html',
})
export class UsersPage implements OnInit {
  @ViewChild(UserCreateModalComponent) createModalComponent!: UserCreateModalComponent;
  @ViewChild(UserUpdateModalComponent) updateModalComponent!: UserUpdateModalComponent;

  private readonly clientIdentitySrv = inject(SrvIdentityClient);
  private readonly dialogSrv = inject(DialogService);
  private readonly permissionService = inject(PermissionService);
  private readonly localDateTimePipe = new LocalDateTimePipe();

  users: any[] = [];

  columns: DatatableColumn[] = [
    {
      key: 'username',
      title: 'Username',
      template: (item) =>
        `<span class="text-gray-800 text-hover-primary mb-1">${item.username}</span>`,
    },
    {
      key: 'emailAddress',
      title: 'Email Address',
    },
    {
      key: 'firstName',
      title: 'First Name',
    },
    {
      key: 'lastName',
      title: 'Last Name',
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
    
    this.clientIdentitySrv.users_GetAll(0, 10).subscribe(users => this.users = users.items ?? []);
  }

  protected onCreateUser(): void {
    this.createModalComponent.show();
  }

  protected onUserCreated(): void {
    this.ngOnInit();
  }

  protected onEdit(item: any): void {
    this.updateModalComponent.show(item);
  }

  protected onUserUpdated(): void {
    this.ngOnInit();
  }

  protected onDelete(item: any): void {
    this.dialogSrv.confirmDelete(`Are you sure you want to delete user "${item.username}"?`)
      .subscribe(confirmed => {
        if (!confirmed) return;

        this.clientIdentitySrv.users_Delete(item.id)
          .subscribe({next: () => this.ngOnInit() });
      });
  }
}
