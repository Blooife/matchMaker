using AutoMapper;
using Match.Application.DTOs.Chat.Response;
using Match.Application.Exceptions;
using Match.Domain.Interfaces.Repositories;
using MediatR;

namespace Match.Application.UseCases.ChatUseCases.Queries.GetByProfilesIds;

public class GetChatByProfilesIdsHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<GetChatByProfilesIdsQuery, ChatResponseDto>
{
    public async Task<ChatResponseDto> Handle(GetChatByProfilesIdsQuery request, CancellationToken cancellationToken)
    {
        var profile1 = await _unitOfWork.Profiles.GetByIdAsync(request.FirstProfileId, cancellationToken);

        if (profile1 is null)
        {
            throw new NotFoundException("Profile", request.FirstProfileId);
        }
        
        var profile2 = await _unitOfWork.Profiles.GetByIdAsync(request.SecondProfileId, cancellationToken);

        if (profile2 is null)
        {
            throw new NotFoundException("Profile", request.SecondProfileId);
        }

        var chat = await _unitOfWork.Chats.GetChatByProfilesIdsAsync(request.FirstProfileId, request.SecondProfileId, cancellationToken);

        if (chat is null)
        {
            throw new NotFoundException($"Chat with profile ids: {request.FirstProfileId} and {request.SecondProfileId} wad not found");
        }

        var mappedChat = _mapper.Map<ChatResponseDto>(chat);
        mappedChat.ProfileName = mappedChat.FirstProfileId == profile1.Id ? profile2.Name : profile1.Name;
        mappedChat.ProfileLastName = mappedChat.FirstProfileId == profile1.Id ? profile2.LastName : profile1.LastName;
        mappedChat.MainImageUrl = mappedChat.FirstProfileId == profile1.Id ? profile2.MainImageUrl : profile1.MainImageUrl;
        
        return mappedChat;
    }
}