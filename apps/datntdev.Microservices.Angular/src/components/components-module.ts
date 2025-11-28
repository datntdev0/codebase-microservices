import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DatatableComponent } from './datatable/datatable';
import { PaginatorComponent } from './paginator/paginator';
import { ToastComponent } from './toast/toast';
import { DialogComponent } from './confirmation-dialog/dialog';
import { FormsModule } from '@angular/forms';
import { BsModalService, ModalModule } from 'ngx-bootstrap/modal';

@NgModule({
  declarations: [
    DatatableComponent,
    PaginatorComponent,
    ToastComponent,
    DialogComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    ModalModule,
  ],
  exports: [
    ModalModule,
    DatatableComponent,
    PaginatorComponent,
    ToastComponent,
    DialogComponent,
  ], 
  providers: [
    BsModalService,
  ]
})
export class ComponentsModule { }
