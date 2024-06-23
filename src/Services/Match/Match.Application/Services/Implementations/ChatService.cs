using Match.Application.Services.Interfaces;
using Match.Domain.Models;
using Match.Domain.Repositories;
using Microsoft.AspNet.SignalR;

namespace Match.Application.Services.Implementations;

public class ChatService : IChatService
{
    private readonly IUnitOfWork _unitOfWork;

    public ChatService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Message> SendMessageAsync(int chatId, string senderId, string message)
    {
        var chat = await _unitOfWork.Chats.GetByIdAsync(chatId, CancellationToken.None);
        
        if (chat is null)
        {
            throw new Exception();
        }

        var newMessage = new Message
        {
            SenderId = senderId,
            Content = message,
            Timestamp = DateTime.UtcNow,
            ChatId = chat.Id
        };

        await _unitOfWork.Messages.CreateAsync(newMessage, CancellationToken.None);
        
        return newMessage;
    }
}