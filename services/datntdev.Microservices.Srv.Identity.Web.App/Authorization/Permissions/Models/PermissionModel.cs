using AutoMapper;
using datntdev.Microservices.Srv.Identity.Contract.Authorization.Permissions.Dto;
using static datntdev.Microservices.Common.Constants.Enum;

namespace datntdev.Microservices.Srv.Identity.Web.App.Authorization.Permissions.Models
{
    [AutoMap(typeof(PermissionDto), ReverseMap = true)]
    public class PermissionModel(
        string permissionName,
        AppPermission permission,
        AppPermission parentPermission = AppPermission.None,
        MultiTenancySide tenancySide = MultiTenancySide.Host | MultiTenancySide.Tenant)
    {
        public string PermissionName { get; set; } = permissionName;
        public AppPermission Parent { get; set; } = parentPermission;
        public AppPermission Permission { get; set; } = permission;
        public MultiTenancySide TenancySide { get; set; } = tenancySide;
    }
}
