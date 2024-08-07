using AutoMapper;
using Match.Application.DTOs.Message.Response;
using Match.Application.Exceptions;
using Match.Domain.Interfaces;
using MediatR;
using Shared.Models;

namespace Match.Application.UseCases.MessageUseCases.Queries.GetPaged;

public class GetPagedMessagesHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<GetPagedMessagesQuery, PagedList<MessageResponseDto>>
{
    public async Task<PagedList<MessageResponseDto>> Handle(GetPagedMessagesQuery request, CancellationToken cancellationToken)
    {
        var chat = await _unitOfWork.Chats.GetByIdAsync(request.ChatId, cancellationToken);

        if (chat is null)
        {
            throw new NotFoundException("Chat", request.ChatId);
        }

        var messages =
            await _unitOfWork.Messages.GetPagedAsync(request.ChatId, request.PageNumber, request.PageSize, cancellationToken);
        var mappedMessages = _mapper.Map<List<MessageResponseDto>>(messages.Item1);

        return new PagedList<MessageResponseDto>(mappedMessages, messages.Item2, request.PageNumber, request.PageSize);
    }
}