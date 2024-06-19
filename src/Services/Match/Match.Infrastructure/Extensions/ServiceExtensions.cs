using Match.Domain.Repositories;
using Match.Infrastructure.Context;
using Match.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Match.Infrastructure.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureDbContext(configuration);
        services.ConfigureRepositories();
    }
    private static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        //services.Configure<MatchDbSettings>(configuration.GetSection("MatchDatabase"));
    }

    private static void ConfigureRepositories(this IServiceCollection services)
    {
        /*services.AddScoped<IMatchRepository, MatchRepository>();
        services.AddScoped<ILikeRepository, LikeRepository>();
        services.AddScoped<IProfileRepository, ProfileRepository>();
        services.AddScoped<IChatRepository, ChatRepository>();*/
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}