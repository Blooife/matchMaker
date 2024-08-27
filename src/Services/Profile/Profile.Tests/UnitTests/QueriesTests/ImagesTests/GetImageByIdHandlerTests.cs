using AutoMapper;
using FluentAssertions;
using Moq;
using Profile.Application.DTOs.Image.Response;
using Profile.Application.Exceptions;
using Profile.Application.UseCases.ImageUseCases.Queries.GetById;
using Profile.Domain.Interfaces.Repositories;
using Profile.Domain.Interfaces.Services;
using Profile.Domain.Models;
using Profile.Tests.Fakers;

namespace Profile.Tests.UnitTests.QueriesTests.ImagesTests;

public class GetImageByIdHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly GetImageByIdHandler _handler;

    public GetImageByIdHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _cacheServiceMock = new Mock<ICacheService>();
        _handler = new GetImageByIdHandler(_unitOfWorkMock.Object, _mapperMock.Object, _cacheServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnCachedData_WhenCacheIsNotNull()
    {
        var cachedImage = ImageFakers.CreateImageResponseDto().Generate();
        var cacheKey = "image:1";
        
        _cacheServiceMock.Setup(c => c.GetAsync<ImageResponseDto>(cacheKey, It.IsAny<CancellationToken>()))
            .ReturnsAsync(cachedImage);

        var result = await _handler.Handle(new GetImageByIdQuery(1), CancellationToken.None);

        result.Should().BeEquivalentTo(cachedImage);
        _unitOfWorkMock.Verify(u => u.ImageRepository.FirstOrDefaultAsync(1, It.IsAny<CancellationToken>()), Times.Never);
        _cacheServiceMock.Verify(c => c.SetAsync(It.IsAny<string>(), It.IsAny<object>(), null, It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnDataFromRepository_WhenCacheIsNull()
    {
        var imageFromRepo = ImageFakers.CreateImage().Generate();
        var imageDto = ImageFakers.CreateImageResponseDto().Generate();
        var cacheKey = "image:1";

        _cacheServiceMock.Setup(c => c.GetAsync<ImageResponseDto>(cacheKey, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as ImageResponseDto);
        _unitOfWorkMock.Setup(u => u.ImageRepository.FirstOrDefaultAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(imageFromRepo);
        _mapperMock.Setup(m => m.Map<ImageResponseDto>(imageFromRepo))
            .Returns(imageDto);

        var result = await _handler.Handle(new GetImageByIdQuery(1), CancellationToken.None);

        result.Should().BeEquivalentTo(imageDto);
        _unitOfWorkMock.Verify(u => u.ImageRepository.FirstOrDefaultAsync(1, It.IsAny<CancellationToken>()), Times.Once);
        _cacheServiceMock.Verify(c => c.SetAsync(cacheKey, imageDto, null, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenImageDoesNotExist()
    {
        var cacheKey = "image:1";
        
        _cacheServiceMock.Setup(c => c.GetAsync<ImageResponseDto>(cacheKey, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as ImageResponseDto);
        _unitOfWorkMock.Setup(u => u.ImageRepository.FirstOrDefaultAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Image);

        Func<Task> act = async () => await _handler.Handle(new GetImageByIdQuery(1), CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
        _unitOfWorkMock.Verify(u => u.ImageRepository.FirstOrDefaultAsync(1, It.IsAny<CancellationToken>()), Times.Once);
        _cacheServiceMock.Verify(c => c.SetAsync(It.IsAny<string>(), It.IsAny<object>(), null, It.IsAny<CancellationToken>()), Times.Never);
    }
}