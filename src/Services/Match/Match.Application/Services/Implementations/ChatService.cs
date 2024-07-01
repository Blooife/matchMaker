using Match.Application.Exceptions;
using Match.Application.Services.Interfaces;
using Match.Domain.Models;
using Match.Domain.Repositories;

namespace Match.Application.Services.Implementations;

public class ChatService : IChatService
{
    private readonly IUnitOfWork _unitOfWork;

    public ChatService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Message> SendMessageAsync(string chatId, string senderId, string message, CancellationToken cancellationToken)
    {
        var chat = await _unitOfWork.Chats.GetByIdAsync(chatId, cancellationToken);
        
        if (chat is null)
        {
            throw new NotFoundException("Chat", chatId);
        }

        var newMessage = new Message
        {
            SenderId = senderId,
            Content = message,
            Timestamp = DateTime.UtcNow,
            ChatId = chat.Id
        };

        await _unitOfWork.Messages.CreateAsync(newMessage, cancellationToken);

        chat.LastMessageTimestamp = newMessage.Timestamp;
        await _unitOfWork.Chats.UpdateAsync(chat, cancellationToken);
        
        return newMessage;
    }
}