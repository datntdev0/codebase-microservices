export interface MenuItem {
  title: string;
  description?: string;
  icon?: string;
  url?: string;
  target?: string;
  children?: MenuItem[];
}

export interface MenuSection {
  heading?: string;
  items: MenuItem[];
}

export const MENU: MenuSection[] = [
  {
    items: [
      {
        title: 'Dashboard',
        description: 'Overview of your application',
        icon: 'bi bi-bar-chart-line-fill',
        url: '/app/dashboard'
      }
    ]
  },
  {
    heading: 'TENANCY',
    items: [
      {
        title: 'Tenant List',
        description: 'Manage application tenants',
        icon: 'bi bi-buildings-fill',
        url: '/app/tenancy/tenants'
      },
      {
        title: 'Edition Plans',
        description: 'Manage application edition plans',
        icon: 'bi bi-grid-fill',
        url: '/app/tenancy/editions'
      },
      {
        title: 'Subscriptions',
        description: 'Manage application subscriptions',
        icon: 'bi bi-receipt',
        url: '/app/tenancy/subscriptions'
      }
    ]
  },
  {
    heading: 'AUTHORIZATION',
    items: [
      {
        title: 'User Management',
        description: 'Manage application users',
        icon: 'bi bi-people-fill',
        url: '/app/authorization/users'
      },
      {
        title: 'Role Management',
        description: 'Manage application roles',
        icon: 'bi bi-person-vcard-fill',
        url: '/app/authorization/roles'
      }
    ]
  },
  {
    heading: 'ADMINISTRATION',
    items: [
      {
        title: 'Audit Logs',
        description: 'View application audit logs',
        icon: 'bi bi-body-text',
        url: '/app/administration/audit-logs'
      },
      {
        title: 'Configuration',
        description: 'Manage application configuration',
        icon: 'bi bi-gear-fill',
        url: '/app/administration/configuration'
      }
    ]
  },
  {
    heading: 'HELP',
    items: [
      {
        title: 'Getting Started',
        icon: 'bi bi-arrow-right-square-fill',
        url: '#',
        target: '_blank'
      },
      {
        title: 'Documentation',
        icon: 'bi bi-info-square-fill',
        url: '#',
        target: '_blank'
      }
    ]
  }
]