using Match.Application.DTOs.Chat.Request;
using Match.Application.DTOs.Chat.Response;
using Match.Domain.Models;

namespace Match.Application.Mappers;

public class ChatMapping : AutoMapper.Profile
{
    public ChatMapping()
    {
        CreateMap<CreateChatDto, Chat>()
            .ForMember(dest=>dest.FirstProfileId, opt=>opt.MapFrom(src=>src.FirstProfileId))
            .ForMember(dest=>dest.SecondProfileId, opt=>opt.MapFrom(src=>src.SecondProfileId));
        
        CreateMap<Chat, ChatResponseDto>()
            .ForMember(dest=>dest.Id, opt=>opt.MapFrom(src=>src.Id))
            .ForMember(dest=>dest.FirstProfileId, opt=>opt.MapFrom(src=>src.FirstProfileId))
            .ForMember(dest=>dest.SecondProfileId, opt=>opt.MapFrom(src=>src.SecondProfileId));
    }
}