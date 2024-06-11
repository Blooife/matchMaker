namespace Authentication.BusinessLogic.Exceptions;

public class RemoveRoleException : Exception
{
    public RemoveRoleException() { }
    public RemoveRoleException(string message) : base(message) { }
}