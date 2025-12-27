import { Component, EventEmitter, inject, OnInit, Output, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { SrvIdentityClient, RoleCreateDto } from '@shared/proxies/identity-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { PermissionTreeComponent } from './permission-tree';

@Component({
  standalone: false,
  selector: 'app-role-create-modal',
  templateUrl: './role-create-modal.html',
})
export class RoleCreateModalComponent implements OnInit {
  @ViewChild('modal', { static: false }) modal!: ModalDirective;
  @ViewChild(PermissionTreeComponent) permissionTree!: PermissionTreeComponent;
  @Output() created = new EventEmitter<void>();

  private readonly clientIdentitySrv = inject(SrvIdentityClient);
  private readonly fb = inject(FormBuilder);

  form!: FormGroup;
  isLoading: boolean = false;

  ngOnInit(): void {
    this.form = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(3)]],
      description: ['']
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
    
    const data = new RoleCreateDto({
      name: this.form.value.name,
      description: this.form.value.description,
      permissions: selectedPermissions
    });

    this.clientIdentitySrv.roles_Create(data)
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
