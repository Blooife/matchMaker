using AutoMapper;
using Match.Application.Exceptions;
using Match.Domain.Repositories;
using MediatR;
using Shared.Models;

namespace Match.Application.UseCases.ChatUseCases.Commands.Delete;

public class DeleteChatHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<DeleteChatCommand, GeneralResponseDto>
{
    public async Task<GeneralResponseDto> Handle(DeleteChatCommand request, CancellationToken cancellationToken)
    {
        var chat = await _unitOfWork.Chats.GetByIdAsync(request.ChatId, cancellationToken);

        if (chat is null)
        {
            throw new NotFoundException("Chat", request.ChatId);
        }

        await _unitOfWork.Chats.DeleteAsync(chat, cancellationToken);
        await _unitOfWork.Messages.DeleteMessagesByChatIdAsync(chat.Id, cancellationToken);
        
        return new GeneralResponseDto();
    }
}