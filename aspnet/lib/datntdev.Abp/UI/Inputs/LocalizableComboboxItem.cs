using System;
using datntdev.Abp.Localization;

namespace datntdev.Abp.UI.Inputs
{
    [Serializable]
    public class LocalizableComboboxItem : ILocalizableComboboxItem
    {
        public string Value { get; set; }

        public ILocalizableString DisplayText { get; set; }

        public LocalizableComboboxItem()
        {
            
        }

        public LocalizableComboboxItem(string value, ILocalizableString displayText)
        {
            Value = value;
            DisplayText = displayText;
        }
    }
}