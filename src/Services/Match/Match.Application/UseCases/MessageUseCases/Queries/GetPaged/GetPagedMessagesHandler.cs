using AutoMapper;
using Match.Application.Exceptions;
using Match.Domain.Models;
using Match.Domain.Interfaces;
using MediatR;
using Shared.Models;

namespace Match.Application.UseCases.MessageUseCases.Queries.GetPaged;

public class GetPagedMessagesHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<GetPagedMessagesQuery, PagedList<Message>>
{
    public async Task<PagedList<Message>> Handle(GetPagedMessagesQuery request, CancellationToken cancellationToken)
    {
        var chat = await _unitOfWork.Chats.GetByIdAsync(request.ChatId, cancellationToken);

        if (chat is null)
        {
            throw new NotFoundException("Chat", request.ChatId);
        }

        var messages =
            await _unitOfWork.Messages.GetPagedAsync(request.ChatId, request.PageNumber, request.PageSize, cancellationToken);

        return messages;
    }
}