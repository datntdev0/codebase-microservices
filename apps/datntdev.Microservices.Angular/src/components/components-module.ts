import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Datatable } from './datatable/datatable';
import { Paginator } from './paginator/paginator';
import { FormsModule } from '@angular/forms';
import { ModalModule } from 'ngx-bootstrap/modal';

@NgModule({
  declarations: [
    Datatable,
    Paginator,
  ],
  imports: [
    CommonModule,
    FormsModule,
    ModalModule,
  ],
  exports: [
    Datatable,
    Paginator,
    ModalModule,
  ], 
})
export class ComponentsModule { }
