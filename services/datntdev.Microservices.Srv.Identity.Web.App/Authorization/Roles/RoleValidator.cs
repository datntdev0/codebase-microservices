using datntdev.Microservices.Srv.Identity.Contract.Authorization.Roles.Dto;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using static datntdev.Microservices.Common.Constants.Enum;

namespace datntdev.Microservices.Srv.Identity.Web.App.Authorization.Roles
{
    internal class RoleCreateValidator : AbstractValidator<RoleCreateDto>
    {
        private readonly SrvIdentityDbContext _dbContext;

        public RoleCreateValidator(IServiceProvider services)
        {
            _dbContext = services.GetRequiredService<SrvIdentityDbContext>();

            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Permissions).Must(CheckPermissions);

            RuleFor(x => x.Name)
                .Must((role, name) => CheckDuplicatedName(role.TenantId, name))
                .WithMessage(v => $"The role name '{v}' already exists for this tenant.");
        }

        public bool CheckDuplicatedName(int? tenantId, string name)
        {
            return !_dbContext.AppRoles.Any(r => r.Name == name && r.TenantId == tenantId && !r.IsDeleted);
        }

        public static bool CheckPermissions(AppPermission[] permissions)
        {
            if (permissions.Length == 0) return true;
            return !permissions.Any(x => !Enum.IsDefined(x));
        }
    }

    internal class RoleUpdateValidator : AbstractValidator<(int, RoleUpdateDto)>
    {
        private readonly SrvIdentityDbContext _dbContext;

        public RoleUpdateValidator(IServiceProvider services)
        {
            _dbContext = services.GetRequiredService<SrvIdentityDbContext>();

            RuleFor(x => x.Item2.Name).NotEmpty();
            RuleFor(x => x.Item2.Description).NotEmpty();
            RuleFor(x => x.Item2.RemovePermissions).Must(CheckPermissions);
            RuleFor(x => x.Item2.AppendPermissions).Must(CheckPermissions);

            RuleFor(x => x.Item2.Name)
                .Must((req, name) => CheckDuplicatedName(req.Item1, name))
                .WithMessage(v => $"The role name '{v}' already exists for this tenant.");
        }

        public bool CheckDuplicatedName(int id, string name)
        {
            var role = _dbContext.AppRoles.FirstOrDefault(r => r.Id == id && !r.IsDeleted);
            if (role == null) return true; // Role doesn't exist, validation will fail elsewhere
            
            return !_dbContext.AppRoles.Any(r => r.Id != id && r.Name == name && r.TenantId == role.TenantId && !r.IsDeleted);
        }

        public static bool CheckPermissions(AppPermission[] permissions)
        {
            if (permissions.Length == 0) return true;
            return !permissions.Any(x => !Enum.IsDefined(x));
        }
    }
}
