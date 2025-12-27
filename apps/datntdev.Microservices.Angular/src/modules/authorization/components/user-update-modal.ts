import { Component, EventEmitter, inject, OnInit, Output, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { SrvIdentityClient, UserDto, UserUpdateDto } from '@shared/proxies/identity-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { PermissionTreeComponent } from './permission-tree';

@Component({
  standalone: false,
  selector: 'app-user-update-modal',
  templateUrl: './user-update-modal.html',
})
export class UserUpdateModalComponent implements OnInit {
  @ViewChild('modal', { static: false }) modal!: ModalDirective;
  @ViewChild(PermissionTreeComponent) permissionTree!: PermissionTreeComponent;
  @Output() updated = new EventEmitter<void>();

  private readonly clientIdentitySrv = inject(SrvIdentityClient);
  private readonly fb = inject(FormBuilder);

  form!: FormGroup;
  isLoading: boolean = false;
  editingUser: UserDto | null = null;
  originalPermissions: number[] = [];

  ngOnInit(): void {
    this.form = this.fb.group({
      username: ['', [Validators.required, Validators.minLength(3)]],
      emailAddress: ['', [Validators.required, Validators.email]],
      firstName: ['', [Validators.required]],
      lastName: ['', [Validators.required]],
      password: ['', [Validators.minLength(6)]]
    });
  }

  show(user: UserDto): void {
    this.editingUser = user;
    
    // Build permission tree from cached data
    this.permissionTree.buildTree();
    
    // Fetch full user details from API
    this.clientIdentitySrv.users_Get(user.id!).subscribe(userDetails => {
      this.originalPermissions = userDetails.permissions || [];
      
      this.form.patchValue({
        username: userDetails.username,
        emailAddress: userDetails.emailAddress,
        firstName: userDetails.firstName,
        lastName: userDetails.lastName
      });

      // Set permissions in tree after form is populated
      this.permissionTree.setSelectedPermissions(this.originalPermissions);
    });

    this.modal.show();
  }

  hide(): void {
    this.modal.hide();
  }

  onSubmit(): void {
    if (this.form.invalid || !this.editingUser) {
      this.form.markAllAsTouched();
      return;
    }

    this.isLoading = true;
    const currentPermissions = this.permissionTree.getSelectedPermissions();
    
    // Calculate permissions to append and remove
    const appendPermissions = currentPermissions.filter(p => !this.originalPermissions.includes(p));
    const removePermissions = this.originalPermissions.filter(p => !currentPermissions.includes(p));

    const data = new UserUpdateDto({
      username: this.form.value.username,
      emailAddress: this.form.value.emailAddress,
      firstName: this.form.value.firstName,
      lastName: this.form.value.lastName,
      password: this.form.value.password || undefined,
      appendPermissions: appendPermissions.length > 0 ? appendPermissions : undefined,
      removePermissions: removePermissions.length > 0 ? removePermissions : undefined
    });

    this.clientIdentitySrv.users_Update(this.editingUser.id!, data)
      .subscribe({
        next: () => {
          this.isLoading = false;
          this.form.reset();
          this.permissionTree.reset();
          this.hide();
          this.updated.emit();
        },
        error: (err) => {
          this.isLoading = false;
          throw err;
        }
      });
  }

  onDiscard(): void {
    this.form.reset();
    this.permissionTree.reset();
    this.hide();
  }
}
