export class MenuItem {
  id: number;
  parentId: number;
  label: string;
  route: string;
  icon: string;
  permissionName: string;
  isActive?: boolean;
  isCollapsed?: boolean;
  children: MenuItem[];

  constructor(
    label: string,
    route: string,
    icon: string,
    permissionName: string = null,
    children: MenuItem[] = null
  ) {
    this.label = label;
    this.route = route;
    this.icon = icon;
    this.permissionName = permissionName;
    this.children = children;
  }
}

export class NavItemGroup {
  group: string;
  separator: boolean;
  items: Array<NavItem>;

  constructor(
    group: string,
    items: Array<NavItem>,
    separator: boolean = false
  ) {
    this.group = group;
    this.items = items;
    this.separator = separator;
  }
}

export class NavItem {
  icon: string;
  label: string;
  route: string;
  active: boolean;
  expanded: boolean;
  permission?: string;
  children?: Array<NavItem>;

  constructor(
    icon: string,
    label: string,
    route: string,
    permission?: string,
    children?: Array<NavItem>
  ) {
    this.icon = icon;
    this.label = label;
    this.route = route;
    this.permission = permission;
    this.children = children;
  }
}

export const SIDEBAR_NAV_ITEMS = [
  new NavItemGroup(
    abp.localization.localize("Administration", abp.localization.defaultSourceName), 
    [
      new NavItem("fa-solid fa-building", abp.localization.localize("Tenants", abp.localization.defaultSourceName), "/app/tenants", "Pages.Tenants"),
      new NavItem("fa-solid fa-briefcase", abp.localization.localize("Roles", abp.localization.defaultSourceName), "/app/roles", "Pages.Roles"),
      new NavItem("fa-solid fa-users", abp.localization.localize("Users", abp.localization.defaultSourceName), "/app/users", "Pages.Users"),
    ], true),
  new NavItemGroup(
    abp.localization.localize("Configuration", abp.localization.defaultSourceName), 
    [
      new NavItem("fa-solid fa-folder", abp.localization.localize("Folders", abp.localization.defaultSourceName), undefined, undefined, 
      [
        new NavItem("", abp.localization.localize("File1", abp.localization.defaultSourceName), undefined),
        new NavItem("", abp.localization.localize("File2", abp.localization.defaultSourceName), undefined),
        new NavItem("", abp.localization.localize("File3", abp.localization.defaultSourceName), undefined),
      ])
    ]),
];

export const NAVBAR_NAV_ITEMS = [
  new NavItemGroup(
    abp.localization.localize("Application", abp.localization.defaultSourceName), 
    [
      new NavItem("fa-solid fa-tv", abp.localization.localize("Home", abp.localization.defaultSourceName), "/app/home"),
      new NavItem("fa-solid fa-circle-question", abp.localization.localize("About", abp.localization.defaultSourceName), "/app/about"),
    ], true),
  ...SIDEBAR_NAV_ITEMS
]