using Microsoft.AspNetCore.Http;

namespace Profile.Application.Services.Interfaces;

public interface IMinioService
{
    public string _bucketName { get; set; }
    public string Endpoint { get; set; }
    Task UploadFileAsync(string objectName, IFormFile file);
    Task<Stream> GetFileAsync(string objectName);
    Task DeleteFileAsync(string objectName);
    Task<Dictionary<string, Stream>> GetFilesAsync(List<string> objectNames);
}