namespace Match.Application.DTOs.Match.Request;

public class PagedMatchesDto
{
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    public int ProfileId { get; set; }
}