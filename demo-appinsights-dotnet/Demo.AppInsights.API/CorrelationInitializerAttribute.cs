using System;
using Demo.AppInsights.Core.Telemetry;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Demo.AppInsights.API
{
    /// <summary>
    /// Provides the CorrelationManager with a static OperationId value to be used for telemetry correlation.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class CorrelationInitializerAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var requestTelemetry = filterContext.HttpContext.Features.Get<RequestTelemetry>();

            if (null == requestTelemetry)
                return;

            CorrelationManager.SetOperationId(requestTelemetry.Context.Operation.Id);
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }
    }
}