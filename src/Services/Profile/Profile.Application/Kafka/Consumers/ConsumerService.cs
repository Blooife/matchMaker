using AutoMapper;
using Confluent.Kafka;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Profile.Application.DTOs.User.Request;
using Profile.Application.UseCases.UserUseCases.Commands.Create;
using Profile.Application.UseCases.UserUseCases.Commands.Delete;
using Shared.Messages.Authentication;

namespace Profile.Application.Kafka.Consumers;

public class ConsumerService : BackgroundService
{
    private readonly string _topic;
    private readonly IConsumer<string, string> _consumer;
    private readonly MessageHandler _messageHandler;
    private readonly IServiceProvider _serviceProvider;

    public ConsumerService(IConfiguration configuration, IOptions<ConsumerConfig> consumerConfig, IServiceProvider serviceProvider)
    {
        _topic = configuration["Kafka:Consumer:Topic"]!;
        _consumer = new ConsumerBuilder<string, string>(consumerConfig.Value).Build();
        //_messageHandler = messageHandler;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await Task.Yield();
        _consumer.Subscribe(_topic);
        Console.WriteLine(_topic);
        while (!cancellationToken.IsCancellationRequested)
        {
            var consumeResult = _consumer.Consume(cancellationToken);
            var message = consumeResult.Message.Value;

            using var scope = _serviceProvider.CreateScope();
            var messageHandler = scope.ServiceProvider.GetRequiredService<MessageHandler>();
            await messageHandler.HandleMessageAsync(message, cancellationToken);
        }

        _consumer.Close();
    }
}