namespace Match.Application.Exceptions;

public class ProfilesAreNotMatchedException : Exception
{
    public ProfilesAreNotMatchedException(string message) : base(message)
    {

    }
}