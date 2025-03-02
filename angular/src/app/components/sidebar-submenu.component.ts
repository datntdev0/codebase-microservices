import { NgClass, NgFor, NgTemplateOutlet } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { LayoutStoreService } from '@shared/layout/layout-store.service';
import { NavItem } from '@shared/layout/menu-item';

@Component({
  selector: 'app-sidebar-submenu',
  templateUrl: 'sidebar-submenu.component.html',
  standalone: false
})
export class SidebarSubmenuComponent {
  @Input() public submenu = <NavItem>{};

  constructor(protected layoutStore: LayoutStoreService) {}
}
