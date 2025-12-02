import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';
import { UsersPage } from './pages/users';
import { API_BASE_URL_IDENTITY, SrvIdentityClient } from '@shared/proxies/identity-proxies';
import { environment } from 'src/environments/environment';
import { ComponentsModule } from '@components/components-module';

const routes: Routes = [
  { path: 'users', component: UsersPage },
  { path: '**', redirectTo: '/error/404' }
]

@NgModule({
  declarations: [
    UsersPage,
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
