namespace Match.Domain.Models;

public class Chat
{
    public int Id { get; set; }
    public string ProfileId1 { get; set; }
    public string ProfileId2 { get; set; }
    
    public List<Message> Messages { get; set; }
}