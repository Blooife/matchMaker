using System.Linq.Expressions;
using AutoMapper;
using FluentAssertions;
using Moq;
using Profile.Application.DTOs.Image.Request;
using Profile.Application.DTOs.Image.Response;
using Profile.Application.DTOs.Profile.Response;
using Profile.Application.Exceptions;
using Profile.Application.Kafka.Producers;
using Profile.Application.UseCases.ImageUseCases.Commands.RemoveImage;
using Profile.Domain.Interfaces.Repositories;
using Profile.Domain.Interfaces.Services;
using Profile.Domain.Models;
using Shared.Messages.Profile;

namespace Profile.Tests.UnitTests.CommandsTests.ImagesTests;

public class RemoveImageHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IMinioService> _minioServiceMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly Mock<IProducerService> _producerServiceMock;
    private readonly RemoveImageHandler _handler;

    public RemoveImageHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _minioServiceMock = new Mock<IMinioService>();
        _cacheServiceMock = new Mock<ICacheService>();
        _producerServiceMock = new Mock<IProducerService>();

        _handler = new RemoveImageHandler(
            _unitOfWorkMock.Object,
            _mapperMock.Object,
            _minioServiceMock.Object,
            _cacheServiceMock.Object,
            _producerServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldRemoveImageAndReturnImageResponseDto_AndProduceProfileUpdatedMessage()
    {
        var profileId = "profile123";
        var imageId = 123;
        var imageEntity = new Image
        {
            ProfileId = profileId,
            ImageUrl = $"http://minio-endpoint/bucket/{profileId}/test.jpg",
            IsMainImage = false,
            UploadTimestamp = DateTime.UtcNow
        };
        var profileResponseDto = new ProfileResponseDto
        {
            Images = new List<ImageResponseDto>
            {
                new ImageResponseDto { ImageUrl = imageEntity.ImageUrl }
            }
        };
        var profile = new UserProfile
        {
            Id = profileId,
            Images = new List<Image> { imageEntity }
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
        _unitOfWorkMock.Setup(u => u.ImageRepository.FirstOrDefaultAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(imageEntity);
        _unitOfWorkMock.Setup(u => u.ImageRepository.RemoveImageFromProfileAsync(It.IsAny<Image>()))
            .Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.SaveAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _minioServiceMock.Setup(m => m.DeleteFileAsync(It.IsAny<string>()))
            .Returns(Task.CompletedTask);
        _mapperMock.Setup(m => m.Map<ImageResponseDto>(It.IsAny<Image>()))
            .Returns(new ImageResponseDto { ImageUrl = imageEntity.ImageUrl });

        var result = await _handler.Handle(new RemoveImageCommand(new RemoveImageDto
        {
            ProfileId = profileId,
            ImageId = imageId
        }), CancellationToken.None);

        result.Should().NotBeNull();
        result.ImageUrl.Should().Be(imageEntity.ImageUrl);

        _unitOfWorkMock.Verify(u => u.ImageRepository.RemoveImageFromProfileAsync(It.IsAny<Image>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        _cacheServiceMock.Verify(c => c.SetAsync(It.IsAny<string>(), It.IsAny<ProfileResponseDto>(), null, It.IsAny<CancellationToken>()), Times.Once);
        _producerServiceMock.Verify(p => p.ProduceAsync(It.IsAny<ProfileUpdatedMessage>()), Times.Never);
        _minioServiceMock.Verify(m => m.DeleteFileAsync(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldRemoveImageAndReturnImageResponseDto_AndProduceProfileUpdatedMessage_WhenMainImageIsRemoved()
    {
        var profileId = "profile123";
        var imageId = 123;
        var imageEntity = new Image
        {
            ProfileId = profileId,
            ImageUrl = $"http://minio-endpoint/bucket/{profileId}/test.jpg",
            IsMainImage = true,
            UploadTimestamp = DateTime.UtcNow
        };
        var profileResponseDto = new ProfileResponseDto
        {
            Images = new List<ImageResponseDto>
            {
                new ImageResponseDto { ImageUrl = imageEntity.ImageUrl }
            }
        };
        var newMainImage = new Image
        {
            ProfileId = profileId,
            ImageUrl = $"http://minio-endpoint/bucket/{profileId}/new.jpg",
            IsMainImage = false,
            UploadTimestamp = DateTime.UtcNow.AddMinutes(-10)
        };
        var profile = new UserProfile
        {
            Id = profileId,
            Images = new List<Image> { imageEntity, newMainImage }
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
        _unitOfWorkMock.Setup(u => u.ImageRepository.FirstOrDefaultAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(imageEntity);
        _unitOfWorkMock.Setup(u => u.ImageRepository.RemoveImageFromProfileAsync(It.IsAny<Image>()))
            .Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.ImageRepository.UpdateImageAsync(It.IsAny<Image>()))
            .Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.SaveAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _minioServiceMock.Setup(m => m.DeleteFileAsync(It.IsAny<string>()))
            .Returns(Task.CompletedTask);
        _mapperMock.Setup(m => m.Map<ImageResponseDto>(It.IsAny<Image>()))
            .Returns(new ImageResponseDto { ImageUrl = imageEntity.ImageUrl });

        var result = await _handler.Handle(new RemoveImageCommand(new RemoveImageDto
        {
            ProfileId = profileId,
            ImageId = imageId
        }), CancellationToken.None);

        result.Should().NotBeNull();
        result.ImageUrl.Should().Be(imageEntity.ImageUrl);

        _unitOfWorkMock.Verify(u => u.ImageRepository.RemoveImageFromProfileAsync(It.IsAny<Image>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        _cacheServiceMock.Verify(c => c.SetAsync(It.IsAny<string>(), It.IsAny<ProfileResponseDto>(), null, It.IsAny<CancellationToken>()), Times.Once);
        _producerServiceMock.Verify(p => p.ProduceAsync(It.IsAny<ProfileUpdatedMessage>()), Times.Once);
        _minioServiceMock.Verify(m => m.DeleteFileAsync(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenProfileDoesNotExist()
    {
        var profileId = "profile123";
        var imageId = 123;

        _cacheServiceMock.Setup(c => c.GetAsync(
            It.IsAny<string>(),
            It.IsAny<Func<Task<ProfileResponseDto?>>>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as ProfileResponseDto);

        var act = async () => await _handler.Handle(new RemoveImageCommand(new RemoveImageDto
        {
            ProfileId = profileId,
            ImageId = imageId
        }), CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();

        _unitOfWorkMock.Verify(u => u.ProfileRepository.GetAllProfileInfoAsync(It.IsAny<Expression<Func<UserProfile, bool>>>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenImageDoesNotExist()
    {
        var profileId = "profile123";
        var imageId = 123;
        var profileResponseDto = new ProfileResponseDto
        {
            Images = new List<ImageResponseDto>()
        };

        _cacheServiceMock.Setup(c => c.GetAsync(
            It.IsAny<string>(),
            It.IsAny<Func<Task<ProfileResponseDto?>>>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(profileResponseDto);
        _mapperMock.Setup(m => m.Map<UserProfile>(profileResponseDto))
            .Returns(new UserProfile { Id = profileId, Images = new List<Image>() });
        _unitOfWorkMock.Setup(u => u.ImageRepository.FirstOrDefaultAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Image);
        
        var act = async () => await _handler.Handle(new RemoveImageCommand(new RemoveImageDto
        {
            ProfileId = profileId,
            ImageId = imageId
        }), CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();

        _unitOfWorkMock.Verify(u => u.ImageRepository.RemoveImageFromProfileAsync(It.IsAny<Image>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenFailedToRemoveImageFromMinio()
    {
        var profileId = "profile123";
        var imageId = 123;
        var imageEntity = new Image
        {
            ProfileId = profileId,
            ImageUrl = $"http://minio-endpoint/bucket/{profileId}/test.jpg",
            IsMainImage = false,
            UploadTimestamp = DateTime.UtcNow
        };
        var profileResponseDto = new ProfileResponseDto
        {
            Images = new List<ImageResponseDto>
            {
                new ImageResponseDto { ImageUrl = imageEntity.ImageUrl }
            }
        };
        var profile = new UserProfile
        {
            Id = profileId,
            Images = new List<Image> { imageEntity }
        };

        _cacheServiceMock.Setup(c => c.GetAsync(
            It.IsAny<string>(),
            It.IsAny<Func<Task<ProfileResponseDto?>>>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(profileResponseDto);

        _mapperMock.Setup(m => m.Map<UserProfile>(profileResponseDto))
            .Returns(profile);
        _unitOfWorkMock.Setup(u => u.ImageRepository.FirstOrDefaultAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(imageEntity);
        _unitOfWorkMock.Setup(u => u.ImageRepository.RemoveImageFromProfileAsync(It.IsAny<Image>()))
            .Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.SaveAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _minioServiceMock.Setup(m => m.DeleteFileAsync(It.IsAny<string>()))
            .ThrowsAsync(new Exception("Minio delete failed"));

        var act = async () => await _handler.Handle(new RemoveImageCommand(new RemoveImageDto
        {
            ProfileId = profileId,
            ImageId = imageId
        }), CancellationToken.None);

        await act.Should().ThrowAsync<Exception>();

        _unitOfWorkMock.Verify(u => u.ImageRepository.RemoveImageFromProfileAsync(It.IsAny<Image>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}