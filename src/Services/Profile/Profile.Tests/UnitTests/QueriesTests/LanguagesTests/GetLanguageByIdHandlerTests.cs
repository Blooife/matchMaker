using AutoMapper;
using FluentAssertions;
using Moq;
using Profile.Application.DTOs.Language.Response;
using Profile.Application.Exceptions;
using Profile.Application.UseCases.LanguageUseCases.Queries.GetById;
using Profile.Domain.Interfaces.Repositories;
using Profile.Domain.Interfaces.Services;
using Profile.Domain.Models;
using Profile.Tests.Fakers;

namespace Profile.Tests.UnitTests.QueriesTests.LanguagesTests;

public class GetLanguageByIdHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly GetLanguageByIdHandler _handler;

    public GetLanguageByIdHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _cacheServiceMock = new Mock<ICacheService>();
        _handler = new GetLanguageByIdHandler(_unitOfWorkMock.Object, _mapperMock.Object, _cacheServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnCachedData_WhenCacheIsNotNull()
    {
        var cachedLanguage = LanguageFakers.CreateLanguageResponseDto().Generate();
        var cacheKey = "language:1";
        
        _cacheServiceMock.Setup(c => c.GetAsync<LanguageResponseDto>(cacheKey, It.IsAny<CancellationToken>()))
            .ReturnsAsync(cachedLanguage);

        var result = await _handler.Handle(new GetLanguageByIdQuery(1), CancellationToken.None);

        result.Should().BeEquivalentTo(cachedLanguage);
        _unitOfWorkMock.Verify(u => u.LanguageRepository.FirstOrDefaultAsync(1, It.IsAny<CancellationToken>()), Times.Never);
        _cacheServiceMock.Verify(c => c.SetAsync(It.IsAny<string>(), It.IsAny<object>(), null, It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnDataFromRepository_WhenCacheIsNull()
    {
        var languageFromRepo = LanguageFakers.CreateLanguage().Generate();
        var languageDto = LanguageFakers.CreateLanguageResponseDto().Generate();
        var cacheKey = "language:1";

        _cacheServiceMock.Setup(c => c.GetAsync<LanguageResponseDto>(cacheKey, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as LanguageResponseDto);
        _unitOfWorkMock.Setup(u => u.LanguageRepository.FirstOrDefaultAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(languageFromRepo);
        _mapperMock.Setup(m => m.Map<LanguageResponseDto>(languageFromRepo))
            .Returns(languageDto);

        var result = await _handler.Handle(new GetLanguageByIdQuery(1), CancellationToken.None);

        result.Should().BeEquivalentTo(languageDto);
        _unitOfWorkMock.Verify(u => u.LanguageRepository.FirstOrDefaultAsync(1, It.IsAny<CancellationToken>()), Times.Once);
        _cacheServiceMock.Verify(c => c.SetAsync(cacheKey, languageDto, null, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenLanguageDoesNotExist()
    {
        var cacheKey = "language:1";
        
        _cacheServiceMock.Setup(c => c.GetAsync<LanguageResponseDto>(cacheKey, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as LanguageResponseDto);
        _unitOfWorkMock.Setup(u => u.LanguageRepository.FirstOrDefaultAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Language);

        Func<Task> act = async () => await _handler.Handle(new GetLanguageByIdQuery(1), CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
        _unitOfWorkMock.Verify(u => u.LanguageRepository.FirstOrDefaultAsync(1, It.IsAny<CancellationToken>()), Times.Once);
        _cacheServiceMock.Verify(c => c.SetAsync(It.IsAny<string>(), It.IsAny<object>(), null, It.IsAny<CancellationToken>()), Times.Never);
    }
}