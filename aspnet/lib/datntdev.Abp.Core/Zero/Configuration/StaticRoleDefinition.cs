﻿using System.Collections.Generic;
using datntdev.Abp.Authorization;
using datntdev.Abp.MultiTenancy;

namespace datntdev.Abp.Zero.Configuration
{
    public class StaticRoleDefinition
    {
        public string RoleName { get; }

        public string RoleDisplayName { get; }

        public bool GrantAllPermissionsByDefault { get; set; }
        
        public List<string> GrantedPermissions { get; }

        public MultiTenancySides Side { get; }

        public StaticRoleDefinition(string roleName, MultiTenancySides side, bool grantAllPermissionsByDefault = false)
        {
            RoleName = roleName;
            RoleDisplayName = roleName;
            Side = side;
            GrantAllPermissionsByDefault = grantAllPermissionsByDefault;
            GrantedPermissions = new List<string>();
        }

        public StaticRoleDefinition(string roleName, string roleDisplayName, MultiTenancySides side, bool grantAllPermissionsByDefault = false)
        {
            RoleName = roleName;
            RoleDisplayName = roleDisplayName;
            Side = side;
            GrantAllPermissionsByDefault = grantAllPermissionsByDefault;
            GrantedPermissions = new List<string>();
        }

        public virtual bool IsGrantedByDefault(Permission permission)
        {
            return GrantAllPermissionsByDefault || GrantedPermissions.Contains(permission.Name);
        }
    }
}