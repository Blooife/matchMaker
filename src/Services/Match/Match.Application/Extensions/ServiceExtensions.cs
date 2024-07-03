using System.Reflection;
using Confluent.Kafka;
using FluentValidation;
using Match.Application.Behavior;
using Match.Application.Consumers;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Match.Application.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureApplication(this IServiceCollection services, IConfiguration config)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
        services.ConfigureMessageBroker(config);
    }
    
    private static void ConfigureMessageBroker(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<MessageHandler>();
        services.AddHostedService<ConsumerService>();
        services.Configure<ConsumerConfig>(config.GetRequiredSection("Kafka:Consumer"));
    }
}