using Microsoft.AspNetCore.Http;

namespace Profile.Application.Services.Interfaces;

public interface IMinioService
{
    Task UploadFileAsync(string objectName, IFormFile file);
    Task<Stream> GetFileAsync(string objectName);
    Task DeleteFileAsync(string objectName);
}