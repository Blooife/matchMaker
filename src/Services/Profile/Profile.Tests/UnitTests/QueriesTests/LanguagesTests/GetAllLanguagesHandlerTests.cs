using AutoMapper;
using FluentAssertions;
using Moq;
using Profile.Application.DTOs.Language.Response;
using Profile.Application.UseCases.LanguageUseCases.Queries.GetAll;
using Profile.Domain.Interfaces.Repositories;
using Profile.Domain.Interfaces.Services;
using Profile.Tests.Fakers;

namespace Profile.Tests.UnitTests.QueriesTests.LanguagesTests;

public class GetAllLanguagesHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly GetAllLanguagesHandler _handler;

    public GetAllLanguagesHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _cacheServiceMock = new Mock<ICacheService>();
        _handler = new GetAllLanguagesHandler(_unitOfWorkMock.Object, _mapperMock.Object, _cacheServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnCachedData_WhenCacheIsNotNull()
    {
        var cachedLanguages = LanguageFakers.CreateLanguageResponseDto().Generate(2);
        _cacheServiceMock.Setup(c => c.GetAsync<IEnumerable<LanguageResponseDto>>("languages", It.IsAny<CancellationToken>()))
            .ReturnsAsync(cachedLanguages);

        var result = await _handler.Handle(new GetAllLanguagesQuery(), CancellationToken.None);

        result.Should().BeEquivalentTo(cachedLanguages);
        _unitOfWorkMock.Verify(u => u.LanguageRepository.GetAllAsync(It.IsAny<CancellationToken>()), Times.Never);
        _cacheServiceMock.Verify(c => c.SetAsync(It.IsAny<string>(), It.IsAny<object>(), null, It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnDataFromRepository_WhenCacheIsNull()
    {
        var languagesFromRepo = LanguageFakers.CreateLanguage().Generate(2);
        var countryDtos = LanguageFakers.CreateLanguageResponseDto().Generate(2);

        _cacheServiceMock.Setup(c => c.GetAsync<IEnumerable<LanguageResponseDto>>("languages", It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as IEnumerable<LanguageResponseDto>);
        _unitOfWorkMock.Setup(u => u.LanguageRepository.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(languagesFromRepo);
        _mapperMock.Setup(m => m.Map<List<LanguageResponseDto>>(languagesFromRepo))
            .Returns(countryDtos);

        var result = await _handler.Handle(new GetAllLanguagesQuery(), CancellationToken.None);

        result.Should().BeEquivalentTo(countryDtos);
        _unitOfWorkMock.Verify(u => u.LanguageRepository.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        _cacheServiceMock.Verify(c => c.SetAsync("languages", countryDtos, null, It.IsAny<CancellationToken>()), Times.Once);
    }
}