namespace Match.Application.DTOs.Message.Response;

public class MessageResponseDto
{
    public string Id { get; set; }
    public string SenderId { get; set; }
    public string Content { get; set; }
    public DateTime Timestamp { get; set; }
    public string ChatId { get; set; }
}