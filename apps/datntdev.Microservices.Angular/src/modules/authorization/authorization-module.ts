import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';
import { ComponentsModule } from '@components/components-module';
import { API_BASE_URL_IDENTITY, SrvIdentityClient } from '@shared/proxies/identity-proxies';
import { environment } from 'src/environments/environment';
import { RolesPage } from './pages/roles';
import { UsersPage } from './pages/users';
import { UserCreateModalComponent } from './components/user-create-modal';
import { UserUpdateModalComponent } from './components/user-update-modal';
import { RoleCreateModalComponent } from './components/role-create-modal';
import { RoleUpdateModalComponent } from './components/role-update-modal';
import { PermissionTreeComponent } from './components/permission-tree';
import { PermissionService } from './services/permission-service';

const routes: Routes = [
  { path: 'users', component: UsersPage },
  { path: 'roles', component: RolesPage },
  { path: '**', redirectTo: '/error/404' }
]

@NgModule({
  declarations: [
    UsersPage,
    RolesPage,
    UserCreateModalComponent,
    UserUpdateModalComponent,
    RoleCreateModalComponent,
    RoleUpdateModalComponent,
    PermissionTreeComponent,
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterModule.forChild(routes),
    ComponentsModule
  ],
  providers: [
    SrvIdentityClient, { provide: API_BASE_URL_IDENTITY, useValue: environment.apiurl.srvIdentity },

    PermissionService,
  ],
})
export class AuthorizationModule { }
