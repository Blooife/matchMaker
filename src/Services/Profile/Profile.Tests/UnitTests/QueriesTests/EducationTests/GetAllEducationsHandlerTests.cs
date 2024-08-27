using Moq;
using AutoMapper;
using FluentAssertions;
using Profile.Application.DTOs.Education.Response;
using Profile.Application.UseCases.EducationUseCases.Queries.GetAll;
using Profile.Domain.Interfaces.Repositories;
using Profile.Domain.Interfaces.Services;
using Profile.Tests.Fakers;

namespace Profile.Tests.UnitTests.QueriesTests.EducationTests;

public class GetAllEducationsHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly GetAllEducationsHandler _handler;

    public GetAllEducationsHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _cacheServiceMock = new Mock<ICacheService>();
        _handler = new GetAllEducationsHandler(_unitOfWorkMock.Object, _mapperMock.Object, _cacheServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnCachedData_WhenCacheIsNotNull()
    {
        var cachedEducations = EducationFakers.CreateEducationResponseDto().Generate(3);
        
        _cacheServiceMock.Setup(c => c.GetAsync<IEnumerable<EducationResponseDto>>("educations", It.IsAny<CancellationToken>()))
            .ReturnsAsync(cachedEducations);

        var result = await _handler.Handle(new GetAllEducationsQuery(), CancellationToken.None);

        result.Should().BeEquivalentTo(cachedEducations);
        _unitOfWorkMock.Verify(u => u.EducationRepository.GetAllAsync(It.IsAny<CancellationToken>()), Times.Never);
        _cacheServiceMock.Verify(c => c.SetAsync(It.IsAny<string>(), It.IsAny<object>(), null, It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnDataFromRepository_WhenCacheIsNull()
    {
        var educationsFromRepo = EducationFakers.CreateEducation().Generate(3);
        var mappedEducations = EducationFakers.CreateEducationResponseDto().Generate(3);

        _cacheServiceMock.Setup(c => c.GetAsync<IEnumerable<EducationResponseDto>>("educations", It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as IEnumerable<EducationResponseDto>);
        _unitOfWorkMock.Setup(u => u.EducationRepository.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(educationsFromRepo);
        _mapperMock.Setup(m => m.Map<List<EducationResponseDto>>(educationsFromRepo))
            .Returns(mappedEducations);

        var result = await _handler.Handle(new GetAllEducationsQuery(), CancellationToken.None);

        result.Should().BeEquivalentTo(mappedEducations);
        _unitOfWorkMock.Verify(u => u.EducationRepository.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        _cacheServiceMock.Verify(c => c.SetAsync("educations", mappedEducations, null, It.IsAny<CancellationToken>()), Times.Once);
    }
}