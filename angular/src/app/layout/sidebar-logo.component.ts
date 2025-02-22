import { Component, ChangeDetectionStrategy } from '@angular/core';

@Component({
    selector: 'sidebar-logo',
    templateUrl: './sidebar-logo.component.html',
    changeDetection: ChangeDetectionStrategy.OnPush,
    standalone: false
})
export class SidebarLogoComponent {}
