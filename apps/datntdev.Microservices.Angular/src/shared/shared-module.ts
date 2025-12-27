import { NgModule } from '@angular/core';
import { UserManager } from 'oidc-client-ts';
import { authConfig } from './models/config';
import { LocalDateTimePipe } from './pipes/local-datetime.pipe';

@NgModule({
  declarations: [
    LocalDateTimePipe
  ],
  exports: [
    LocalDateTimePipe
  ],
  providers: [
    { provide: UserManager, useValue: new UserManager(authConfig) },
    LocalDateTimePipe
  ]
})
export class SharedModule { }
