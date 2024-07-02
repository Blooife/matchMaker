using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Shared.Messages.Authentication;

namespace Authentication.BusinessLogic.Producers;

public class ProducerService
{
    private readonly IConfiguration _configuration;
    private readonly IProducer<string, string> _producer;

    public ProducerService(IConfiguration configuration)
    {
        _configuration = configuration;

        var producerConfig = new ProducerConfig
        {
            BootstrapServers = _configuration["Kafka:Consumer:BootstrapServers"]
        };

        _producer = new ProducerBuilder<string, string>(producerConfig).Build();
    }

    public async Task ProduceAsync<T>(string topic, T message) where T : BaseMessage
    {
        var kafkaMessage = new Message<string, string>
        {
            Key = message.Id,
            Value = JsonSerializer.Serialize(message)
        };

        await _producer.ProduceAsync(topic, kafkaMessage);
    }
}