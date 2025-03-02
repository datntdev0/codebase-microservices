import { Component, Renderer2 } from '@angular/core';
import { LayoutStoreService } from '@shared/layout/layout-store.service';

@Component({
  selector: 'app-sidebar',
  templateUrl: 'sidebar.component.html',
  standalone: false
})
export class SidebarComponent {
  sidebarExpanded: boolean;

  constructor(
    private renderer: Renderer2,
    protected layoutStore: LayoutStoreService
  ) { }

  toggleSidebar(): void {
    this.layoutStore.toggleSidebar();
  }

  showSidebar(): void {
    this.renderer.removeClass(document.body, 'sidebar-collapse');
    this.renderer.addClass(document.body, 'sidebar-open');
  }

  hideSidebar(): void {
    this.renderer.removeClass(document.body, 'sidebar-open');
    this.renderer.addClass(document.body, 'sidebar-collapse');
  }
}
