using Match.Application.DTOs.Message.Response;
using Microsoft.AspNet.SignalR.Messaging;

namespace Match.Application.Mappers;

public class MessageMapping : AutoMapper.Profile
{
    public MessageMapping()
    {
        CreateMap<Message, MessageResponseDto>();
    }
}