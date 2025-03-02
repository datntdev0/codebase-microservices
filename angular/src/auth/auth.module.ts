import { CommonModule } from '@angular/common';
import { HttpClientJsonpModule, HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { ServiceProxyModule } from '@shared/service-proxies/service-proxy.module';
import { SharedModule } from '@shared/shared.module';
import { AuthRoutingModule } from './auth-routing.module';
import { AuthComponent } from './auth.component';
import { LanguageChangeComponent } from './components/language-change.component';
import { TenantChangeDialogComponent } from './components/tenant-change-dialog.component';
import { TenantChangeComponent } from './components/tenant-change.component';
import { LoginComponent } from './containers/login.component';
import { RegisterComponent } from './containers/register.component';
import { ThemeModule } from 'theme/theme.module';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        HttpClientModule,
        HttpClientJsonpModule,
        SharedModule,
        ServiceProxyModule,
        AuthRoutingModule,
        MatButtonModule,
        ThemeModule.MaterialModules,
    ],
    declarations: [
        AuthComponent,
        LoginComponent,
        RegisterComponent,
        LanguageChangeComponent,
        TenantChangeComponent,
        TenantChangeDialogComponent,
    ]
})
export class AuthModule { }
