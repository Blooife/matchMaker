using AutoMapper;
using Confluent.Kafka;
using Match.Application.DTOs.Profile.Request;
using Match.Application.UseCases.ProfileUseCases.Commands.Create;
using Match.Application.UseCases.ProfileUseCases.Commands.Delete;
using Match.Application.UseCases.ProfileUseCases.Commands.DeletePermanently;
using Match.Application.UseCases.ProfileUseCases.Commands.Update;
using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shared.Messages.Profile;

namespace Match.Application.Consumers;

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

                if (typedMessage is ProfileCreatedMessage profileCreatedMessage)
                {
                    Console.WriteLine("created");
                    var command = new CreateProfileCommand(_mapper.Map<CreateProfileDto>(profileCreatedMessage));
                    await _mediator.Send(command, cancellationToken);
                }
                else if (typedMessage is ProfileDeletedMessage profileDeletedMessage)
                {
                    var command = new DeleteProfileCommand(profileDeletedMessage.Id);
                    await _mediator.Send(command, cancellationToken);
                }
                else if (typedMessage is ProfileUpdatedMessage profileUpdatedMessage)
                {
                    var command = new UpdateProfileCommand(_mapper.Map<UpdateProfileDto>(profileUpdatedMessage));
                    await _mediator.Send(command, cancellationToken);
                }
                else if (typedMessage is ManyProfilesDeletedMessage manyProfilesDeletedMessage)
                {
                    Console.WriteLine(manyProfilesDeletedMessage.ProfilesIds);
                    var command = new DeleteProfilesPermanentlyCommand(manyProfilesDeletedMessage.ProfilesIds);
                    await _mediator.Send(command, cancellationToken);
                }
            }
        }
    }
}