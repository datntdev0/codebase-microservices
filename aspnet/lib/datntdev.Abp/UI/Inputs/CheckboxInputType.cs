using System;
using datntdev.Abp.Runtime.Validation;

namespace datntdev.Abp.UI.Inputs
{
    [Serializable]
    [InputType("CHECKBOX")]
    public class CheckboxInputType : InputTypeBase
    {
        public CheckboxInputType()
            : this(new BooleanValueValidator())
        {

        }

        public CheckboxInputType(IValueValidator validator)
            : base(validator)
        {
            
        }
    }
}