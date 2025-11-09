import { Component, inject, OnInit } from '@angular/core';
import { SrvAdminClient } from '@shared/proxies/admin-proxies';

@Component({
  selector: 'tenant',
  templateUrl: './tenant.html',
})
export class TenantPage implements OnInit {
  private readonly clientAdminSrv = inject(SrvAdminClient);

  ngOnInit(): void {
    this.clientAdminSrv.tenants_GetAll(0, 10).subscribe(tenants => {
      console.log(tenants);
    });
  }

}
