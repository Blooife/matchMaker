using AutoMapper;
using Match.Application.DTOs.Message.Response;
using Match.Application.Exceptions;
using Match.Domain.Models;
using Match.Domain.Repositories;
using MediatR;
using Shared.Models;

namespace Match.Application.UseCases.MessageUseCases.Queries.GetPagedByChatId;

public class GetPagedMessagesHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<GetPagedMessagesQuery, PagedList<Message>>
{
    public async Task<PagedList<Message>> Handle(GetPagedMessagesQuery request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;
        var chat = await _unitOfWork.Chats.GetByIdAsync(dto.ChatId, cancellationToken);

        if (chat is null)
        {
            throw new NotFoundException();
        }

        var messages =
            await _unitOfWork.Messages.GetPagedAsync(dto.ChatId, dto.PageNumber, dto.PageSize, cancellationToken);

        return messages;
    }
}