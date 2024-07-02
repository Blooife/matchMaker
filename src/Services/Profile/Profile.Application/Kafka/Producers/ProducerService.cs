using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Shared.Messages;

namespace Profile.Application.Kafka.Producers;

public class ProducerService
{
    private readonly string _topic;
    private readonly IProducer<string, string> _producer;

    public ProducerService(IConfiguration configuration, IOptions<ProducerConfig> producerConfig)
    {
        _topic = configuration["Kafka:Producer:Topic"]!;
        _producer = new ProducerBuilder<string, string>(producerConfig.Value).Build();
    }

    public async Task ProduceAsync<T>(T message) where T : BaseMessage
    {
        var kafkaMessage = new Message<string, string>
        {
            Key = message.Id,
            Value = JsonConvert.SerializeObject(new
            {
                Type = typeof(T).AssemblyQualifiedName,
                Payload = message
            })
        };

        await _producer.ProduceAsync(_topic, kafkaMessage);
    }
}