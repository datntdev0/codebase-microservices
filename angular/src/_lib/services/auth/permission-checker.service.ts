import { Injectable } from '@angular/core';

@Injectable({
    providedIn: 'root'
})
export class PermissionCheckerService {

    isGranted(permissionName: string): boolean {
        return abp.auth.isGranted(permissionName);
    }

}