import { Injectable, inject } from '@angular/core';
import { Observable, of, tap } from 'rxjs';
import { SrvIdentityClient, PermissionDto } from '@shared/proxies/identity-proxies';

@Injectable()
export class PermissionService {
  private readonly clientIdentitySrv = inject(SrvIdentityClient);
  private permissionsCache: PermissionDto[] | null = null;

  getPermissions(): Observable<PermissionDto[]> {
    if (this.permissionsCache) {
      return of(this.permissionsCache);
    }

    return this.clientIdentitySrv.permissions_GetAll().pipe(
      tap(permissions => {
        this.permissionsCache = permissions;
      })
    );
  }

  clearCache(): void {
    this.permissionsCache = null;
  }
}
