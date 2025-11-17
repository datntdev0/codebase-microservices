import { NgModule } from '@angular/core';
import { UserManager } from 'oidc-client-ts';
import { authConfig } from './models/config';

@NgModule({
  providers: [
    { provide: UserManager, useValue: new UserManager(authConfig) }
  ]
})
export class SharedModule { }
