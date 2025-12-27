import { Component, EventEmitter, inject, Output, ViewChild } from '@angular/core';
import { SrvIdentityClient, UserDto, UserUpdateDto } from '@shared/proxies/identity-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { LocalDateTimePipe } from '@shared/pipes/local-datetime.pipe';
import { DatatableColumn } from '@components/datatable/datatable';
import { forkJoin } from 'rxjs';

interface RoleWithAssignment {
  id: number;
  name: string;
  description: string;
  isAssigned: boolean;
  assignedDate?: Date;
  assignedBy?: string;
}

@Component({
  standalone: false,
  selector: 'app-user-role-assignment-modal',
  templateUrl: './user-role-assignment-modal.html',
})
export class UserRoleAssignmentModalComponent {
  @ViewChild('modal', { static: false }) modal!: ModalDirective;
  @ViewChild('datatable') datatable: any;
  @Output() updated = new EventEmitter<void>();

  private readonly clientIdentitySrv = inject(SrvIdentityClient);
  private readonly localDateTimePipe = new LocalDateTimePipe();

  isLoading: boolean = false;
  editingUser: UserDto | null = null;
  roles: RoleWithAssignment[] = [];
  selectedRoleIds: Set<number> = new Set();
  originalRoleIds: Set<number> = new Set();

  // Pagination
  currentPage: number = 1;
  pageSize: number = 10;
  totalRoles: number = 0;

  columns: DatatableColumn[] = [
    {
      key: 'name',
      title: 'Role Name',
      template: (item) =>
        `<span class="text-gray-800 text-hover-primary mb-1">${item.name}</span>`,
    },
    {
      key: 'description',
      title: 'Description',
    },
    {
      key: 'assignedDate',
      title: 'Assigned Date',
      minWidth: "125px",
      template: (item) => item.assignedDate ? this.localDateTimePipe.transform(item.assignedDate) : '-'
    },
    {
      key: 'assignedBy',
      title: 'Assigned By',
      template: (item) => item.assignedBy || '-'
    }
  ];

  get totalPages(): number {
    return Math.ceil(this.totalRoles / this.pageSize);
  }

  show(user: UserDto): void {
    this.editingUser = user;
    this.selectedRoleIds.clear();
    this.originalRoleIds.clear();
    this.currentPage = 1;
    this.loadRoles();
    this.modal.show();
  }

  hide(): void {
    this.modal.hide();
  }

  loadRoles(): void {
    if (!this.editingUser) return;

    this.isLoading = true;
    const offset = (this.currentPage - 1) * this.pageSize;

    // Fetch all roles and user's assigned roles
    forkJoin({
      allRoles: this.clientIdentitySrv.roles_GetAll(offset, this.pageSize),
      userRoles: this.clientIdentitySrv.users_GetAllRoles(this.editingUser.id!, 0, 1000) // Get all user roles
    }).subscribe({
      next: (result) => {
        this.totalRoles = result.allRoles.total ?? 0;
        
        // Create a map of assigned roles for quick lookup
        const assignedRolesMap = new Map(
          (result.userRoles.items || []).map(ur => [
            ur.id!,
            { assignedDate: ur.createdAt, assignedBy: ur.createdBy }
          ])
        );

        // Combine all roles with assignment status
        this.roles = (result.allRoles.items || []).map(role => {
          const assignmentInfo = assignedRolesMap.get(role.id!);
          const isAssigned = !!assignmentInfo;
          
          if (isAssigned) {
            this.originalRoleIds.add(role.id!);
          }

          return {
            id: role.id!,
            name: role.name!,
            description: role.description!,
            isAssigned,
            assignedDate: assignmentInfo?.assignedDate,
            assignedBy: assignmentInfo?.assignedBy
          };
        });

        // Pre-select assigned roles in datatable
        setTimeout(() => {
          if (this.datatable) {
            this.datatable.selectedItems.clear();
            this.roles.forEach(role => {
              if (role.isAssigned) {
                this.datatable.selectedItems.add(role);
              }
            });
            this.datatable.allSelected = this.datatable.selectedItems.size === this.roles.length;
          }
        });

        this.isLoading = false;
      },
      error: (err) => {
        this.isLoading = false;
        throw err;
      }
    });
  }

  onPageChange(page: number): void {
    this.currentPage = page;
    this.loadRoles();
  }

  getSelectedRoles(): RoleWithAssignment[] {
    return this.datatable?.selectedItems ? Array.from(this.datatable.selectedItems) : [];
  }

  onSubmit(): void {
    if (!this.editingUser) return;

    this.isLoading = true;

    // Get selected roles from datatable
    const selectedRoles = this.getSelectedRoles();
    const currentRoleIds = selectedRoles.map(r => r.id);
    const originalRoleIds = Array.from(this.originalRoleIds);
    
    const appendRoleIds = currentRoleIds.filter(id => !originalRoleIds.includes(id));
    const removeRoleIds = originalRoleIds.filter(id => !currentRoleIds.includes(id));

    const data = new UserUpdateDto({
      username: this.editingUser.username,
      emailAddress: this.editingUser.emailAddress,
      firstName: this.editingUser.firstName,
      lastName: this.editingUser.lastName,
      appendRoleIds: appendRoleIds.length > 0 ? appendRoleIds : undefined,
      removeRoleIds: removeRoleIds.length > 0 ? removeRoleIds : undefined
    });

    this.clientIdentitySrv.users_Update(this.editingUser.id!, data)
      .subscribe({
        next: () => {
          this.isLoading = false;
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
    this.hide();
  }
}
