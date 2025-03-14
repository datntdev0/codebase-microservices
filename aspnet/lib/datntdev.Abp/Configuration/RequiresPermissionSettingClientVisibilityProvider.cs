﻿using System.Threading.Tasks;
using datntdev.Abp.Authorization;
using datntdev.Abp.Dependency;
using datntdev.Abp.Runtime.Session;

namespace datntdev.Abp.Configuration
{
    public class RequiresPermissionSettingClientVisibilityProvider : ISettingClientVisibilityProvider
    {
        private readonly IPermissionDependency _permissionDependency;

        public RequiresPermissionSettingClientVisibilityProvider(IPermissionDependency permissionDependency)
        {
            _permissionDependency = permissionDependency;
        }

        public async Task<bool> CheckVisible(IScopedIocResolver scope)
        {
            var abpSession = scope.Resolve<IAbpSession>();

            if (!abpSession.UserId.HasValue)
            {
                return false;
            }

            var permissionDependencyContext = scope.Resolve<PermissionDependencyContext>();
            permissionDependencyContext.User = abpSession.ToUserIdentifier();

            return await _permissionDependency.IsSatisfiedAsync(permissionDependencyContext);
        }
    }
}