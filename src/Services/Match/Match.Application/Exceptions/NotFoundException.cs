namespace Match.Application.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string objName, object key) : base($"{objName} (with key = {key}) was not found")
    {

    }
    
    public NotFoundException(string message) : base(message)
    {

    }
}