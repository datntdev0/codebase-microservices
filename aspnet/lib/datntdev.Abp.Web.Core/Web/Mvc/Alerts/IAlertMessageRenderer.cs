using System;
using System.Collections.Generic;
using System.Text;
using datntdev.Abp.Dependency;

namespace datntdev.Abp.Web.Mvc.Alerts
{
    public interface IAlertMessageRenderer : ITransientDependency
    {
        string DisplayType { get; }
        string Render(List<AlertMessage> alertList);
    }
}
