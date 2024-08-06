using System.Net;
using Match.Application.Exceptions;
using MongoDB.Driver;
using Newtonsoft.Json;
using Shared.Models;
using ValidationException = Match.Application.Exceptions.ValidationException;

namespace Match.Presentation.MiddlewareHandlers;

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
            case AlreadyExistsException alreadyExistsException:
                statusCode = HttpStatusCode.Conflict;
                break;
            case NotFoundException notFoundException:
                statusCode = HttpStatusCode.NotFound;
                break;
            case MongoException mongoException:
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