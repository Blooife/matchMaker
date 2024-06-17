namespace Profile.Application.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(object key) : base($"Object (with key = {key}) was not found")
    {

    }
}