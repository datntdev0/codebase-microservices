import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TenantPage } from './pages/tenant';
import { API_BASE_URL_ADMIN, SrvAdminClient } from '@shared/proxies/admin-proxies';
import { environment } from 'src/environments/environment';

const routes: Routes = [
  { path: 'tenants', component: TenantPage },
  { path: '**', redirectTo: '/error/404' }
]

@NgModule({
  declarations: [],
  imports: [RouterModule.forChild(routes)],
  providers: [
    SrvAdminClient,
    { provide: API_BASE_URL_ADMIN, useValue: environment.apiurl.srvAdmin },
  ],
})
export class TenancyModule { }
