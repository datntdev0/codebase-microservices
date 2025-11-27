import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DatatableColumn } from '@components/datatable/datatable';
import { ToastService } from '@components/toast/toast-service';
import { MULTI_TENANCY } from '@shared/models/constants';
import { SrvAdminClient, TenantCreateDto } from '@shared/proxies/admin-proxies';

@Component({
  standalone: false,
  templateUrl: './tenant.html',
})
export class TenantPage implements OnInit {
  private readonly clientAdminSrv = inject(SrvAdminClient);
  private readonly toastSrv = inject(ToastService);
  private readonly fb = inject(FormBuilder);

  tenants: any[] = [];
  createForm!: FormGroup;
  isCreating: boolean = false;
  
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
    
    this.clientAdminSrv.tenants_GetAll(0, 10).subscribe(tenants => this.tenants = tenants.items ?? []);
  }

  protected onCreate(): void {
    if (this.createForm.invalid) {
      this.createForm.markAllAsTouched();
      return;
    }

    this.isCreating = true;
    const data = new TenantCreateDto({
      tenantName: this.createForm.value.tenantName
    })
    
    this.clientAdminSrv.tenants_Create(data)
      .subscribe({
        next: () => {
          this.createForm.reset();
          this.isCreating = false;
          this.ngOnInit(); // Refresh the list
        },
        error: () => {
          this.isCreating = false;
        }
      });
  }

  protected onEdit(item: any): void {
    console.log('Edit item', item);
    this.toastSrv.info('Info', 'Edit functionality is not implemented yet.', 3000);
  }
  protected onDelete(item: any): void {
    console.log('Delete item', item);
    this.toastSrv.info('Info', 'Delete functionality is not implemented yet.', 3000);
  }
}
