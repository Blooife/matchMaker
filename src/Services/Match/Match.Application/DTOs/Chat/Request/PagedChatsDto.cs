namespace Match.Application.DTOs.Chat.Request;

public class PagedChatsDto
{
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    public int ProfileId { get; set; }
}