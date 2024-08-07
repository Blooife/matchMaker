using Microsoft.AspNetCore.Http;
using Minio;
using Minio.DataModel.Args;
using Profile.Application.Services.Interfaces;

namespace Profile.Application.Services.Implementations;

public class MinioService : IMinioService
{
    private readonly IMinioClient _minioClient;
    
    public MinioService(string endpoint, string accessKey, string secretKey, string bucketname)
    {
        _minioClient = new MinioClient()
                            .WithEndpoint(endpoint)
                            .WithCredentials(accessKey, secretKey)
                            .WithSSL(false)
                            .Build();
        _bucketName = bucketname;
        Endpoint = endpoint;
    }

    public string _bucketName { get; set; }
    public string Endpoint { get; set; }
    public async Task UploadFileAsync(string objectName, IFormFile file)
    {
        var bucketExistsArgs = new BucketExistsArgs()
            .WithBucket(_bucketName);
            
        bool found = await _minioClient.BucketExistsAsync(bucketExistsArgs).ConfigureAwait(false);
            
        if (!found)
        {
            await _minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(_bucketName));
        }
            
        using(var fileStream = new MemoryStream())
        {
            await file.CopyToAsync(fileStream);
                
            var fileBytes = fileStream.ToArray();
            var putObjectArgs = new PutObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(objectName)
                .WithStreamData(new MemoryStream(fileBytes))
                .WithObjectSize(fileStream.Length)
                .WithContentType(Path.GetExtension(objectName));
            await _minioClient.PutObjectAsync(putObjectArgs).ConfigureAwait(false);
        }
    }

    public async Task<Stream> GetFileAsync(string objectName)
    {
        var memoryStream = new MemoryStream();
        await _minioClient.GetObjectAsync(new GetObjectArgs()
            .WithBucket(_bucketName)
            .WithObject(objectName)
            .WithCallbackStream(stream =>
            {
                stream.CopyTo(memoryStream);
            }));

        memoryStream.Position = 0;
        
        return memoryStream;
    }
    
    public async Task<Dictionary<string, Stream>> GetFilesAsync(List<string> objectNames)
    {
        var fileStreams = new Dictionary<string, Stream>();

        foreach (var objectName in objectNames)
        {
            var memoryStream = new MemoryStream();
            await _minioClient.GetObjectAsync(new GetObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(objectName)
                .WithCallbackStream(stream =>
                {
                    stream.CopyTo(memoryStream);
                }));

            memoryStream.Position = 0;
            fileStreams.Add(objectName, memoryStream);
        }

        return fileStreams;
    }


    public async Task DeleteFileAsync(string objectName)
    {
        await _minioClient.RemoveObjectAsync(new RemoveObjectArgs()
            .WithBucket(_bucketName)
            .WithObject(objectName));
    }
}