﻿using System;
using Microsoft.AspNetCore.Http;

namespace datntdev.Abp.Web.Core.Mvc.Extensions;

public static class AjaxRequestExtensions
{
    public static bool IsAjaxRequest(this HttpRequest request)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        return request.Headers != null &&
               request.Headers["X-Requested-With"] == "XMLHttpRequest";
    }
}
