using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Match.Application.Consumers;

public class ConsumerService : BackgroundService
{
    private readonly string _topic;
    private readonly IConsumer<string, string> _consumer;
    private readonly IServiceProvider _serviceProvider;

    public ConsumerService(IConfiguration configuration, IOptions<ConsumerConfig> consumerConfig, IServiceProvider serviceProvider)
    {
        _topic = configuration["Kafka:Consumer:Topic"]!;
        _consumer = new ConsumerBuilder<string, string>(consumerConfig.Value).Build();
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        try
        {
            await Task.Yield();
            _consumer.Subscribe(_topic);

            while (!cancellationToken.IsCancellationRequested)
            {
                var consumeResult = _consumer.Consume(cancellationToken);
                var message = consumeResult.Message.Value;

                using var scope = _serviceProvider.CreateScope();
                var messageHandler = scope.ServiceProvider.GetRequiredService<MessageHandler>();
                await messageHandler.HandleMessageAsync(message, cancellationToken);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            _consumer.Close();
        }
    }
}