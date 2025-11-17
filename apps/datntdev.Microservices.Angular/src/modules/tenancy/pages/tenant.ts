import { Component, inject, OnInit } from '@angular/core';
import { SrvAdminClient } from '@shared/proxies/admin-proxies';
import { DatatableColumn } from '@components/datatable/datatable';

@Component({
  standalone: false,
  templateUrl: './tenant.html',
})
export class TenantPage implements OnInit {
  private readonly clientAdminSrv = inject(SrvAdminClient);

  tenants: any[] = [];
  
  columns: DatatableColumn[] = [
    {
      key: 'customer',
      title: 'Customer',
      minWidth: '125px',
      template: (item) => `<a href="/metronic8/demo1/apps/customers/view.html" class="text-gray-800 text-hover-primary mb-1">${item.customer}</a>`
    },
    {
      key: 'status',
      title: 'Status',
      minWidth: '125px',
      template: (item) => {
        const statusClass = item.status === 'Active' ? 'badge-light-success' 
          : item.status === 'Expiring' ? 'badge-light-warning' 
          : 'badge-light-danger';
        return `<div class="badge ${statusClass}">${item.status}</div>`;
      }
    },
    {
      key: 'billing',
      title: 'Billing',
      minWidth: '125px',
      template: (item) => `<div class="badge badge-light">${item.billing}</div>`
    },
    {
      key: 'product',
      title: 'Product',
      minWidth: '125px'
    },
    {
      key: 'createdDate',
      title: 'Created Date',
      minWidth: '125px'
    }
  ];

  ngOnInit(): void {
    this.clientAdminSrv.tenants_GetAll(0, 10).subscribe(tenants => {
      console.log(tenants);
      // Map the response to the format expected by the datatable
      // this.tenants = tenants;
    });

    // Temporary mock data for demonstration
    this.tenants = [
      { customer: 'Emma Smith', status: 'Active', billing: 'Auto-debit', product: 'Basic', createdDate: 'Aug 19, 2025' },
      { customer: 'Melody Macy', status: 'Active', billing: 'Manual - Credit Card', product: 'Basic', createdDate: 'Jun 20, 2025' },
      { customer: 'Max Smith', status: 'Active', billing: 'Manual - Cash', product: 'Teams Bundle', createdDate: 'Nov 10, 2025' },
      { customer: 'Sean Bean', status: 'Expiring', billing: 'Manual - Paypal', product: 'Enterprise', createdDate: 'Aug 19, 2025' },
      { customer: 'Brian Cox', status: 'Expiring', billing: 'Auto-debit', product: 'Basic', createdDate: 'Aug 19, 2025' },
      { customer: 'Mikaela Collins', status: 'Active', billing: 'Auto-debit', product: 'Enterprise Bundle', createdDate: 'Dec 20, 2025' },
      { customer: 'Francis Mitcham', status: 'Active', billing: 'Auto-debit', product: 'Teams', createdDate: 'Apr 15, 2025' },
      { customer: 'Olivia Wild', status: 'Suspended', billing: '--', product: 'Enterprise', createdDate: 'Mar 10, 2025' },
      { customer: 'Neil Owen', status: 'Expiring', billing: 'Auto-debit', product: 'Basic', createdDate: 'Jul 25, 2025' },
      { customer: 'Dan Wilson', status: 'Active', billing: 'Auto-debit', product: 'Enterprise Bundle', createdDate: 'Jun 24, 2025' }
    ];
  }

  protected onEdit(item: any): void {
    console.log('Edit item', item);
  }
  protected onDelete(item: any): void {
    console.log('Delete item', item);
  }
}
