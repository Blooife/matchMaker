using Microsoft.AspNetCore.Http;

namespace Profile.Domain.Interfaces.Services;

public interface IMinioService
{
    public string _bucketName { get; set; }
    public string Endpoint { get; set; }
    Task UploadFileAsync(string objectName, IFormFile file);
    Task<Stream> GetFileAsync(string objectName);
    Task DeleteFileAsync(string objectName);
}