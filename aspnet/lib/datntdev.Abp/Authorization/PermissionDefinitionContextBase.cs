﻿using System.Collections.Generic;
using datntdev.Abp.Application.Features;
using datntdev.Abp.Collections.Extensions;
using datntdev.Abp.Localization;
using datntdev.Abp.MultiTenancy;

namespace datntdev.Abp.Authorization
{
    public abstract class PermissionDefinitionContextBase : IPermissionDefinitionContext
    {
        protected readonly PermissionDictionary Permissions;

        protected PermissionDefinitionContextBase()
        {
            Permissions = new PermissionDictionary();
        }

        public Permission CreatePermission(
            string name,
            ILocalizableString displayName = null,
            ILocalizableString description = null,
            MultiTenancySides multiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant,
            IFeatureDependency featureDependency = null,
            Dictionary<string, object> properties = null)
        {
            if (Permissions.ContainsKey(name))
            {
                throw new AbpException("There is already a permission with name: " + name);
            }

            var permission = new Permission(name, displayName, description, multiTenancySides, featureDependency, properties);
            Permissions[permission.Name] = permission;
            return permission;
        }

        public virtual Permission GetPermissionOrNull(string name)
        {
            return Permissions.GetOrDefault(name);
        }

        public virtual void RemovePermission(string name)
        {
            Permissions.Remove(name);
        }
    }
}