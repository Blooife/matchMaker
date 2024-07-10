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
        await _loggingService.LogErrorAsync(
            context.RouteData.Values["controller"].ToString(),
            context.ActionDescriptor.DisplayName,
            context.Exception);
    }
}
