using System;
using datntdev.Abp.Domain.Uow;
using Microsoft.AspNetCore.Http;

namespace datntdev.Abp.Web.Core.Uow;

public class UnitOfWorkMiddlewareOptions
{
    public Func<HttpContext, bool> Filter { get; set; } = context => true;

    public Func<HttpContext, UnitOfWorkOptions> OptionsFactory { get; set; } = context => new UnitOfWorkOptions();
}
