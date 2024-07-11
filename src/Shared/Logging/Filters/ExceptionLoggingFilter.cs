namespace Shared.Logging.Filters;

using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

public class ExceptionLoggingFilter : IAsyncExceptionFilter
{
    private readonly ILoggingService _loggingService;

    public ExceptionLoggingFilter(ILoggingService loggingService)
    {
        _loggingService = loggingService;
    }

    public async Task OnExceptionAsync(ExceptionContext context)
    {
        var controllerName = context.ActionDescriptor.RouteValues["controller"];
        var actionName = context.ActionDescriptor.RouteValues["action"];
        
        await _loggingService.LogErrorAsync(
            controllerName,
            actionName,
            context.Exception);
    }
}