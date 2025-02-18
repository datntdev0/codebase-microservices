using System.Collections.Generic;

namespace datntdev.Abp.Zero.Configuration
{
    public interface IRoleManagementConfig
    {
        List<StaticRoleDefinition> StaticRoles { get; }
    }
}