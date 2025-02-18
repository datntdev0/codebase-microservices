using Microsoft.AspNetCore.Mvc.Rendering;

namespace datntdev.Abp.Application.Services.Dto;

public static class ComboboxItemDtoExtensions
{
    public static SelectListItem ToSelectListItem(this ComboboxItemDto comboboxItem)
    {
        return new SelectListItem
        {
            Value = comboboxItem.Value,
            Text = comboboxItem.DisplayText,
            Selected = comboboxItem.IsSelected
        };
    }
}