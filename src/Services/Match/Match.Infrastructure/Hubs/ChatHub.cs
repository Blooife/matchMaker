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

    public async Task SendMessage(int chatId, string senderId, string message)
    {
        var newMessage = await _chatService.SendMessageAsync(chatId, senderId, message);
        await Clients.Group(chatId.ToString()).SendAsync("ReceiveMessage", newMessage);
    }

    public async Task JoinChat(int chatId)
    {
        await Groups.Add(Context.ConnectionId, chatId.ToString());
    }

    public async Task LeaveChat(int chatId)
    {
        await Groups.Add(Context.ConnectionId, chatId.ToString());
    }
}