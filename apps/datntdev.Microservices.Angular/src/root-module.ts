import { NgModule, provideBrowserGlobalErrorListeners } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { RootComponent } from './root';
import { RouterModule, Routes } from '@angular/router';
import { WelcomePage } from './pages/welcome/welcome';

const routes: Routes = [
  { path: "", component: WelcomePage }
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
    provideBrowserGlobalErrorListeners()
  ],
  bootstrap: [RootComponent]
})
export class RootModule { }
