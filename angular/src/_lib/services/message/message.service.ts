﻿import { Injectable } from '@angular/core';

@Injectable({
    providedIn: 'root'
})
export class MessageService {

    info(message: string, title?: string, options?: any): any {
        return abp.message.info(message, title, options);
    }

    success(message: string, title?: string, options?: any): any {
        return abp.message.success(message, title, options);
    }

    warn(message: string, title?: string, options?: any): any {
        return abp.message.warn(message, title, options);
    }

    error(message: string, title?: string, options?: any): any {
        return abp.message.error(message, title, options);
    }

    confirm(message: string, title?: string, callback?: (result: boolean, info?: any) => void, options?: any): any {
        return abp.message.confirm(message, title, callback, options);
    }

}