using Microsoft.AspNetCore.Http;

namespace Shared.Logging;

public interface ILoggingService
{
    Task LogResponseAsync(string controllerName, string actionName, object response);
    Task LogErrorAsync(string controllerName, string actionName, Exception exception);
    Task LogRequestAsync(string controllerName, string actionName, HttpRequest request);
}