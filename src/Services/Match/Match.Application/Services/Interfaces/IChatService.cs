using Match.Domain.Models;

namespace Match.Application.Services.Interfaces;

public interface IChatService
{
    Task<Message> SendMessageAsync(int chatId, string senderId, string message);
}