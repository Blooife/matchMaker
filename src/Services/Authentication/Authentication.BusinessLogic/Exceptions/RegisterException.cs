namespace Authentication.BusinessLogic.Exceptions;

public class RegisterException : Exception
{
    public RegisterException(string message) : base(message) { }
}