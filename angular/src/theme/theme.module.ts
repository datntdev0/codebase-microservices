import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDialogModule } from '@angular/material/dialog';
import { MatInputModule } from '@angular/material/input';

@NgModule({
  declarations: [],
  imports: [
    CommonModule
  ]
})
export class ThemeModule {
  public static MaterialModules = [
    MatButtonModule,
    MatDialogModule,
    MatInputModule,
    MatCheckboxModule,
  ]
}
