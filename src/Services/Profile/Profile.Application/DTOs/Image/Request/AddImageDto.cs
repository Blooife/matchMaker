using Microsoft.AspNetCore.Http;

namespace Profile.Application.DTOs.Image.Request;

public class AddImageDto
{
    public string ProfileId { get; set; }
    public IFormFile file { get; set; }
}