import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { NgModule, provideAppInitializer, provideBrowserGlobalErrorListeners } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule, Routes } from '@angular/router';

import { RootComponent } from './root';

import { ErrorLayout } from './layout/error-layout';
import { MainLayout } from './layout/main-layout';

import { authGuard } from './shared/guards/auth-guard';
import { appInitializerFactory } from './shared/services/app-initializer';

import { SigninCallback } from './shared/pages/callbacks/signin-callback';
import { Error403Page } from './shared/pages/error403/error403';
import { Error404Page } from './shared/pages/error404/error404';
import { Error500Page } from './shared/pages/error500/error500';

const routes: Routes = [
  {
    path: "error", component: ErrorLayout, children: [
      { path: '403', component: Error403Page },
      { path: '404', component: Error404Page },
      { path: '500', component: Error500Page },
      { path: '**', redirectTo: '/error/404' }
    ]
  },
  {
    path: "app", component: MainLayout, canActivate: [authGuard], children: [
      { path: "dashboard", loadChildren: () => import('./modules/dashboard/dashboard-module').then(m => m.DashboardModule) }, 
      { path: "tenancy", loadChildren: () => import('./modules/tenancy/tenancy-module').then(m => m.TenancyModule) },
      { path: '**', redirectTo: '/error/404' },
    ]
  },
  { path: 'auth/callback', component: SigninCallback },
  { path: '', redirectTo: '/app/dashboard', pathMatch: 'full' },
  { path: '**', redirectTo: '/error/404' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class RootRoutingModule { }

@NgModule({
  declarations: [
    RootComponent
  ],
  imports: [
    BrowserModule,
    RootRoutingModule
  ],
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideAppInitializer(appInitializerFactory),
    provideHttpClient(withInterceptorsFromDi())
  ],
  bootstrap: [RootComponent]
})
export class RootModule { }
