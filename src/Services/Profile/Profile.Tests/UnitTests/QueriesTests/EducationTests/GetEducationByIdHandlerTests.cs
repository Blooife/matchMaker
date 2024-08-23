using Moq;
using AutoMapper;
using FluentAssertions;
using Profile.Application.DTOs.Education.Response;
using Profile.Application.Exceptions;
using Profile.Application.UseCases.EducationUseCases.Queries.GetById;
using Profile.Domain.Interfaces.Repositories;
using Profile.Domain.Interfaces.Services;
using Profile.Domain.Models;
using Profile.Tests.Fakers;

namespace Profile.Tests.UnitTests.QueriesTests.EducationTests;

public class GetEducationByIdHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly GetEducationByIdHandler _handler;

    public GetEducationByIdHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _cacheServiceMock = new Mock<ICacheService>();
        _handler = new GetEducationByIdHandler(_unitOfWorkMock.Object, _mapperMock.Object, _cacheServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnCachedData_WhenCacheIsNotNull()
    {
        var educationId = 1;
        var cachedEducation = EducationFakers.CreateEducationResponseDto().Generate();
        var cacheKey = $"education:{educationId}";
        
        _cacheServiceMock.Setup(c => c.GetAsync<EducationResponseDto>(cacheKey, It.IsAny<CancellationToken>()))
            .ReturnsAsync(cachedEducation);

        var result = await _handler.Handle(new GetEducationByIdQuery(educationId), CancellationToken.None);

        result.Should().BeEquivalentTo(cachedEducation);
        _unitOfWorkMock.Verify(u => u.EducationRepository.FirstOrDefaultAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
        _cacheServiceMock.Verify(c => c.SetAsync(It.IsAny<string>(), It.IsAny<object>(), null, It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnDataFromRepository_WhenCacheIsNull()
    {
        var educationId = 1;
        var educationFromRepo = EducationFakers.CreateEducation().Generate();
        var mappedEducation = EducationFakers.CreateEducationResponseDto().Generate();
        var cacheKey = $"education:{educationId}";

        _cacheServiceMock.Setup(c => c.GetAsync<EducationResponseDto>(cacheKey, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as EducationResponseDto);
        _unitOfWorkMock.Setup(u => u.EducationRepository.FirstOrDefaultAsync(educationId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(educationFromRepo);
        _mapperMock.Setup(m => m.Map<EducationResponseDto>(educationFromRepo))
            .Returns(mappedEducation);

        var result = await _handler.Handle(new GetEducationByIdQuery(educationId), CancellationToken.None);

        result.Should().BeEquivalentTo(mappedEducation);
        _unitOfWorkMock.Verify(u => u.EducationRepository.FirstOrDefaultAsync(educationId, It.IsAny<CancellationToken>()), Times.Once);
        _cacheServiceMock.Verify(c => c.SetAsync(cacheKey, mappedEducation, null, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenEducationDoesNotExist()
    {
        var educationId = 1;
        var cacheKey = $"education:{educationId}";

        _cacheServiceMock.Setup(c => c.GetAsync<EducationResponseDto>(cacheKey, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as EducationResponseDto);
        _unitOfWorkMock.Setup(u => u.EducationRepository.FirstOrDefaultAsync(educationId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Education);

        Func<Task> act = async () => await _handler.Handle(new GetEducationByIdQuery(educationId), CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
        _unitOfWorkMock.Verify(u => u.EducationRepository.FirstOrDefaultAsync(educationId, It.IsAny<CancellationToken>()), Times.Once);
        _cacheServiceMock.Verify(c => c.SetAsync(It.IsAny<string>(), It.IsAny<object>(), null, It.IsAny<CancellationToken>()), Times.Never);
    }
}
