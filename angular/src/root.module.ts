import { NgModule, LOCALE_ID, provideExperimentalZonelessChangeDetection, inject, provideAppInitializer } from '@angular/core';
import { BrowserModule, provideClientHydration } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { ModalModule } from 'ngx-bootstrap/modal';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { CollapseModule } from 'ngx-bootstrap/collapse';
import { TabsModule } from 'ngx-bootstrap/tabs';

import { AbpHttpInterceptor } from '_lib/interceptors/abpHttpInterceptor';

import { SharedModule } from '@shared/shared.module';
import { ServiceProxyModule } from '@shared/service-proxies/service-proxy.module';
import { RootRoutingModule } from './root-routing.module';
import { AppConsts } from '@shared/AppConsts';
import { API_BASE_URL } from '@shared/service-proxies/service-proxies';

import { RootComponent } from './root.component';
import { AppInitializer } from './app-initializer';

export function getCurrentLanguage(): string {
  if (abp.localization.currentLanguage.name) {
    return abp.localization.currentLanguage.name;
  }

  // todo: Waiting for https://github.com/angular/angular/issues/31465 to be fixed.
  return 'en';
}

@NgModule({
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    SharedModule.forRoot(),
    ModalModule.forRoot(),
    BsDropdownModule.forRoot(),
    CollapseModule.forRoot(),
    TabsModule.forRoot(),
    ServiceProxyModule,
    RootRoutingModule,
  ],
  declarations: [RootComponent],
  providers: [
    provideExperimentalZonelessChangeDetection(),
    provideClientHydration(),
    { provide: HTTP_INTERCEPTORS, useClass: AbpHttpInterceptor, multi: true },
    provideAppInitializer(() => {
        const initializerFn = ((appInitializer: AppInitializer) => appInitializer.init())(inject(AppInitializer));
        return initializerFn();
      }),
    { provide: API_BASE_URL, useFactory: () => AppConsts.remoteServiceBaseUrl },
    {
      provide: LOCALE_ID,
      useFactory: getCurrentLanguage,
    },
  ],
  bootstrap: [RootComponent],
})
export class RootModule {}
