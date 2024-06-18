using System.Data.Common;
using System.Net;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Profile.Application.Exceptions;
using Shared.Models;
using ValidationException = Profile.Application.Exceptions.ValidationException;

namespace Profile.Presentation.MiddlewareHandlers
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
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
            string result = JsonConvert.SerializeObject(new ErrorDetails 
            { 
                ErrorMessage = exception.Message, 
                ErrorType = "Failure" 
            });

            switch (exception)
            {
                case ValidationException validationException:
                    statusCode = HttpStatusCode.BadRequest;
                    result = JsonConvert.SerializeObject(validationException.Errors);
                    break;
                case NotFoundException notFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    break;
                case AlreadyContainsException alreadyContainsException:
                    statusCode = HttpStatusCode.Conflict;
                    break;
                case NotContainsException notContainsException:
                    statusCode = HttpStatusCode.NotFound;
                    break;
                case DbException dbException:
                    statusCode = HttpStatusCode.BadRequest;
                    break;
                case DbUpdateException dbUpdateException:
                    statusCode = HttpStatusCode.BadRequest;
                    break;
                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    break;
            }

            context.Response.StatusCode = (int)statusCode;
            
            return context.Response.WriteAsync(result);
        }
    }
}
