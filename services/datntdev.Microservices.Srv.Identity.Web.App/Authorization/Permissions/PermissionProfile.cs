using AutoMapper;
using datntdev.Microservices.Srv.Identity.Contract.Authorization.Permissions.Dto;
using datntdev.Microservices.Srv.Identity.Web.App.Authorization.Permissions.Models;
using static datntdev.Microservices.Common.Constants.Enum;

namespace datntdev.Microservices.Srv.Identity.Web.App.Authorization.Permissions
{
    internal class PermissionProfile : Profile
    {
        public PermissionProfile()
        {
            CreateMap<PermissionModel, PermissionDto>();
            CreateMap<IEnumerable<AppPermission>, IEnumerable<PermissionDto>>().ConvertUsing<PermissionConverter>();
        }
    }

    internal class PermissionConverter(PermissionProvider permissionProvider) :
        ITypeConverter<IEnumerable<AppPermission>, IEnumerable<PermissionDto>>
    {
        private readonly PermissionProvider _permissionProvider = permissionProvider;

        public IEnumerable<PermissionDto> Convert(
            IEnumerable<AppPermission> source,
            IEnumerable<PermissionDto>? destination,
            ResolutionContext context)
        {
            var fullPermissions = _permissionProvider.GetAllPermissions().ToList();
            var permissionDtos = context.Mapper.Map<List<PermissionDto>>(fullPermissions);
            permissionDtos.ForEach(x => x.IsGranted = source.Contains(x.Permission));
            return permissionDtos;
        }
    }
}
