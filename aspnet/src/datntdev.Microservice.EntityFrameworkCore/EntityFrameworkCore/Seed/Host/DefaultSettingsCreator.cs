using datntdev.Abp.Configuration;
using datntdev.Abp.Localization;
using datntdev.Abp.MultiTenancy;
using datntdev.Abp.Net.Mail;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace datntdev.Microservice.EntityFrameworkCore.Seed.Host;

public class DefaultSettingsCreator
{
    private readonly MicroserviceDbContext _context;

    public DefaultSettingsCreator(MicroserviceDbContext context)
    {
        _context = context;
    }

    public void Create()
    {
        int? tenantId = null;

        if (MicroserviceConsts.MultiTenancyEnabled == false)
        {
            tenantId = MultiTenancyConsts.DefaultTenantId;
        }

        // Emailing
        AddSettingIfNotExists(EmailSettingNames.DefaultFromAddress, "admin@mydomain.com", tenantId);
        AddSettingIfNotExists(EmailSettingNames.DefaultFromDisplayName, "mydomain.com mailer", tenantId);

        // Languages
        AddSettingIfNotExists(LocalizationSettingNames.DefaultLanguage, "en", tenantId);
    }

    private void AddSettingIfNotExists(string name, string value, int? tenantId = null)
    {
        if (_context.Settings.IgnoreQueryFilters().Any(s => s.Name == name && s.TenantId == tenantId && s.UserId == null))
        {
            return;
        }

        _context.Settings.Add(new Setting(tenantId, null, name, value));
        _context.SaveChanges();
    }
}
