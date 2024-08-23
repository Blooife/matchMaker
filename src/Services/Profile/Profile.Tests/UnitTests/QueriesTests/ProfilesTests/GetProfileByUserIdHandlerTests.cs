using System.Linq.Expressions;
using AutoMapper;
using FluentAssertions;
using Moq;
using Profile.Application.DTOs.Profile.Response;
using Profile.Application.Exceptions;
using Profile.Application.UseCases.ProfileUseCases.Queries.GetByUserId;
using Profile.Domain.Interfaces.Repositories;
using Profile.Domain.Interfaces.Services;
using Profile.Domain.Models;
using Profile.Tests.Fakers;

namespace Profile.Tests.UnitTests.QueriesTests.ProfilesTests;

public class GetProfileByUserIdHandlerTests
{
     private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly GetProfileByUserIdHandler _handler;

    public GetProfileByUserIdHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _cacheServiceMock = new Mock<ICacheService>();
        _handler = new GetProfileByUserIdHandler(_unitOfWorkMock.Object, _mapperMock.Object, _cacheServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnDataFromRepository_WhenProfileExists()
    {
        var userId = "1";
        var profile = ProfileFakers.CreateUserProfile().Generate();
        var profileDto = ProfileFakers.CreateProfileResponseDto().Generate();
        var cacheKey = $"profile:{profile.Id}";
        
        _unitOfWorkMock.Setup(u => u.ProfileRepository.GetAllProfileInfoAsync(It.IsAny<Expression<System.Func<UserProfile,bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(profile);
        _mapperMock.Setup(m => m.Map<ProfileResponseDto>(profile))
            .Returns(profileDto);

        var result = await _handler.Handle(new GetProfileByUserIdQuery(userId), CancellationToken.None);

        result.Should().BeEquivalentTo(profileDto);
        _unitOfWorkMock.Verify(u => u.ProfileRepository.GetAllProfileInfoAsync(It.IsAny< Expression<System.Func<UserProfile, bool>>>(), It.IsAny<CancellationToken>()), Times.Once);
        _cacheServiceMock.Verify(c => c.SetAsync(cacheKey, profileDto, null, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenProfileDoesNotExist()
    {
        var userId = "999";

        _unitOfWorkMock.Setup(u => u.ProfileRepository.GetAllProfileInfoAsync(It.IsAny<Expression<System.Func<UserProfile,bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as UserProfile);

        Func<Task> act = async () => await _handler.Handle(new GetProfileByUserIdQuery(userId), CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
        _unitOfWorkMock.Verify(u => u.ProfileRepository.GetAllProfileInfoAsync(It.IsAny<Expression<Func<UserProfile,bool>>>(), It.IsAny<CancellationToken>()), Times.Once);
        _cacheServiceMock.Verify(c => c.SetAsync(It.IsAny<string>(), It.IsAny<ProfileResponseDto>(), null, It.IsAny<CancellationToken>()), Times.Never);
    }
}