using Shared.Messages;

namespace Profile.Application.Kafka.Producers;

public interface IProducerService
{
    Task ProduceAsync<T>(T message) where T : BaseMessage;
}