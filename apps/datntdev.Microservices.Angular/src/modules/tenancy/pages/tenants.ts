import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DialogService } from '@components/confirmation-dialog/dialog-service';
import { DatatableColumn } from '@components/datatable/datatable';
import { MULTI_TENANCY } from '@shared/models/constants';
import { SrvAdminClient, TenantCreateDto, TenantUpdateDto } from '@shared/proxies/admin-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';

@Component({
  standalone: false,
  templateUrl: './tenants.html',
})
export class TenantsPage implements OnInit {
  private readonly clientAdminSrv = inject(SrvAdminClient);
  private readonly dialogSrv = inject(DialogService);
  private readonly fb = inject(FormBuilder);

  tenants: any[] = [];
  editingTenant: any = null;
  createForm!: FormGroup;
  updateForm!: FormGroup;
  isLoading: boolean = false;

  columns: DatatableColumn[] = [
    {
      key: 'tenantName',
      title: 'Tenant Name',
      template: (item) =>
        `<span class="text-gray-800 text-hover-primary mb-1">${item.tenantName}</span>`
        + ((item.id === MULTI_TENANCY.defaultTenantId) ? `<span class="ms-3 badge badge-primary">Default</span>` : ''),
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
      tenantName: ['', [Validators.required, Validators.minLength(3)]]
    });
    this.updateForm = this.fb.group({
      tenantName: ['', [Validators.required, Validators.minLength(3)]]
    });

    this.clientAdminSrv.tenants_GetAll(0, 10).subscribe(tenants => this.tenants = tenants.items ?? []);
  }

  protected onCreate(modal: ModalDirective): void {
    if (this.createForm.invalid) {
      this.createForm.markAllAsTouched();
      return;
    }

    this.isLoading = true;
    const data = new TenantCreateDto({
      tenantName: this.createForm.value.tenantName
    })

    this.clientAdminSrv.tenants_Create(data)
      .subscribe({
        next: () => {
          this.createForm.reset();
          this.isLoading = false;
          this.ngOnInit();
          modal.hide();
        },
        error: () => {
          this.isLoading = false;
        }
      });
  }

  protected onUpdate(modal: ModalDirective): void {
    if (this.updateForm.invalid) {
      this.updateForm.markAllAsTouched();
      return;
    }

    this.isLoading = true;
    const data = new TenantUpdateDto({
      tenantName: this.updateForm.value.tenantName
    });

    this.clientAdminSrv.tenants_Update(this.editingTenant.id, data)
      .subscribe({
        next: () => {
          this.updateForm.reset();
          this.isLoading = false;
          this.ngOnInit();
          modal.hide();
        },
        error: () => {
          this.isLoading = false;
        }
      });
  }

  protected onEdit(item: any, modal: ModalDirective): void {
    this.editingTenant = item;
    this.updateForm.patchValue({ tenantName: item.tenantName });
    modal.show();
  }

  protected onDelete(item: any): void {
    this.dialogSrv.confirmDelete(`Are you sure you want to delete tenant "${item.tenantName}"?`)
      .subscribe(confirmed => {
        if (!confirmed) return;

        this.clientAdminSrv.tenants_Delete(item.id)
          .subscribe({next: () => this.ngOnInit() });
      });
  }
}
