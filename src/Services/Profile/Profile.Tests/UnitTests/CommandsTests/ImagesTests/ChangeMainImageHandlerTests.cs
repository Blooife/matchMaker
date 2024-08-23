using AutoMapper;
using FluentAssertions;
using Moq;
using Profile.Application.DTOs.Image.Request;
using Profile.Application.DTOs.Image.Response;
using Profile.Application.DTOs.Profile.Response;
using Profile.Application.Exceptions;
using Profile.Application.Kafka.Producers;
using Profile.Application.UseCases.ImageUseCases.Commands.ChangeMainImage;
using Profile.Domain.Interfaces.Repositories;
using Profile.Domain.Interfaces.Services;
using Profile.Domain.Models;
using Shared.Messages.Profile;

namespace Profile.Tests.UnitTests.CommandsTests.ImagesTests;

public class ChangeMainImageHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IProducerService> _producerServiceMock;
    private readonly ChangeMainImageHandler _handler;

    public ChangeMainImageHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _cacheServiceMock = new Mock<ICacheService>();
        _mapperMock = new Mock<IMapper>();
        _producerServiceMock = new Mock<IProducerService>();

        _handler = new ChangeMainImageHandler(
            _unitOfWorkMock.Object,
            _cacheServiceMock.Object,
            _mapperMock.Object,
            _producerServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldChangeMainImageAndReturnUpdatedImages()
    {
        var profileId = "profile123";
        var imageId = 1;
        var newMainImageId = 2;

        var profileResponseDto = new ProfileResponseDto
        {
            Images = new List<ImageResponseDto>
            {
                new ImageResponseDto { Id = imageId, IsMainImage = true },
                new ImageResponseDto { Id = newMainImageId, IsMainImage = false }
            }
        };
        var profile = new UserProfile
        {
            Id = profileId,
            Images = new List<Image>
            {
                new Image { Id = imageId, IsMainImage = true },
                new Image { Id = newMainImageId, IsMainImage = false }
            }
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
        _unitOfWorkMock.Setup(u => u.ImageRepository.FirstOrDefaultAsync(newMainImageId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(profile.Images.First(i => i.Id == newMainImageId));
        _unitOfWorkMock.Setup(u => u.ImageRepository.UpdateImageAsync(It.IsAny<Image>()))
            .Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.SaveAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _mapperMock.Setup(m => m.Map<ProfileUpdatedMessage>(profile))
            .Returns(new ProfileUpdatedMessage { Id = profileId });
        _mapperMock.Setup(m => m.Map<IEnumerable<ImageResponseDto>>(It.IsAny<IEnumerable<Image>>()))
            .Returns(profileResponseDto.Images);

        var result = await _handler.Handle(new ChangeMainImageCommand(new ChangeMainImageDto
        {
            ProfileId = profileId,
            ImageId = newMainImageId
        }), CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().HaveCount(profileResponseDto.Images.Count);

        _unitOfWorkMock.Verify(u => u.ImageRepository.UpdateImageAsync(It.Is<Image>(i => i.Id == imageId && !i.IsMainImage)), Times.Once);
        _unitOfWorkMock.Verify(u => u.ImageRepository.UpdateImageAsync(It.Is<Image>(i => i.Id == newMainImageId && i.IsMainImage)), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        _cacheServiceMock.Verify(c => c.SetAsync(It.IsAny<string>(), It.IsAny<ProfileResponseDto>(), null, It.IsAny<CancellationToken>()), Times.Once);
        _producerServiceMock.Verify(p => p.ProduceAsync(It.Is<ProfileUpdatedMessage>(m => m.Id == profileId)), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenProfileDoesNotExist()
    {
        var profileId = "profile123";
        var imageId = 1;

        _cacheServiceMock.Setup(c => c.GetAsync(
            It.IsAny<string>(),
            It.IsAny<Func<Task<ProfileResponseDto?>>>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as ProfileResponseDto);

        var act = async () => await _handler.Handle(new ChangeMainImageCommand(new ChangeMainImageDto
        {
            ProfileId = profileId,
            ImageId = imageId
        }), CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();

        _unitOfWorkMock.Verify(u => u.ImageRepository.FirstOrDefaultAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.ImageRepository.UpdateImageAsync(It.IsAny<Image>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveAsync(It.IsAny<CancellationToken>()), Times.Never);
        _cacheServiceMock.Verify(c => c.SetAsync(It.IsAny<string>(), It.IsAny<ProfileResponseDto>(), null, It.IsAny<CancellationToken>()), Times.Never);
        _producerServiceMock.Verify(p => p.ProduceAsync(It.IsAny<ProfileUpdatedMessage>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenImageDoesNotExist()
    {
        var profileId = "profile123";
        var imageId = 1;
        var profileResponseDto = new ProfileResponseDto
        {
            Images = new List<ImageResponseDto>
            {
                new ImageResponseDto { Id = 2, IsMainImage = true }
            }
        };

        _cacheServiceMock.Setup(c => c.GetAsync(
            It.IsAny<string>(),
            It.IsAny<Func<Task<ProfileResponseDto?>>>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(profileResponseDto);
        _mapperMock.Setup(m => m.Map<UserProfile>(profileResponseDto))
            .Returns(new UserProfile { Id = profileId, Images = profileResponseDto.Images.Select(img => new Image { Id = img.Id, IsMainImage = img.IsMainImage }).ToList() });
        _unitOfWorkMock.Setup(u => u.ImageRepository.FirstOrDefaultAsync(imageId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Image);

        var act = async () => await _handler.Handle(new ChangeMainImageCommand(new ChangeMainImageDto
        {
            ProfileId = profileId,
            ImageId = imageId
        }), CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();

        _unitOfWorkMock.Verify(u => u.ImageRepository.UpdateImageAsync(It.IsAny<Image>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveAsync(It.IsAny<CancellationToken>()), Times.Never);
        _cacheServiceMock.Verify(c => c.SetAsync(It.IsAny<string>(), It.IsAny<ProfileResponseDto>(), null, It.IsAny<CancellationToken>()), Times.Never);
        _producerServiceMock.Verify(p => p.ProduceAsync(It.IsAny<ProfileUpdatedMessage>()), Times.Never);
    }
}