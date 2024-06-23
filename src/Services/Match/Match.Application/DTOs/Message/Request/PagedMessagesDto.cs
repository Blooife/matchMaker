namespace Match.Application.DTOs.Message.Request;

public class PagedMessagesDto
{
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    public int ChatId { get; set; }
}