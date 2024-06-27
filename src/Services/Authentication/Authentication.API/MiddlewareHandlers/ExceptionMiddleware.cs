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
            var result = JsonConvert.SerializeObject(new ErrorDetails 
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
                case AssignRoleException assignRoleException:
                    statusCode = HttpStatusCode.BadRequest;
                    break;
                case RemoveRoleException removeRoleException:
                    statusCode = HttpStatusCode.BadRequest;
                    break;
                case LoginException loginException:
                    statusCode = HttpStatusCode.Unauthorized;
                    break;
                case RegisterException registerException:
                    statusCode = HttpStatusCode.BadRequest;
                    break;
                case DbException dbException:
                    statusCode = HttpStatusCode.BadRequest;
                    break;
                case CreateRoleException createRoleException:
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
