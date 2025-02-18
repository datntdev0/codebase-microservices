using System;
using datntdev.Abp.Auditing;
using datntdev.Abp.Domain.Uow;
using datntdev.Abp.Extensions;
using datntdev.Abp.Runtime.Validation;
using Microsoft.AspNetCore.Mvc;

namespace datntdev.Abp.Web.Core.Mvc.Controllers;

public class AbpAppViewController : AbpController
{
    [DisableAuditing]
    [DisableValidation]
    [UnitOfWork(IsDisabled = true)]
    public ActionResult Load(string viewUrl)
    {
        if (viewUrl.IsNullOrEmpty())
        {
            throw new ArgumentNullException(nameof(viewUrl));
        }

        return View(viewUrl.EnsureStartsWith('~'));
    }
}
