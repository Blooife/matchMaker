using AutoMapper;
using Match.Application.Services.Interfaces;
using Match.Domain.Repositories;
using Match.Infrastructure.Context;
using Match.Infrastructure.Mapper;
using Match.Infrastructure.Repositories;
using Match.Infrastructure.Services;
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
        services.ConfigureGrpcClient();
       // services.AddScoped<IProfileGrpcClient, ProfileGrpcClient>();
    }
    
    private static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration["MatchDatabase:ConnectionString"]!;
        
        services.Configure<MatchDbSettings>(configuration.GetSection("MatchDatabase"));
        services.AddSingleton<IMongoClient>(_ => new MongoClient(connectionString));
        services.AddScoped<IMongoDbContext, MatchDbContext>();
    }

    private static void ConfigureGrpcClient(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ProfileMapping));

        // Регистрация ProfileGrpcClient с DI
        services.AddScoped<IProfileGrpcClient>((sp) =>
        {
            var configuration = sp.GetRequiredService<IConfiguration>();
            var grpcServiceUrl = configuration["GrpcServiceUrl"]; // Настройте URL gRPC сервиса

            var mapper = sp.GetRequiredService<IMapper>();
            return new ProfileGrpcClient(grpcServiceUrl, mapper);
        });
    }
}