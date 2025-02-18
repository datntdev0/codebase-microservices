using System.Collections.Generic;
using System.Reflection;

namespace datntdev.Abp.EntityFrameworkCore.Utils;

public class EntityDateTimePropertiesInfo
{
    public List<PropertyInfo> DateTimePropertyInfos { get; set; }

    public List<string> ComplexTypePropertyPaths { get; set; }

    public EntityDateTimePropertiesInfo()
    {
        DateTimePropertyInfos = new List<PropertyInfo>();
        ComplexTypePropertyPaths = new List<string>();
    }
}