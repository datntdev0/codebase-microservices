import { Injectable } from '@angular/core';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { DialogComponent } from './dialog';
import { Observable, Subject } from 'rxjs';

export interface DialogConfig {
  title: string;
  message: string;
  confirmText?: string;
  cancelText?: string;
  confirmButtonClass?: string;
  cancelButtonClass?: string;
  icon?: string;
  iconColor?: string;
}

@Injectable({ providedIn: 'root' })
export class DialogService {
  private modalRef?: BsModalRef;

  constructor(private modalService: BsModalService) { }

  private confirm(config: DialogConfig): Observable<boolean> {
    const resultSubject = new Subject<boolean>();

    const initialState = {
      ...config,
      onConfirm: () => {
        resultSubject.next(true);
        resultSubject.complete();
        this.modalRef?.hide();
      },
      onCancel: () => {
        resultSubject.next(false);
        resultSubject.complete();
        this.modalRef?.hide();
      }
    };

    this.modalRef = this.modalService.show(DialogComponent, {
      initialState,
      class: 'modal-dialog-centered',
      backdrop: 'static',
      keyboard: true
    });

    return resultSubject.asObservable();
  }

  public confirmDelete(message: string, title?: string): Observable<boolean> {
    return this.confirm({
      title: title || 'Confirm Deletion',
      message: message,
      confirmText: 'Delete',
      cancelText: 'Cancel',
      confirmButtonClass: 'btn-danger',
      cancelButtonClass: 'btn-secondary',
      icon: 'bi-trash-fill',
      iconColor: 'text-danger'
    });
  }

  private acknowledge(config: DialogConfig): Observable<boolean> {
    return this.confirm({ ...config, confirmText: config.confirmText || 'OK' });
  }

  public info(message: string, title?: string): Observable<boolean> {
    return this.acknowledge({
      title: title || 'Information',
      message: message,
      confirmButtonClass: 'btn-primary',
      icon: 'bi-info-circle-fill',
      iconColor: 'text-primary',
    });
  }

  public success(message: string, title?: string): Observable<boolean> {
    return this.acknowledge({
      title: title || 'Success',
      message: message,
      confirmButtonClass: 'btn-success',
      icon: 'bi-check-circle-fill',
      iconColor: 'text-success',
    });
  }

  public warning(message: string, title?: string): Observable<boolean> {
    return this.acknowledge({
      title: title || 'Warning',
      message: message,
      confirmButtonClass: 'btn-warning',
      icon: 'bi-exclamation-triangle-fill',
      iconColor: 'text-warning',
    });
  }

  public error(message: string, title?: string): Observable<boolean> {
    return this.acknowledge({
      title: title || 'Error',
      message: message,
      confirmButtonClass: 'btn-danger',
      icon: 'bi-x-circle-fill',
      iconColor: 'text-danger',
    });
  }
}
