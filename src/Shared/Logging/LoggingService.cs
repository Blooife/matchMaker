using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Shared.Logging;

public class LoggingService : ILoggingService
{
    
        private readonly ILogger<LoggingService> _logger;

        public LoggingService(ILogger<LoggingService> logger)
        {
            _logger = logger;
        }

        public async Task LogResponseAsync(string controllerName, string actionName, object response)
        {
            var logMessage = $"Controller: {controllerName}, Action: {actionName}, Response: {JsonConvert.SerializeObject(response)}";
            _logger.LogInformation(logMessage);
            await Task.CompletedTask; 
        }
        
        public async Task LogErrorAsync(string controllerName, string actionName, Exception exception)
        {
            var logMessage = $"Controller: {controllerName}, Action: {actionName}, Exception: {exception.Message}";
            _logger.LogError(logMessage);
            await Task.CompletedTask; 
        }
        
        public async Task LogRequestAsync(string controllerName, string actionName, HttpRequest request)
        {
            var logMessage = $"Controller: {controllerName}, Action: {actionName}, Request: {request.Method} {request.Path}";
            _logger.LogInformation(logMessage);
            await Task.CompletedTask;
        }

}