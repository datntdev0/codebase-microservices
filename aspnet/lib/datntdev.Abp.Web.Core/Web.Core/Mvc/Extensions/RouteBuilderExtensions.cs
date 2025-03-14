﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Routing;

namespace datntdev.Abp.Web.Core.Mvc.Extensions;

public static class RouteBuilderExtensions
{
    public static void ConfigureAll(this List<Action<IRouteBuilder>> routeBuilderActions, IRouteBuilder routes)
    {
        if (routeBuilderActions == null)
        {
            throw new ArgumentNullException(nameof(routeBuilderActions));
        }

        routeBuilderActions.ForEach(action =>
        {
            action(routes);
        });
    }

    public static void ConfigureAllEndpoints(this List<Action<IEndpointRouteBuilder>> routeBuilderActions, IEndpointRouteBuilder routes)
    {
        if (routeBuilderActions == null)
        {
            throw new ArgumentNullException(nameof(routeBuilderActions));
        }

        routeBuilderActions.ForEach(action =>
        {
            action(routes);
        });
    }
}
