using Match.Application.DTOs.Chat.Request;
using Match.Application.DTOs.Chat.Response;
using Match.Domain.Models;

namespace Match.Application.Mappers;

public class ChatMapping : AutoMapper.Profile
{
    public ChatMapping()
    {
        CreateMap<CreateChatDto, Chat>();
        CreateMap<Chat, ChatResponseDto>();
    }
}