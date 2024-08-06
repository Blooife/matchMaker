using Match.Domain.Models;

namespace Match.Application.Services.Interfaces;

public interface IChatService
{
    Task<Message> SendMessageAsync(string chatId, string senderId, string message, CancellationToken cancellationToken = default);
}