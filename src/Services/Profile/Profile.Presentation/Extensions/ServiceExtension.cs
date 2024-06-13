namespace Profile.Presentation.Extensions;

public static class ServiceExtension
{
    public static void ConfigurePresentation(this IServiceCollection services)
    {
        services.AddAuthorization();
        services.AddControllers();
    }
}