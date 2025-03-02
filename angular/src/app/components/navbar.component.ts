import { Component } from '@angular/core';
import { LayoutStoreService } from '@shared/layout/layout-store.service';

@Component({
  selector: 'app-navbar',
  templateUrl: 'navbar.component.html',
  standalone: false
})
export class NavbarComponent {
  constructor(protected layoutStore: LayoutStoreService) { }
}
