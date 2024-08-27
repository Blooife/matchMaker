using Match.Domain.Models;

namespace Match.Domain.Interfaces.Services;

public interface IChatService
{
    Task<Message> SendMessageAsync(string chatId, string senderId, string message, CancellationToken cancellationToken = default);
}