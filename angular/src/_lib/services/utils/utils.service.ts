﻿import {Injectable} from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class UtilsService {
  getCookieValue(key: string): string {
    return abp.utils.getCookieValue(key);
  }

  setCookieValue(key: string, value: string, expireDate?: Date, path?: string, domain?: string, attributes?: any): void {
    abp.utils.setCookieValue(key, value, expireDate, path, domain, attributes);
  }

  deleteCookie(key: string, path?: string): void {
    abp.utils.deleteCookie(key, path);
  }
}
