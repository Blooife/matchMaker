using AutoMapper;
using Match.Application.DTOs.Chat.Response;
using Match.Application.Exceptions;
using Match.Domain.Repositories;
using MediatR;

namespace Match.Application.UseCases.ChatUseCases.Queries.GetByProfileId;

public class GetChatsByProfileIdHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<GetChatsByProfileIdQuery, IEnumerable<ChatResponseDto>>
{
    public async Task<IEnumerable<ChatResponseDto>> Handle(GetChatsByProfileIdQuery request, CancellationToken cancellationToken)
    {
        var profile = await _unitOfWork.Profiles.GetByIdAsync(request.ProfileId, cancellationToken);

        if (profile is null)
        {
            throw new NotFoundException("Profile", request.ProfileId);
        }

        var chats = await _unitOfWork.Chats.GetChatsByProfileIdAsync(request.ProfileId, cancellationToken);

        return _mapper.Map<IEnumerable<ChatResponseDto>>(chats);
    }
}