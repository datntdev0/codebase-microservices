import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DialogService } from '@components/dialog/dialog-service';
import { DatatableColumn } from '@components/datatable/datatable';
import { SrvIdentityClient, UserCreateDto, UserUpdateDto } from '@shared/proxies/identity-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';

@Component({
  standalone: false,
  templateUrl: './users.html',
})
export class UsersPage implements OnInit {
  private readonly clientIdentitySrv = inject(SrvIdentityClient);
  private readonly dialogSrv = inject(DialogService);
  private readonly fb = inject(FormBuilder);

  users: any[] = [];
  editingUser: any = null;
  createForm!: FormGroup;
  updateForm!: FormGroup;
  isLoading: boolean = false;

  columns: DatatableColumn[] = [
    {
      key: 'username',
      title: 'Username',
      template: (item) =>
        `<span class="text-gray-800 text-hover-primary mb-1">${item.username}</span>`,
    },
    {
      key: 'emailAddress',
      title: 'Email Address',
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
      key: 'createdAt',
      title: 'Created Date',
    },
    {
      key: 'updatedAt',
      title: 'Updated Date',
    }
  ];

  ngOnInit(): void {
    this.createForm = this.fb.group({
      username: ['', [Validators.required, Validators.minLength(3)]],
      emailAddress: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      firstName: ['', [Validators.required]],
      lastName: ['', [Validators.required]]
    });
    this.updateForm = this.fb.group({
      emailAddress: ['', [Validators.required, Validators.email]],
      firstName: ['', [Validators.required]],
      lastName: ['', [Validators.required]],
      password: ['', [Validators.minLength(6)]]
    });

    this.clientIdentitySrv.users_GetAll(0, 10).subscribe(users => this.users = users.items ?? []);
  }

  protected onCreate(modal: ModalDirective): void {
    if (this.createForm.invalid) {
      this.createForm.markAllAsTouched();
      return;
    }

    this.isLoading = true;
    const data = new UserCreateDto({
      username: this.createForm.value.username,
      emailAddress: this.createForm.value.emailAddress,
      password: this.createForm.value.password,
      firstName: this.createForm.value.firstName,
      lastName: this.createForm.value.lastName
    })

    this.clientIdentitySrv.users_Create(data)
      .subscribe({
        next: () => {
          this.createForm.reset();
          this.isLoading = false;
          this.ngOnInit();
          modal.hide();
        },
        error: (err) => {
          this.isLoading = false;
          throw err;
        }
      });
  }

  protected onUpdate(modal: ModalDirective): void {
    if (this.updateForm.invalid) {
      this.updateForm.markAllAsTouched();
      return;
    }

    this.isLoading = true;
    const data = new UserUpdateDto({
      emailAddress: this.updateForm.value.emailAddress,
      firstName: this.updateForm.value.firstName,
      lastName: this.updateForm.value.lastName,
      password: this.updateForm.value.password || undefined
    });

    this.clientIdentitySrv.users_Update(this.editingUser.id, data)
      .subscribe({
        next: () => {
          this.updateForm.reset();
          this.isLoading = false;
          this.ngOnInit();
          modal.hide();
        },
        error: (err) => {
          this.isLoading = false;
          throw err;
        }
      });
  }

  protected onEdit(item: any, modal: ModalDirective): void {
    this.editingUser = item;
    this.updateForm.patchValue({ 
      emailAddress: item.emailAddress,
      firstName: item.firstName,
      lastName: item.lastName
    });
    modal.show();
  }

  protected onDelete(item: any): void {
    this.dialogSrv.confirmDelete(`Are you sure you want to delete user "${item.username}"?`)
      .subscribe(confirmed => {
        if (!confirmed) return;

        this.clientIdentitySrv.users_Delete(item.id)
          .subscribe({next: () => this.ngOnInit() });
      });
  }
}
