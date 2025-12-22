import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';
import { ComponentsModule } from '@components/components-module';
import { API_BASE_URL_IDENTITY, SrvIdentityClient } from '@shared/proxies/identity-proxies';
import { environment } from 'src/environments/environment';
import { RolesPage } from './pages/roles';
import { UsersPage } from './pages/users';

const routes: Routes = [
  { path: 'users', component: UsersPage },
  { path: 'roles', component: RolesPage },
  { path: '**', redirectTo: '/error/404' }
]

@NgModule({
  declarations: [
    UsersPage,
    RolesPage,
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterModule.forChild(routes),
    ComponentsModule
  ],
  providers: [
    SrvIdentityClient,
    { provide: API_BASE_URL_IDENTITY, useValue: environment.apiurl.srvIdentity },
  ],
})
export class AuthorizationModule { }
