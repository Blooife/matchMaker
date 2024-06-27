namespace Authentication.BusinessLogic.Exceptions;

public class UpdateUserException : Exception
{
    public UpdateUserException(string message) : base(message)
    {
    }
}