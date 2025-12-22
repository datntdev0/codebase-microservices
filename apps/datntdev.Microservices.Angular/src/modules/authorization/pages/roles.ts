import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DialogService } from '@components/dialog/dialog-service';
import { DatatableColumn } from '@components/datatable/datatable';
import { SrvIdentityClient, RoleCreateDto, RoleUpdateDto } from '@shared/proxies/identity-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { MULTI_TENANCY } from '@shared/models/constants';

@Component({
  standalone: false,
  templateUrl: './roles.html',
})
export class RolesPage implements OnInit {
  private readonly clientIdentitySrv = inject(SrvIdentityClient);
  private readonly dialogSrv = inject(DialogService);
  private readonly fb = inject(FormBuilder);

  roles: any[] = [];
  editingRole: any = null;
  createForm!: FormGroup;
  updateForm!: FormGroup;
  isLoading: boolean = false;

  columns: DatatableColumn[] = [
    {
      key: 'name',
      title: 'Name',
      template: (item) =>
        `<span class="text-gray-800 text-hover-primary mb-1">${item.name}</span>`
        + ((item.tenantId === null) ? `<span class="ms-3 badge badge-primary">Host</span>` : ''),
    },
    {
      key: 'description',
      title: 'Description',
    },
    {
      key: 'createdAt',
      title: 'Created Date',
    },
    {
      key: 'updatedAt',
      title: 'Updated Date',
    }
  ];

  ngOnInit(): void {
    this.createForm = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(3)]],
      description: ['']
    });
    this.updateForm = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(3)]],
      description: ['']
    });

    this.clientIdentitySrv.roles_GetAll(0, 10).subscribe(roles => this.roles = roles.items ?? []);
  }

  protected onCreate(modal: ModalDirective): void {
    if (this.createForm.invalid) {
      this.createForm.markAllAsTouched();
      return;
    }

    this.isLoading = true;
    const data = new RoleCreateDto({
      name: this.createForm.value.name,
      description: this.createForm.value.description
    })

    this.clientIdentitySrv.roles_Create(data)
      .subscribe({
        next: () => {
          this.createForm.reset();
          this.isLoading = false;
          this.ngOnInit();
          modal.hide();
        },
        error: (err) => {
          this.isLoading = false;
          throw err;
        }
      });
  }

  protected onUpdate(modal: ModalDirective): void {
    if (this.updateForm.invalid) {
      this.updateForm.markAllAsTouched();
      return;
    }

    this.isLoading = true;
    const data = new RoleUpdateDto({
      name: this.updateForm.value.name,
      description: this.updateForm.value.description
    });

    this.clientIdentitySrv.roles_Update(this.editingRole.id, data)
      .subscribe({
        next: () => {
          this.updateForm.reset();
          this.isLoading = false;
          this.ngOnInit();
          modal.hide();
        },
        error: (err) => {
          this.isLoading = false;
          throw err;
        }
      });
  }

  protected onEdit(item: any, modal: ModalDirective): void {
    this.editingRole = item;
    this.updateForm.patchValue({ 
      name: item.name,
      description: item.description
    });
    modal.show();
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
