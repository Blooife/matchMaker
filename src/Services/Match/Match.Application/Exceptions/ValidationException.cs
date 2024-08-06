namespace Match.Application.Exceptions;

public class ValidationException : Exception
{
    public List<ValidationError> Errors;
    public ValidationException(List<ValidationError> errors) : base()
    {
        Errors = errors;
    }
}

public sealed record ValidationError(string PropertyName, string Error)
{
    
}