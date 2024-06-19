namespace Match.Domain.Models;

public class Message
{
    public int Id { get; set; }
    public string SenderId { get; set; }
    public string Text { get; set; }
    public DateTime SendTime { get; set; }
    
    public int ChatId { get; set; }
}