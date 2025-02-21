using datntdev.Abp.Application.Editions;
using datntdev.Abp.Application.Features;
using datntdev.Microservice.Editions;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace datntdev.Microservice.EntityFrameworkCore.Seed.Host;

public class DefaultEditionCreator
{
    private readonly MicroserviceDbContext _context;

    public DefaultEditionCreator(MicroserviceDbContext context)
    {
        _context = context;
    }

    public void Create()
    {
        CreateEditions();
    }

    private void CreateEditions()
    {
        var defaultEdition = _context.Editions.IgnoreQueryFilters().FirstOrDefault(e => e.Name == EditionNames.DefaultEditionName);
        if (defaultEdition == null)
        {
            defaultEdition = new Edition { Name = EditionNames.DefaultEditionName, DisplayName = EditionNames.DefaultEditionName };
            _context.Editions.Add(defaultEdition);
            _context.SaveChanges();

            /* Add desired features to the standard edition, if wanted... */
        }
    }

    private void CreateFeatureIfNotExists(int editionId, string featureName, bool isEnabled)
    {
        if (_context.EditionFeatureSettings.IgnoreQueryFilters().Any(ef => ef.EditionId == editionId && ef.Name == featureName))
        {
            return;
        }

        _context.EditionFeatureSettings.Add(new EditionFeatureSetting
        {
            Name = featureName,
            Value = isEnabled.ToString(),
            EditionId = editionId
        });
        _context.SaveChanges();
    }
}
