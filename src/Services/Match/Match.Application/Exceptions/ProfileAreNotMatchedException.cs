namespace Match.Application.Exceptions;

public class ProfileAreNotMatchedException : Exception
{
    public ProfileAreNotMatchedException(string message) : base(message)
    {

    }
}