using System.Data.Common;
using System.Net;
using Authentication.BusinessLogic.Exceptions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Shared.Models;

namespace Authentication.API.MiddlewareHandlers
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }
        
        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            HttpStatusCode statusCode;
            string result;

            switch (exception)
            {
                case ValidationException validationException:
                    statusCode = HttpStatusCode.BadRequest;
                    result = CreateErrorResponse(validationException.Message, "ValidationError");
                    _logger.LogError(validationException.Message);
                    break;
                case NotFoundException notFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    result = CreateErrorResponse(notFoundException.Message, "NotFound");
                    break;
                case AssignRoleException assignRoleException:
                    statusCode = HttpStatusCode.BadRequest;
                    result = CreateErrorResponse(assignRoleException.Message, "AssignRoleError");
                    break;
                case RemoveRoleException removeRoleException:
                    statusCode = HttpStatusCode.BadRequest;
                    result = CreateErrorResponse(removeRoleException.Message, "RemoveRoleError");
                    break;
                case LoginException loginException:
                    statusCode = HttpStatusCode.Unauthorized;
                    result = CreateErrorResponse(loginException.Message, "LoginError");
                    break;
                case RegisterException registerException:
                    statusCode = HttpStatusCode.BadRequest;
                    result = CreateErrorResponse(registerException.Message, "RegisterError");
                    break;
                case DbException dbException:
                    statusCode = HttpStatusCode.BadRequest;
                    result = CreateDbErrorResponse(dbException);
                    break;
                case DbUpdateException dbUpdateException:
                    statusCode = HttpStatusCode.BadRequest;
                    result = CreateDbUpdateErrorResponse(dbUpdateException);
                    break;
                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    result = CreateErrorResponse(exception.Message, "Failure");
                    break;
            }

            context.Response.StatusCode = (int)statusCode;
            return context.Response.WriteAsync(result);
        }

        private string CreateErrorResponse(string message, string errorType)
        {
            return JsonConvert.SerializeObject(new ErrorDetails
            {
                ErrorMessage = message,
                ErrorType = errorType
            });
        }

        private string CreateDbErrorResponse(DbException dbException)
        {
            var errorMessage = $"Database Error: {dbException.Message}";
            if (dbException.InnerException != null)
            {
                errorMessage += $" | Inner Exception: {dbException.InnerException.Message}";
            }
            _logger.LogError(errorMessage);
            
            return CreateErrorResponse(errorMessage, "DatabaseError");
        }

        private string CreateDbUpdateErrorResponse(DbUpdateException dbUpdateException)
        {
            var errorMessage = $"Database Update Error: {dbUpdateException.Message}";
            if (dbUpdateException.InnerException != null)
            {
                errorMessage += $" | Inner Exception: {dbUpdateException.InnerException.Message}";
            }

            foreach (var entry in dbUpdateException.Entries)
            {
                errorMessage += $" | Entity: {entry.Entity.GetType().Name}, State: {entry.State}";
            }
            
            _logger.LogError(errorMessage);
            
            return CreateErrorResponse(errorMessage, "DatabaseUpdateError");
        }
    }
}