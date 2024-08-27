namespace Match.Application.Exceptions;

public class ProfilesAreNotMatchedException : Exception
{
    public ProfilesAreNotMatchedException() : base("Profiles are not matched")
    {

    }
}