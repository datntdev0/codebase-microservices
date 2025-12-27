import { Component, EventEmitter, inject, OnInit, Output, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { SrvIdentityClient, UserCreateDto } from '@shared/proxies/identity-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { PermissionTreeComponent } from './permission-tree';

@Component({
  standalone: false,
  selector: 'app-user-create-modal',
  templateUrl: './user-create-modal.html',
})
export class UserCreateModalComponent implements OnInit {
  @ViewChild('modal', { static: false }) modal!: ModalDirective;
  @ViewChild(PermissionTreeComponent) permissionTree!: PermissionTreeComponent;
  @Output() created = new EventEmitter<void>();

  private readonly clientIdentitySrv = inject(SrvIdentityClient);
  private readonly fb = inject(FormBuilder);

  form!: FormGroup;
  isLoading: boolean = false;

  ngOnInit(): void {
    this.form = this.fb.group({
      username: ['', [Validators.required, Validators.minLength(3)]],
      emailAddress: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      firstName: ['', [Validators.required]],
      lastName: ['', [Validators.required]]
    });
  }

  show(): void {
    this.permissionTree.buildTree();
    this.modal.show();
  }

  hide(): void {
    this.modal.hide();
  }

  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.isLoading = true;
    const selectedPermissions = this.permissionTree.getSelectedPermissions();
    
    const data = new UserCreateDto({
      username: this.form.value.username,
      emailAddress: this.form.value.emailAddress,
      password: this.form.value.password,
      firstName: this.form.value.firstName,
      lastName: this.form.value.lastName,
      permissions: selectedPermissions
    });

    this.clientIdentitySrv.users_Create(data)
      .subscribe({
        next: () => {
          this.isLoading = false;
          this.form.reset();
          this.permissionTree.reset();
          this.hide();
          this.created.emit();
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
