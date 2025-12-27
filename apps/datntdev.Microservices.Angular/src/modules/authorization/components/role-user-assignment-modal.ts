import { Component, EventEmitter, inject, Output, ViewChild } from '@angular/core';
import { SrvIdentityClient, RoleDto, RoleUpdateDto } from '@shared/proxies/identity-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { LocalDateTimePipe } from '@shared/pipes/local-datetime.pipe';
import { DatatableColumn } from '@components/datatable/datatable';
import { forkJoin } from 'rxjs';

interface UserWithAssignment {
  id: number;
  username: string;
  firstName: string;
  lastName: string;
  isAssigned: boolean;
  assignedDate?: Date;
  assignedBy?: string;
}

@Component({
  standalone: false,
  selector: 'app-role-user-assignment-modal',
  templateUrl: './role-user-assignment-modal.html',
})
export class RoleUserAssignmentModalComponent {
  @ViewChild('modal', { static: false }) modal!: ModalDirective;
  @ViewChild('datatable') datatable: any;
  @Output() updated = new EventEmitter<void>();

  private readonly clientIdentitySrv = inject(SrvIdentityClient);
  private readonly localDateTimePipe = new LocalDateTimePipe();

  isLoading: boolean = false;
  editingRole: RoleDto | null = null;
  users: UserWithAssignment[] = [];
  originalUserIds: Set<number> = new Set();

  // Pagination
  currentPage: number = 1;
  pageSize: number = 10;
  totalUsers: number = 0;

  columns: DatatableColumn[] = [
    {
      key: 'username',
      title: 'Username',
      template: (item) =>
        `<span class="text-gray-800 text-hover-primary mb-1">${item.username}</span>`,
    },
    {
      key: 'firstName',
      title: 'First Name',
    },
    {
      key: 'lastName',
      title: 'Last Name',
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
    return Math.ceil(this.totalUsers / this.pageSize);
  }

  show(role: RoleDto): void {
    this.editingRole = role;
    this.originalUserIds.clear();
    this.currentPage = 1;
    this.loadUsers();
    this.modal.show();
  }

  hide(): void {
    this.modal.hide();
  }

  loadUsers(): void {
    if (!this.editingRole) return;

    this.isLoading = true;
    const offset = (this.currentPage - 1) * this.pageSize;

    // Fetch all users and role's assigned users
    forkJoin({
      allUsers: this.clientIdentitySrv.users_GetAll(offset, this.pageSize),
      roleUsers: this.clientIdentitySrv.roles_GetAllUsers(this.editingRole.id!, 0, 1000) // Get all role users
    }).subscribe({
      next: (result) => {
        this.totalUsers = result.allUsers.total ?? 0;
        
        // Create a map of assigned users for quick lookup
        const assignedUsersMap = new Map(
          (result.roleUsers.items || []).map(ru => [
            ru.id!,
            { assignedDate: ru.createdAt, assignedBy: ru['createdBy'] }
          ])
        );

        // Combine all users with assignment status
        this.users = (result.allUsers.items || []).map(user => {
          const assignmentInfo = assignedUsersMap.get(user.id!);
          const isAssigned = !!assignmentInfo;
          
          if (isAssigned) {
            this.originalUserIds.add(user.id!);
          }

          return {
            id: user.id!,
            username: user.username!,
            firstName: user.firstName!,
            lastName: user.lastName!,
            isAssigned,
            assignedDate: assignmentInfo?.assignedDate,
            assignedBy: assignmentInfo?.assignedBy
          };
        });

        // Pre-select assigned users in datatable
        setTimeout(() => {
          if (this.datatable) {
            this.datatable.selectedItems.clear();
            this.users.forEach(user => {
              if (user.isAssigned) {
                this.datatable.selectedItems.add(user);
              }
            });
            this.datatable.allSelected = this.datatable.selectedItems.size === this.users.length;
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
    this.loadUsers();
  }

  getSelectedUsers(): UserWithAssignment[] {
    return this.datatable?.selectedItems ? Array.from(this.datatable.selectedItems) : [];
  }

  onSubmit(): void {
    if (!this.editingRole) return;

    this.isLoading = true;

    // Get selected users from datatable
    const selectedUsers = this.getSelectedUsers();
    const currentUserIds = selectedUsers.map(u => u.id);
    const originalUserIds = Array.from(this.originalUserIds);
    
    const appendUserIds = currentUserIds.filter(id => !originalUserIds.includes(id));
    const removeUserIds = originalUserIds.filter(id => !currentUserIds.includes(id));

    const data = new RoleUpdateDto({
      name: this.editingRole.name,
      description: this.editingRole.description,
      appendUserIds: appendUserIds.length > 0 ? appendUserIds : undefined,
      removeUserIds: removeUserIds.length > 0 ? removeUserIds : undefined
    });

    this.clientIdentitySrv.roles_Update(this.editingRole.id!, data)
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
