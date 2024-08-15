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
        string result;

        switch (exception)
        {
            case ValidationException validationException:
                statusCode = HttpStatusCode.BadRequest;
                result = CreatValidationErrorResponse(validationException.Errors, "ValidationError");
                break;
            case AlreadyExistsException alreadyExistsException:
                statusCode = HttpStatusCode.Conflict;
                result = CreateErrorResponse(alreadyExistsException.Message, "AlreadyExistsError");
                break;
            case NotFoundException notFoundException:
                statusCode = HttpStatusCode.NotFound;
                result = CreateErrorResponse(notFoundException.Message, "NotFoundError");
                break;
            case MongoException mongoException:
                statusCode = HttpStatusCode.BadRequest;
                result = CreateErrorResponse(mongoException.Message, "DatabaseError");
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
    
    private string CreatValidationErrorResponse(List<ValidationError> errors, string errorType)
    {
        var errorMessage = $"Validation Errors: ";

        foreach (var error in errors)
        {
            errorMessage += $" | {error.Error}";
        }
            
        return CreateErrorResponse(errorMessage, "DatabaseError");
    }
}