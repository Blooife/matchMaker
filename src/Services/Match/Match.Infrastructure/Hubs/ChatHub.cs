using Match.Application.Services.Interfaces;
using Microsoft.AspNet.SignalR;

namespace Match.Infrastructure.Hubs;

public class ChatHub : Hub
{
    private readonly IChatService _chatService;

    public ChatHub(IChatService chatService)
    {
        _chatService = chatService;
    }

    public async Task SendMessage(string chatId, string senderId, string message)
    {
        var newMessage = await _chatService.SendMessageAsync(chatId, senderId, message);
        await Clients.Group(chatId.ToString()).SendAsync("ReceiveMessage", newMessage);
    }

    public async Task JoinChat(string chatId)
    {
        await Groups.Add(Context.ConnectionId, chatId);
    }

    public async Task LeaveChat(string chatId)
    {
        await Groups.Add(Context.ConnectionId, chatId);
    }
}