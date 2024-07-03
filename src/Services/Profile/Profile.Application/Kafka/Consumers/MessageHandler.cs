using AutoMapper;
using Confluent.Kafka;
using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Profile.Application.DTOs.User.Request;
using Profile.Application.UseCases.UserUseCases.Commands.Create;
using Profile.Application.UseCases.UserUseCases.Commands.Delete;
using Shared.Messages.Authentication;

namespace Profile.Application.Kafka.Consumers;

public class MessageHandler(IMapper _mapper, IMediator _mediator)
{
    public async Task HandleMessageAsync(string message, CancellationToken cancellationToken)
    {
        var deserializedMessage = JsonConvert.DeserializeObject<JObject>(message);

        if (deserializedMessage is null)
        {
            throw new MessageNullException();
        }

        var messageType = deserializedMessage["Type"]?.ToString();
        var payload = deserializedMessage["Payload"]?.ToString();
        
        if (!string.IsNullOrEmpty(messageType) && !string.IsNullOrEmpty(payload))
        {
            var type = Type.GetType(messageType);
            if (type != null)
            {
                var typedMessage = JsonConvert.DeserializeObject(payload, type);

                if (typedMessage is UserCreatedMessage userCreatedMessage)
                {
                    var command = new CreateUserCommand(_mapper.Map<CreateUserDto>(userCreatedMessage));
                    var result = await _mediator.Send(command, cancellationToken);
                }
                else if (typedMessage is UserDeletedMessage userDeletedMessage)
                {
                    var command = new DeleteUserCommand(userDeletedMessage.Id);
                    var result = await _mediator.Send(command, cancellationToken);
                }
            }
        }
    }
}
