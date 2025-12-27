import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DatatableComponent } from './datatable/datatable';
import { PaginatorComponent } from './paginator/paginator';
import { ToastComponent } from './toast/toast';
import { DialogComponent } from './dialog/dialog';
import { FormsModule } from '@angular/forms';
import { BsModalService, ModalModule } from 'ngx-bootstrap/modal';
import { TooltipModule } from 'ngx-bootstrap/tooltip';

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
    TooltipModule,
  ],
  exports: [
    ModalModule,
    TooltipModule,
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
