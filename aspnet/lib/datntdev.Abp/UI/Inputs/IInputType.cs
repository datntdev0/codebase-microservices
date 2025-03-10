using System.Collections.Generic;
using datntdev.Abp.Runtime.Validation;

namespace datntdev.Abp.UI.Inputs
{
    public interface IInputType
    {
        string Name { get; }

        object this[string key] { get; set; }

        IDictionary<string, object> Attributes { get; }

        IValueValidator Validator { get; set; }
    }
}