using System.Collections.Generic;
using datntdev.Abp.MultiTenancy;

namespace datntdev.Abp.Domain.Uow
{
    public class ConnectionStringResolveArgs : Dictionary<string, object>
    {
        public MultiTenancySides? MultiTenancySide { get; set; }

        public ConnectionStringResolveArgs(MultiTenancySides? multiTenancySide = null)
        {
            MultiTenancySide = multiTenancySide;
        }
    }
}