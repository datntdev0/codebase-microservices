using System.Collections.Generic;

namespace datntdev.Abp.EntityHistory
{
    public class EntityHistorySelectorList : List<NamedTypeSelector>, IEntityHistorySelectorList
    {
        public bool RemoveByName(string name)
        {
            return RemoveAll(s => s.Name == name) > 0;
        }
    }
}
