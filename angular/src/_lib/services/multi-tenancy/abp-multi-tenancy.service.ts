import { Injectable } from '@angular/core';

@Injectable({
    providedIn: 'root'
})
export class AbpMultiTenancyService {

    get isEnabled(): boolean {
        return abp.multiTenancy.isEnabled;
    }

}