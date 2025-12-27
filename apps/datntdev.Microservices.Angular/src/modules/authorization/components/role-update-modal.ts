import { Component, EventEmitter, inject, OnInit, Output, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { SrvIdentityClient, RoleDto, RoleUpdateDto } from '@shared/proxies/identity-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { PermissionTreeComponent } from './permission-tree';

@Component({
  standalone: false,
  selector: 'app-role-update-modal',
  templateUrl: './role-update-modal.html',
})
export class RoleUpdateModalComponent implements OnInit {
  @ViewChild('modal', { static: false }) modal!: ModalDirective;
  @ViewChild(PermissionTreeComponent) permissionTree!: PermissionTreeComponent;
  @Output() updated = new EventEmitter<void>();

  private readonly clientIdentitySrv = inject(SrvIdentityClient);
  private readonly fb = inject(FormBuilder);

  form!: FormGroup;
  isLoading: boolean = false;
  editingRole: RoleDto | null = null;
  originalPermissions: number[] = [];

  ngOnInit(): void {
    this.form = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(3)]],
      description: ['']
    });
  }

  show(role: RoleDto): void {
    this.editingRole = role;
    
    // Build permission tree from cached data
    this.permissionTree.buildTree();
    
    // Fetch full role details from API
    this.clientIdentitySrv.roles_Get(role.id!).subscribe(roleDetails => {
      this.originalPermissions = roleDetails.permissions || [];
      
      this.form.patchValue({
        name: roleDetails.name,
        description: roleDetails.description
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
    if (this.form.invalid || !this.editingRole) {
      this.form.markAllAsTouched();
      return;
    }

    this.isLoading = true;
    const currentPermissions = this.permissionTree.getSelectedPermissions();
    
    // Calculate permissions to append and remove
    const appendPermissions = currentPermissions.filter(p => !this.originalPermissions.includes(p));
    const removePermissions = this.originalPermissions.filter(p => !currentPermissions.includes(p));

    const data = new RoleUpdateDto({
      name: this.form.value.name,
      description: this.form.value.description,
      appendPermissions: appendPermissions.length > 0 ? appendPermissions : undefined,
      removePermissions: removePermissions.length > 0 ? removePermissions : undefined
    });

    this.clientIdentitySrv.roles_Update(this.editingRole.id!, data)
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
