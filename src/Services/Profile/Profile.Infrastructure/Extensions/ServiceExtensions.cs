using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Profile.Domain.Interfaces;
using Profile.Infrastructure.Contexts;
using Profile.Infrastructure.Implementations;

namespace Profile.Infrastructure.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.ConfigureDbContext(config);
        services.ConfigureRepositories();
        services.AddGrpc();
        services.AddScoped<IDbCleanupService, DbCleanUpService>();
    }
    
    private static void ConfigureDbContext(this IServiceCollection services, IConfiguration config)
    {
        var connectionString = config.GetConnectionString("DefaultConnection");
        services.AddDbContext<ProfileDbContext>(options => options.UseNpgsql(connectionString));
    }
    
    private static void ConfigureRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserProfileRepository, UserProfileRepository>();
        services.AddScoped<IGoalRepository, GoalRepository>();
        services.AddScoped<IInterestRepository, InterestRepository>();
        services.AddScoped<ILanguageRepository, LanguageRepository>();
        services.AddScoped<IEducationRepository, EducationRepository>();
        services.AddScoped<ICountryRepository, CountryRepository>();
        services.AddScoped<ICityRepository, CityRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}