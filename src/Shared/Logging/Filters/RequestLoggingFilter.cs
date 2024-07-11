using Microsoft.AspNetCore.Mvc.Filters;

namespace Shared.Logging.Filters;

public class RequestLoggingFilter : IAsyncActionFilter
{
    private readonly ILoggingService _loggingService;

    public RequestLoggingFilter(ILoggingService loggingService)
    {
        _loggingService = loggingService;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var controllerName = context.ActionDescriptor.RouteValues["controller"];
        var actionName = context.ActionDescriptor.RouteValues["action"];
        var request = context.HttpContext.Request;

        await _loggingService.LogRequestAsync(controllerName, actionName, request);

        await next();
    }
}