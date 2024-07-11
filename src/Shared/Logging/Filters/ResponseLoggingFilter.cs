using Microsoft.AspNetCore.Mvc;

namespace Shared.Logging.Filters;

using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

public class ResponseLoggingFilter : IAsyncActionFilter
{
    private readonly ILoggingService _loggingService;

    public ResponseLoggingFilter(ILoggingService loggingService)
    {
        _loggingService = loggingService;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var resultContext = await next();
        
        if (resultContext.Result is ObjectResult objectResult)
        {
            await _loggingService.LogResponseAsync(
                resultContext.ActionDescriptor.Id,
                resultContext.ActionDescriptor.Id,
                objectResult.Value);
        }
    }
}