import { Injectable, signal } from '@angular/core';
import { NAVBAR_NAV_ITEMS, NavItem, NavItemGroup, SIDEBAR_NAV_ITEMS } from './menu-item';

@Injectable()
export class LayoutStoreService {

  private _showSidebar = signal(true);
  private _showMobileMenu = signal(false);
  private _sidebarNavItems = signal<NavItemGroup[]>([]);
  private _navbarNavItems = signal<NavItemGroup[]>([]);

  constructor() {
    this._sidebarNavItems.set(SIDEBAR_NAV_ITEMS);
    this._navbarNavItems.set(NAVBAR_NAV_ITEMS);
  }

  get showSideBar() {
    return this._showSidebar();
  }

  get showMobileMenu() {
    return this._showMobileMenu();
  }

  get sidebarNavItems() {
    return this._sidebarNavItems();
  }

  get navbarNavItems() {
    return this._navbarNavItems();
  }

  public toggleSidebar() {
    this._showSidebar.set(!this._showSidebar());
  }

  public toggleSidebarMenu(navItem: NavItem) {
    this._showSidebar.set(true);
    navItem.expanded = !navItem.expanded;
  }

  public toggleMobileMenu() {
    this._showMobileMenu.set(!this._showMobileMenu());
  }

}
