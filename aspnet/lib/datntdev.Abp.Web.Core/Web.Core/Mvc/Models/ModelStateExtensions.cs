﻿using System.Collections.Generic;
using datntdev.Abp.Localization;
using datntdev.Abp.Web;
using datntdev.Abp.Web.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace datntdev.Abp.Web.Core.Mvc.Models;

public static class ModelStateExtensions
{
    public static AjaxResponse ToMvcAjaxResponse(this ModelStateDictionary modelState, ILocalizationManager localizationManager)
    {
        if (modelState.IsValid)
        {
            return new AjaxResponse();
        }

        var validationErrors = new List<ValidationErrorInfo>();

        foreach (var state in modelState)
        {
            foreach (var error in state.Value.Errors)
            {
                validationErrors.Add(new ValidationErrorInfo(error.ErrorMessage, state.Key));
            }
        }

        var errorInfo = new ErrorInfo(localizationManager.GetString(AbpWebConsts.LocalizationSourceName, "ValidationError"))
        {
            ValidationErrors = validationErrors.ToArray()
        };

        return new AjaxResponse(errorInfo);
    }
}
