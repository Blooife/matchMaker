using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using FluentAssertions;
using Profile.Application.DTOs.Image.Request;
using Profile.Application.DTOs.Image.Response;
using Profile.Tests.Fakers;
using Profile.Tests.IntegrationTests.Fixtures;
using Shared.Models;

namespace Profile.Tests.IntegrationTests.ControllersTests;

public class ImagesControllerTests(CustomWebApplicationFactory factory) : ClassFixture(factory)
{
    private const string BaseUrl = "/api/images";
    
    [Fact]
    public async Task GetImageById_ImageExists_ReturnsImage()
    {
        var profile = await AddValidProfileToDbAsync();
        var image = ImageFakers.CreateImage().Clone()
            .RuleFor(i => i.ProfileId, profile.Id)
            .Generate();
        await AddEntitiesToDbAsync(image);
        
        var response = await Client.GetAsync($"{BaseUrl}/{image.Id}");
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseDto = await response.Content.ReadFromJsonAsync<ImageResponseDto>();
        responseDto.Should().NotBeNull();
        responseDto!.Id.Should().Be(image.Id);
    }
    
    [Fact]
    public async Task GetImageById_ImageNotExists_ReturnsNotFound()
    {
        int notExistingId = -1;
        
        var response = await Client.GetAsync($"{BaseUrl}/{notExistingId}");
        
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var responseDto = await response.Content.ReadFromJsonAsync<ErrorDetails>();
        responseDto.Should().NotBeNull();
        responseDto!.ErrorType.Should().Be("NotFoundError");
    }
    
    [Fact]
    public async Task AddImageToProfile_ProfileNotExists_ReturnsNotFound()
    {
        int notExistingId = -1;
        
        var response = await Client.GetAsync($"{BaseUrl}/{notExistingId}");
        
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var responseDto = await response.Content.ReadFromJsonAsync<ErrorDetails>();
        responseDto.Should().NotBeNull();
        responseDto!.ErrorType.Should().Be("NotFoundError");
    }
    
    [Fact]
    public async Task AddImageToProfile_ValidData_ReturnsImages_AddedImageIsMain()
    {
        var profile = await AddValidProfileToDbAsync();

        var addImageDto = new MultipartFormDataContent();
        addImageDto.Add(new StringContent(profile.Id), nameof(AddImageDto.ProfileId));

        var filePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "IntegrationTests","TestData", "test.jpg"));
        await using var fileStream = File.OpenRead(filePath);
        var fileContent = new StreamContent(fileStream);
        fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
        addImageDto.Add(fileContent, nameof(AddImageDto.file), "test.jpg");

        var response = await Client.PostAsync($"{BaseUrl}", addImageDto);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseDto = await response.Content.ReadFromJsonAsync<List<ImageResponseDto>>();
        responseDto.Should().NotBeNull();
        responseDto.Should().ContainSingle(); 
    
        var addedImage = responseDto!.First();
        addedImage.ImageUrl.Should().Contain("test.jpg"); 
        addedImage.ProfileId.Should().Be(profile.Id);
        addedImage.IsMainImage.Should().Be(true);
    }
    
    [Fact]
    public async Task AddImageToProfile_ValidData_ReturnsImages_AddedImageNotMain()
    {
        var profile = await AddValidProfileToDbAsync();
        var images = ImageFakers.CreateImage().Clone()
            .RuleFor(i => i.ProfileId, profile.Id)
            .Generate(3);
        images[0].IsMainImage = true;
        profile.Images = images;
        
        await AddEntitiesToDbAsync(images);
        await UpdateDbEntityAsync(profile);
        
        var addImageDto = new MultipartFormDataContent();
        addImageDto.Add(new StringContent(profile.Id), nameof(AddImageDto.ProfileId));

        var filePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "IntegrationTests","TestData", "test.jpg"));
        await using var fileStream = File.OpenRead(filePath);
        var fileContent = new StreamContent(fileStream);
        fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
        addImageDto.Add(fileContent, nameof(AddImageDto.file), "test.jpg");

        var response = await Client.PostAsync($"{BaseUrl}", addImageDto);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseDto = await response.Content.ReadFromJsonAsync<List<ImageResponseDto>>();
        responseDto.Should().NotBeNull();
        responseDto!.Count.Should().Be(4); 
    
        var addedImage = responseDto!.First(i=>i.ImageUrl.Contains("test.jpg"));
        addedImage.ProfileId.Should().Be(profile.Id);
        addedImage.IsMainImage.Should().Be(false);
    }
    
    [Fact]
    public async Task RemoveImageFromProfile_RemovedImageWasMain_ReturnsImage()
    {
        var profile = await AddValidProfileToDbAsync();
        var images = ImageFakers.CreateImage().Clone()
            .RuleFor(i => i.ProfileId, profile.Id)
            .Generate(3);
        images[0].IsMainImage = true;
        profile.Images = images;
        
        await AddEntitiesToDbAsync(images);
        await UpdateDbEntityAsync(profile);

        var dto = new RemoveImageDto()
        {
            ImageId = images[0].Id,
            ProfileId = profile.Id
        };

        var requestMessage = new HttpRequestMessage(HttpMethod.Delete, $"{BaseUrl}")
        {
            Content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json")
        };
        var response = await Client.SendAsync(requestMessage);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseDto = await response.Content.ReadFromJsonAsync<ImageResponseDto>();
        responseDto.Should().NotBeNull();
        responseDto!.Id.Should().Be(images[0].Id);
    }
    
    [Fact]
    public async Task RemoveImageFromProfile_ImageNotExists_ReturnsNotFound()
    {
        var profile = await AddValidProfileToDbAsync();
        var images = ImageFakers.CreateImage().Clone()
            .RuleFor(i => i.ProfileId, profile.Id)
            .Generate(3);
        images[0].IsMainImage = true;
        profile.Images = images;
        
        await AddEntitiesToDbAsync(images);
        await UpdateDbEntityAsync(profile);

        var dto = new RemoveImageDto()
        {
            ImageId = -1,
            ProfileId = profile.Id
        };

        var requestMessage = new HttpRequestMessage(HttpMethod.Delete, $"{BaseUrl}")
        {
            Content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json")
        };
        var response = await Client.SendAsync(requestMessage);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var responseDto = await response.Content.ReadFromJsonAsync<ErrorDetails>();
        responseDto.Should().NotBeNull();
        responseDto!.ErrorType.Should().Be("NotFoundError");
    }
    
    [Fact]
    public async Task ChangeMainImage_ValidData_ReturnsImages()
    {
        var profile = await AddValidProfileToDbAsync();
        var images = ImageFakers.CreateImage().Clone()
            .RuleFor(i => i.ProfileId, profile.Id)
            .Generate(3);
        images[0].IsMainImage = true;
        profile.Images = images;
        
        await AddEntitiesToDbAsync(images);
        await UpdateDbEntityAsync(profile);

        var dto = new ChangeMainImageDto()
        {
            ImageId = images[2].Id,
            ProfileId = profile.Id
        };

        var response = await Client.PatchAsJsonAsync($"{BaseUrl}", dto);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseDto = await response.Content.ReadFromJsonAsync<List<ImageResponseDto>>();
        responseDto.Should().NotBeNull();
        responseDto!.Count.Should().Be(3);
        responseDto[0].Id.Should().Be(images[2].Id);
        responseDto[0].IsMainImage.Should().Be(true);
        var alreadyNotMainImage = responseDto.First(i => i.Id == images[0].Id);
        alreadyNotMainImage.IsMainImage.Should().Be(false);
    }
}