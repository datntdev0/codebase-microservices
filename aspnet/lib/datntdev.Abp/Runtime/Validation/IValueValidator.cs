using System.Collections.Generic;

namespace datntdev.Abp.Runtime.Validation
{
    public interface IValueValidator
    {
        string Name { get; }

        object this[string key] { get; set; }

        IDictionary<string, object> Attributes { get; }

        bool IsValid(object value);
    }
}