using AutoMapper;
using Match.Application.DTOs.Chat.Response;
using Match.Application.Exceptions;
using Match.Domain.Models;
using Match.Domain.Interfaces;
using MediatR;

namespace Match.Application.UseCases.ChatUseCases.Commands.Create;

public class CreateChatHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<CreateChatCommand, ChatResponseDto>
{
    public async Task<ChatResponseDto> Handle(CreateChatCommand request, CancellationToken cancellationToken)
    {
        var profile1 = await _unitOfWork.Profiles.GetByIdAsync(request.Dto.FirstProfileId, cancellationToken);

        if (profile1 is null)
        {
            throw new NotFoundException("Profile", request.Dto.FirstProfileId);
        }
        
        var profile2 = await _unitOfWork.Profiles.GetByIdAsync(request.Dto.SecondProfileId, cancellationToken);

        if (profile2 is null)
        {
            throw new NotFoundException("Profile", request.Dto.SecondProfileId);
        } 
        
        var areProfilesMatched =
            await _unitOfWork.Matches.AreProfilesMatchedAsync(request.Dto.FirstProfileId, request.Dto.SecondProfileId, cancellationToken);
        
        if (!areProfilesMatched)
        {
            throw new ProfilesAreNotMatchedException("You cant create chat for unmatched users");
        }
        
        var chat = _mapper.Map<Chat>(request.Dto);
        await _unitOfWork.Chats.CreateAsync(chat, cancellationToken);
        
        var mappedChat = _mapper.Map<ChatResponseDto>(chat);
        mappedChat.ProfileName = mappedChat.FirstProfileId == profile1.Id ? profile2.Name : profile1.Name;
        mappedChat.ProfileLastName = mappedChat.FirstProfileId == profile1.Id ? profile2.LastName : profile1.LastName;
        mappedChat.MainImageUrl = mappedChat.FirstProfileId == profile1.Id ? profile2.MainImageUrl : profile1.MainImageUrl;
        
        return mappedChat;
    }
}