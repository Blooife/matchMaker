using AutoMapper;
using Match.Application.DTOs.Chat.Response;
using Match.Application.Exceptions;
using Match.Domain.Models;
using Match.Domain.Repositories;
using MediatR;

namespace Match.Application.UseCases.ChatUseCases.Commands.Create;

public class CreateChatHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<CreateChatCommand, ChatResponseDto>
{
    public async Task<ChatResponseDto> Handle(CreateChatCommand request, CancellationToken cancellationToken)
    {
        var areProfilesMatched =
            await _unitOfWork.Matches.AreProfilesMatched(request.Dto.ProfileId1, request.Dto.ProfileId2, cancellationToken);
        
        if (!areProfilesMatched)
        {
            throw new ProfilesAreNotMatchedException("You cant create chat for unmatched users");
        }
        
        var chat = _mapper.Map<Chat>(request.Dto);
        await _unitOfWork.Chats.CreateAsync(chat, cancellationToken);
        
        return _mapper.Map<ChatResponseDto>(chat);
    }
}