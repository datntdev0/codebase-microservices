using System;

namespace datntdev.Abp.EntityFramework
{
    public interface IDbContextTypeMatcher
    {
        void Populate(Type[] dbContextTypes);

        Type GetConcreteType(Type sourceDbContextType);
    }
}