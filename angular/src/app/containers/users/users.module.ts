import { NgModule } from '@angular/core';
import { SharedModule } from '@shared/shared.module';
import { CreateUserDialogComponent } from './create-user-dialog.component';
import { EditUserDialogComponent } from './edit-user-dialog.component';
import { ResetPasswordDialogComponent } from './reset-password.component';
import { ChangePasswordComponent } from './change-password.component';
import { UsersRoutingModule } from './users-routing.module';
import { UsersComponent } from './users.component';
import { CommonModule } from '@angular/common';

@NgModule({
    declarations: [UsersComponent, ResetPasswordDialogComponent, EditUserDialogComponent, CreateUserDialogComponent, ChangePasswordComponent],
    imports: [SharedModule, UsersRoutingModule, CommonModule],
})
export class UsersModule {}