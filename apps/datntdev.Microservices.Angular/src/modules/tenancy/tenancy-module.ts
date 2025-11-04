import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TenantPage } from './pages/tenant';

const routes: Routes = [
  { path: 'tenants', component: TenantPage },
  { path: '**', redirectTo: '/error/404' }
]

@NgModule({
  declarations: [],
  imports: [RouterModule.forChild(routes)]
})
export class TenancyModule { }
