using Shared.Messages.Authentication;

namespace Authentication.BusinessLogic.Producers;

public interface IProducerService
{
    Task ProduceAsync<T>(T message) where T : BaseMessage;
}