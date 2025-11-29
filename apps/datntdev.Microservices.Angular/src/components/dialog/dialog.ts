import { Component } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  standalone: false,
  selector: 'app-dialog',
  templateUrl: './dialog.html',
})
export class DialogComponent {
  public title!: string;
  public message!: string;
  public confirmText?: string;
  public cancelText?: string;
  public confirmButtonClass?: string;
  public cancelButtonClass?: string;
  public icon?: string;
  public iconColor?: string;
  public onConfirm?: () => void;
  public onCancel?: () => void;

  constructor(public bsModalRef: BsModalRef) {}

  confirm(): void {
    if (this.onConfirm) {
      this.onConfirm();
    }
  }

  cancel(): void {
    if (this.onCancel) {
      this.onCancel();
    }
  }
}
