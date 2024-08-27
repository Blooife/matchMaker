using System.Linq.Expressions;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using Profile.Application.DTOs.Image.Request;
using Profile.Application.DTOs.Image.Response;
using Profile.Application.DTOs.Profile.Response;
using Profile.Application.Exceptions;
using Profile.Application.Kafka.Producers;
using Profile.Application.UseCases.ImageUseCases.Commands.AddImage;
using Profile.Domain.Interfaces.Repositories;
using Profile.Domain.Interfaces.Services;
using Profile.Domain.Models;
using Profile.Tests.Fakers;
using Shared.Messages.Profile;

namespace Profile.Tests.UnitTests.CommandsTests.ImagesTests;

public class AddImageHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IMinioService> _minioServiceMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly Mock<IProducerService> _producerServiceMock;
    private readonly AddImageHandler _handler;

    public AddImageHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _minioServiceMock = new Mock<IMinioService>();
        _cacheServiceMock = new Mock<ICacheService>();
        _producerServiceMock = new Mock<IProducerService>();

        _handler = new AddImageHandler(
            _unitOfWorkMock.Object,
            _mapperMock.Object,
            _minioServiceMock.Object,
            _cacheServiceMock.Object,
            _producerServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldAddImageAndReturnImageResponseDto_NoMessagesProduced()
    {
        var profileId = "profile123";
        var file = new Mock<IFormFile>();
        var fileName = "test.jpg";
        var stream = new MemoryStream();
        file.Setup(f => f.FileName).Returns(fileName);
        file.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
            .Callback<Stream, CancellationToken>((s, _) => stream.CopyTo(s));

        var profileResponseDto = new ProfileResponseDto
        {
            Images = new List<ImageResponseDto>()
        };
        var profile = new UserProfile
        {
            Id = profileId,
            Images = new List<Image>
            {
                new Image { IsMainImage = true, UploadTimestamp = DateTime.UtcNow.AddMinutes(-10) }
            }
        };
        var imageEntity = new Image
        {
            ProfileId = profileId,
            ImageUrl = $"http://minio-endpoint/bucket/{profileId}/{fileName}",
            IsMainImage = true,
            UploadTimestamp = DateTime.UtcNow
        };

        _cacheServiceMock.Setup(c => c.GetAsync(
            It.IsAny<string>(),
            It.IsAny<Func<Task<ProfileResponseDto?>>>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(profileResponseDto);

        _mapperMock.Setup(m => m.Map<UserProfile>(profileResponseDto))
            .Returns(profile);

        _mapperMock.Setup(m => m.Map<ProfileResponseDto>(profile))
            .Returns(profileResponseDto);

        _unitOfWorkMock.Setup(u => u.ImageRepository.AddImageToProfileAsync(It.IsAny<Image>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(imageEntity);

        _minioServiceMock.Setup(m => m.UploadFileAsync(It.IsAny<string>(), It.IsAny<IFormFile>()))
            .Returns(Task.CompletedTask);

        _mapperMock.Setup(m => m.Map<IEnumerable<ImageResponseDto>>(It.IsAny<List<Image>>()))
            .Returns(profile.Images.Select(img => new ImageResponseDto { ImageUrl = img.ImageUrl }));

        var result = await _handler.Handle(new AddImageCommand(new AddImageDto
        {
            ProfileId = profileId,
            file = file.Object
        }), CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().HaveCount(2);

        _unitOfWorkMock.Verify(u => u.ImageRepository.AddImageToProfileAsync(It.IsAny<Image>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        _cacheServiceMock.Verify(c => c.SetAsync(It.IsAny<string>(), It.IsAny<ProfileResponseDto>(), null, It.IsAny<CancellationToken>()), Times.Once);
        _producerServiceMock.Verify(p => p.ProduceAsync(It.IsAny<ProfileUpdatedMessage>()), Times.Never);
        _minioServiceMock.Verify(m => m.UploadFileAsync(It.IsAny<string>(), It.IsAny<IFormFile>()), Times.Once);
    }
    
    [Fact]
    public async Task Handle_ShouldAddImageAndReturnImageResponseDto_ProfileUpdatedMessageProduced()
    {
        var profileId = "profile123";
        var file = new Mock<IFormFile>();
        var fileName = "test.jpg";
        var stream = new MemoryStream();
        file.Setup(f => f.FileName).Returns(fileName);
        file.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
            .Callback<Stream, CancellationToken>((s, _) => stream.CopyTo(s));

        var profile = ProfileFakers.CreateUserProfile().Generate();
        var profileResponseDto = ProfileFakers.CreateProfileResponseDto().Generate();
        profile.Images = new List<Image>();
        
        var imageEntity = new Image
        {
            ProfileId = profileId,
            ImageUrl = $"http://minio-endpoint/bucket/{profileId}/{fileName}",
            IsMainImage = true,
            UploadTimestamp = DateTime.UtcNow
        };

        _cacheServiceMock.Setup(c => c.GetAsync(
            It.IsAny<string>(),
            It.IsAny<Func<Task<ProfileResponseDto?>>>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(profileResponseDto);

        _mapperMock.Setup(m => m.Map<UserProfile>(profileResponseDto))
            .Returns(profile);

        _mapperMock.Setup(m => m.Map<ProfileResponseDto>(profile))
            .Returns(profileResponseDto);

        _unitOfWorkMock.Setup(u => u.ImageRepository.AddImageToProfileAsync(It.IsAny<Image>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(imageEntity);

        _minioServiceMock.Setup(m => m.UploadFileAsync(It.IsAny<string>(), It.IsAny<IFormFile>()))
            .Returns(Task.CompletedTask);

        _mapperMock.Setup(m => m.Map<IEnumerable<ImageResponseDto>>(It.IsAny<List<Image>>()))
            .Returns(profile.Images.Select(img => new ImageResponseDto { ImageUrl = img.ImageUrl }));

        var result = await _handler.Handle(new AddImageCommand(new AddImageDto
        {
            ProfileId = profileId,
            file = file.Object
        }), CancellationToken.None);

        result.Should().NotBeNull();

        _unitOfWorkMock.Verify(u => u.ImageRepository.AddImageToProfileAsync(It.IsAny<Image>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        _cacheServiceMock.Verify(c => c.SetAsync(It.IsAny<string>(), It.IsAny<ProfileResponseDto>(), null, It.IsAny<CancellationToken>()), Times.Once);
        _producerServiceMock.Verify(p => p.ProduceAsync(It.IsAny<ProfileUpdatedMessage>()), Times.Once);
        _minioServiceMock.Verify(m => m.UploadFileAsync(It.IsAny<string>(), It.IsAny<IFormFile>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenProfileDoesNotExist()
    {
        var profileId = "profile123";
        var file = new Mock<IFormFile>();
        file.Setup(f => f.FileName).Returns("test.jpg");

        _cacheServiceMock.Setup(c => c.GetAsync(
            It.IsAny<string>(),
            It.IsAny<Func<Task<ProfileResponseDto?>>>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as ProfileResponseDto);

        var act = async () => await _handler.Handle(new AddImageCommand(new AddImageDto
        {
            ProfileId = profileId,
            file = file.Object
        }), CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();

        _unitOfWorkMock.Verify(u => u.ProfileRepository.GetAllProfileInfoAsync(It.IsAny<Expression<Func<UserProfile, bool>>>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldThrowImageUploadException_WhenFileExtensionIsNotAllowed()
    {
        var profileId = "profile123";
        var file = new Mock<IFormFile>();
        var profile = ProfileFakers.CreateUserProfile().Generate();
        var profileResponseDto = ProfileFakers.CreateProfileResponseDto().Generate();
        
        file.Setup(f => f.FileName).Returns("test.txt");
        _cacheServiceMock.Setup(cs => cs.GetAsync(
                It.IsAny<string>(), 
                It.IsAny<Func<Task<ProfileResponseDto?>>>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(profileResponseDto);  
        _mapperMock.Setup(m => m.Map<UserProfile>(profileResponseDto))
            .Returns(profile);
        
        var act = async () => await _handler.Handle(new AddImageCommand(new AddImageDto
        {
            ProfileId = profileId,
            file = file.Object
        }), CancellationToken.None);

        await act.Should().ThrowAsync<ImageUploadException>();

        _unitOfWorkMock.Verify(u => u.ImageRepository.AddImageToProfileAsync(It.IsAny<Image>(), It.IsAny<CancellationToken>()), Times.Never);
        _minioServiceMock.Verify(m => m.UploadFileAsync(It.IsAny<string>(), It.IsAny<IFormFile>()), Times.Never);
    }
}