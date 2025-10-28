import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { Dashboard } from './pages/dashboard';

const routes: Routes = [
  { path: '', component: Dashboard },
  { path: '**', redirectTo: '/error/404' }
];

@NgModule({
  declarations: [],
  imports: [RouterModule.forChild(routes)]
})
export class DashboardModule { }
