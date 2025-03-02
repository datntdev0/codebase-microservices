import { Component } from '@angular/core';
import { LayoutStoreService } from '@shared/layout/layout-store.service';

@Component({
  selector: 'app-navbar-mobile-menu',
  templateUrl: 'navbar-mobile-menu.component.html',
  standalone: false
})
export class NavbarMobileMenuComponent {
  constructor(protected layoutStore: LayoutStoreService) { }
}
