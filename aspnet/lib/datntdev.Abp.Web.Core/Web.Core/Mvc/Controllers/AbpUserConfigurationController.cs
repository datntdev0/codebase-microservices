using System.Threading.Tasks;
using datntdev.Abp.Web.Configuration;
using Microsoft.AspNetCore.Mvc;

namespace datntdev.Abp.Web.Core.Mvc.Controllers;

public class AbpUserConfigurationController : AbpController
{
    private readonly AbpUserConfigurationBuilder _abpUserConfigurationBuilder;

    public AbpUserConfigurationController(AbpUserConfigurationBuilder abpUserConfigurationBuilder)
    {
        _abpUserConfigurationBuilder = abpUserConfigurationBuilder;
    }

    public async Task<JsonResult> GetAll()
    {
        var userConfig = await _abpUserConfigurationBuilder.GetAll();
        return Json(userConfig);
    }
}
