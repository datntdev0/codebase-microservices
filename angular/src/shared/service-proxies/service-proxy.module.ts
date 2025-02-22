import { NgModule } from '@angular/core';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AbpHttpInterceptor } from 'abp-ng2-module';

import * as ApiServiceProxies from './service-proxies';

@NgModule({
    providers: [
        ApiServiceProxies.AuthServiceProxy,
        ApiServiceProxies.SessionServiceProxy,
        ApiServiceProxies.ConfigsServiceProxy,
        ApiServiceProxies.TenantsServiceProxy,
        ApiServiceProxies.RolesServiceProxy,
        ApiServiceProxies.UsersServiceProxy,
        { provide: HTTP_INTERCEPTORS, useClass: AbpHttpInterceptor, multi: true }
    ]
})
export class ServiceProxyModule { }
