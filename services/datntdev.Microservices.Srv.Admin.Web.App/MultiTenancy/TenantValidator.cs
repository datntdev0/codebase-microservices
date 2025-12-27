using datntdev.Microservices.Srv.Admin.Contract.MultiTenancy.Dto;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace datntdev.Microservices.Srv.Admin.Web.App.MultiTenancy
{
    internal class TenantCreateValidator : AbstractValidator<TenantCreateDto>
    {
        private readonly SrvAdminDbContext _dbContext;

        public TenantCreateValidator(IServiceProvider services)
        {
            _dbContext = services.GetRequiredService<SrvAdminDbContext>();

            RuleFor(x => x.TenantName)
                .NotEmpty()
                .Must(CheckDuplicatedName)
                .WithMessage(name => $"The tenant name '{name}' is already existed.");
        }

        public bool CheckDuplicatedName(string tenantName)
        {
            return !_dbContext.AppTenants.Any(t => t.TenantName == tenantName);
        }
    }

    internal class TenantUpdateValidator : AbstractValidator<(int, TenantUpdateDto)>
    {
        private readonly SrvAdminDbContext _dbContext;

        public TenantUpdateValidator(IServiceProvider services)
        {
            _dbContext = services.GetRequiredService<SrvAdminDbContext>();

            RuleFor(x => x.Item2.TenantName)
                .NotEmpty()
                .Must((req, name) => CheckDuplicatedName(req.Item1, name))
                .WithMessage(name => $"The tenant name '{name}' is already existed.");
        }

        public bool CheckDuplicatedName(int id, string tenantName)
        {
            return !_dbContext.AppTenants.Any(t => t.Id != id && t.TenantName == tenantName);
        }
    }
}
