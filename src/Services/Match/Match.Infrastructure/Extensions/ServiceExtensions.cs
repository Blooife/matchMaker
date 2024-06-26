

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
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
    
    private static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration["MatchDatabase:ConnectionString"]!;
        
        services.Configure<MatchDbSettings>(configuration.GetSection("MatchDatabase"));
        services.AddSingleton<IMongoClient>(_ => new MongoClient(connectionString));
        services.AddScoped<IMongoDbContext, MatchDbContext>();
    }
}