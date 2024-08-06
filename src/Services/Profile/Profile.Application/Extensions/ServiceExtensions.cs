using System.Reflection;
using Confluent.Kafka;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Profile.Application.Behavior;
using Profile.Application.Services.Implementations;
using Profile.Application.Services.Interfaces;
using Profile.Application.Kafka.Consumers;
using Profile.Application.Kafka.Producers;

namespace Profile.Application.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureApplication(this IServiceCollection services, IConfiguration config)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
        services.AddSingleton<ICacheService, CacheService>();
        services.ConfigureMessageBroker(config);
    }

    private static void ConfigureMessageBroker(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<MessageHandler>();
        services.AddHostedService<ConsumerService>();
        services.Configure<ConsumerConfig>(config.GetRequiredSection("Kafka:Consumer"));
        services.AddSingleton<ProducerService>();
        services.Configure<ProducerConfig>(config.GetRequiredSection("Kafka:Producer"));
    }
}