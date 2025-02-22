import { Injector, ElementRef } from '@angular/core';
import { AppConsts } from '@shared/AppConsts';
import { LocalizationService } from '@lib/services/localization/localization.service';

import { AppSessionService } from '@shared/session/app-session.service';
import { PrimengTableHelper } from 'shared/helpers/PrimengTableHelper';
import { PermissionCheckerService } from '@lib/services/auth/permission-checker.service';
import { AbpMultiTenancyService } from '@lib/services/multi-tenancy/abp-multi-tenancy.service';
import { MessageService } from '@lib/services/message/message.service';
import { FeatureCheckerService } from '@lib/services/features/feature-checker.service';
import { SettingService } from '@lib/services/settings/setting.service';
import { NotifyService } from '@lib/services/notify/notify.service';

export abstract class AppComponentBase {

    localizationSourceName = AppConsts.localization.defaultLocalizationSourceName;

    localization: LocalizationService;
    permission: PermissionCheckerService;
    feature: FeatureCheckerService;
    notify: NotifyService;
    setting: SettingService;
    message: MessageService;
    multiTenancy: AbpMultiTenancyService;
    appSession: AppSessionService;
    elementRef: ElementRef;
    primengTableHelper: PrimengTableHelper;

    constructor(injector: Injector) {
        this.localization = injector.get(LocalizationService);
        this.permission = injector.get(PermissionCheckerService);
        this.feature = injector.get(FeatureCheckerService);
        this.notify = injector.get(NotifyService);
        this.setting = injector.get(SettingService);
        this.message = injector.get(MessageService);
        this.multiTenancy = injector.get(AbpMultiTenancyService);
        this.appSession = injector.get(AppSessionService);
        this.elementRef = injector.get(ElementRef);
        this.primengTableHelper = new PrimengTableHelper();
    }

    l(key: string, ...args: any[]): string {
        let localizedText = this.localization.localize(key, this.localizationSourceName);

        if (!localizedText) {
            localizedText = key;
        }

        if (!args || !args.length) {
            return localizedText;
        }

        args.unshift(localizedText);
        return abp.utils.formatString.apply(this, args);
    }

    isGranted(permissionName: string): boolean {
        return this.permission.isGranted(permissionName);
    }
}
