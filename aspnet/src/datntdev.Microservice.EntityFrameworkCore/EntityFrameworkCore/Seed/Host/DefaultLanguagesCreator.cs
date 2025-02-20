using datntdev.Abp.Localization;
using datntdev.Abp.MultiTenancy;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace datntdev.Microservice.EntityFrameworkCore.Seed.Host;

public class DefaultLanguagesCreator
{
    public static List<ApplicationLanguage> InitialLanguages => GetInitialLanguages();

    private readonly MicroserviceDbContext _context;

    private static List<ApplicationLanguage> GetInitialLanguages()
    {
        var tenantId = MicroserviceConsts.MultiTenancyEnabled ? null : (int?)MultiTenancyConsts.DefaultTenantId;
        return
        [
            new(tenantId, "en", "English", "famfamfam-flags us"),
            new(tenantId, "vi", "Tiếng Việt", "famfamfam-flags vn"),
        ];
    }

    public DefaultLanguagesCreator(MicroserviceDbContext context)
    {
        _context = context;
    }

    public void Create()
    {
        CreateLanguages();
    }

    private void CreateLanguages()
    {
        foreach (var language in InitialLanguages)
        {
            AddLanguageIfNotExists(language);
        }
    }

    private void AddLanguageIfNotExists(ApplicationLanguage language)
    {
        if (_context.Languages.IgnoreQueryFilters().Any(l => l.TenantId == language.TenantId && l.Name == language.Name))
        {
            return;
        }

        _context.Languages.Add(language);
        _context.SaveChanges();
    }
}
