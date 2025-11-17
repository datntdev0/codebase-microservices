import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Datatable } from './datatable/datatable';
import { Paginator } from './paginator/paginator';
import { FormsModule } from '@angular/forms';



@NgModule({
  declarations: [
    Datatable,
    Paginator,
  ],
  imports: [
    CommonModule,
    FormsModule,
  ],
  exports: [
    Datatable,
    Paginator,
  ]
})
export class ComponentsModule { }
