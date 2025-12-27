using datntdev.Microservices.Common.Application;
using datntdev.Microservices.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Reflection;
using System.Text.RegularExpressions;

namespace datntdev.Microservices.ServiceDefaults.Providers
{
    internal partial class AppServiceControllerFeatureProvider : ControllerFeatureProvider, IApplicationModelConvention
    {
        protected override bool IsController(TypeInfo typeInfo)
        {
            if (!typeInfo.IsClass || typeInfo.IsAbstract) return false;

            return typeInfo.IsAssignableTo(typeof(IAppService));
        }

        public void Apply(ApplicationModel application)
        {
            var appServiceControllers = application.Controllers
                .Where(c => c.ControllerType.IsAssignableTo(typeof(IAppService)));
            foreach (var controller in appServiceControllers)
            {
                controller.ControllerName = GetConventionalControllerName(controller);
                foreach (var action in controller.Actions)
                {
                    action.Selectors.Clear();
                    action.Selectors.Add(NormalizeDefaultSelector(action));
                    action.ApiExplorer.IsVisible = true;

                    action.Filters.Add(NormalizeSuccessResponseType(action));
                    action.Filters.Add(new ProducesResponseTypeAttribute(typeof(ErrorResponse), 400));
                    action.Filters.Add(new ProducesResponseTypeAttribute(typeof(ErrorResponse), 404));
                    action.Filters.Add(new ProducesResponseTypeAttribute(typeof(ErrorResponse), 409));
                    action.Filters.Add(new ProducesResponseTypeAttribute(typeof(ErrorResponse), 500));
                }
            }
        }

        private static SelectorModel NormalizeDefaultSelector(ActionModel action)
        {
            var httpMethod = GetConventionalVerbForMethodName(action.ActionName);
            var httpMethodConstraint = new HttpMethodActionConstraint([httpMethod]);
            var routeAttribute = new RouteAttribute(GetConventionalActionRoute(action));
            var routeAttributeModel = new AttributeRouteModel(routeAttribute)
            {
                Name = action.Controller.ControllerName + "." + action.ActionName
            };

            foreach (var param in action.Parameters.Where(x => x.Name != "id" && x.BindingInfo is null))
            {
                if (httpMethod == "POST" || httpMethod == "PUT")
                {
                    param.BindingInfo = BindingInfo.GetBindingInfo([new FromBodyAttribute()]);
                }
            }

            var selector = new SelectorModel() { AttributeRouteModel = routeAttributeModel };
            selector.ActionConstraints.Add(httpMethodConstraint);
            return selector;
        }

        private static ProducesResponseTypeAttribute NormalizeSuccessResponseType(ActionModel action)
        {
            var returnType = action.ActionMethod.ReturnType;
            if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
            {
                returnType = returnType.GetGenericArguments()[0]; // Unwrap Task<T>
            }

            if (returnType == typeof(void) || returnType == typeof(Task))
            {
                return new ProducesResponseTypeAttribute(200);
            }
            else
            {
                return new ProducesResponseTypeAttribute(returnType, 200);
            }
        } 

        private static string GetConventionalControllerName(ControllerModel controller)
        {
            return controller.ControllerName.Replace("AppService", "").Replace("ApplicationService", "");
        }

        private static string GetConventionalActionRoute(ActionModel action)
        {
            var controllerName = action.Controller.ControllerName;
            var baseRoute = $"api/{KebabCaseRegex().Replace(controllerName, "$1-$2").ToLower()}";

            var routeAttribute = action.Attributes.SingleOrDefault(x => x is RouteAttribute);
            if (routeAttribute != null) return $"{baseRoute}/{((RouteAttribute)routeAttribute).Template}";

            if (action.ActionName == "Get") return $"{baseRoute}/{{id}}";
            if (action.ActionName == "Update") return $"{baseRoute}/{{id}}";
            if (action.ActionName == "Delete") return $"{baseRoute}/{{id}}";
            return baseRoute;
        }

        private static string GetConventionalVerbForMethodName(string actionName)
        {
            if (actionName.StartsWith("Get", StringComparison.OrdinalIgnoreCase))
                return "GET";

            if (actionName.StartsWith("Update", StringComparison.OrdinalIgnoreCase))
                return "PUT";

            if (actionName.StartsWith("Delete", StringComparison.OrdinalIgnoreCase))
                return "DELETE";

            if (actionName.StartsWith("Create", StringComparison.OrdinalIgnoreCase))
                return "POST";

            throw new InvalidOperationException($"No conventional HTTP verb found for action '{actionName}'");
        }

        [GeneratedRegex("([a-z])([A-Z])")]
        private static partial Regex KebabCaseRegex();
    }
}
