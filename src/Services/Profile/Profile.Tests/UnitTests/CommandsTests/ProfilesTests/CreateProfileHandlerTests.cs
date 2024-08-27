using System.Linq.Expressions;
using AutoMapper;
using FluentAssertions;
using Moq;
using Profile.Application.DTOs.Profile.Response;
using Profile.Application.Kafka.Producers;
using Profile.Application.UseCases.ProfileUseCases.Commands.Create;
using Profile.Domain.Interfaces.Repositories;
using Profile.Domain.Interfaces.Services;
using Profile.Domain.Models;
using Profile.Tests.Fakers;
using Shared.Messages.Profile;

namespace Profile.Tests.UnitTests.CommandsTests.ProfilesTests;

public class CreateProfileHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly Mock<IProducerService> _producerServiceMock;
    private readonly CreateProfileHandler _handler;

    public CreateProfileHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _cacheServiceMock = new Mock<ICacheService>();
        _producerServiceMock = new Mock<IProducerService>();
        
        _handler = new CreateProfileHandler(
            _unitOfWorkMock.Object, 
            _mapperMock.Object, 
            _cacheServiceMock.Object,
            _producerServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldCreateProfileAndReturnProfileResponseDto()
    {
        var profileDto = ProfileFakers.CreateCreateProfileDto().Generate();
        var profile = ProfileFakers.CreateUserProfile().Generate();
        var profileResponseDto = ProfileFakers.CreateProfileResponseDto().Generate();
        var profileCreatedMessage = new ProfileCreatedMessage();
        var cacheKey = $"profile:{profile.Id}";
        
        _mapperMock.Setup(m => m.Map<UserProfile>(profileDto))
            .Returns(profile);
        
        _unitOfWorkMock.Setup(u => u.ProfileRepository.CreateProfileAsync(profile, It.IsAny<CancellationToken>()))
            .ReturnsAsync(profile);
        
        _unitOfWorkMock.Setup(u => u.ProfileRepository.GetAllProfileInfoAsync(
            It.IsAny<Expression<Func<UserProfile, bool>>>(), 
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(profile);

        _mapperMock.Setup(m => m.Map<ProfileResponseDto>(profile))
            .Returns(profileResponseDto);
        
        _mapperMock.Setup(m => m.Map<ProfileCreatedMessage>(profile))
            .Returns(profileCreatedMessage);

        var result = await _handler.Handle(new CreateProfileCommand(profileDto), CancellationToken.None);

        result.Should().BeEquivalentTo(profileResponseDto);
        
        _unitOfWorkMock.Verify(u => u.ProfileRepository.CreateProfileAsync(profile, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.ProfileRepository.GetAllProfileInfoAsync(
            It.IsAny<Expression<Func<UserProfile, bool>>>(), 
            It.IsAny<CancellationToken>()), 
            Times.Once);
        
        _cacheServiceMock.Verify(c => c.SetAsync(cacheKey, profileResponseDto, null, It.IsAny<CancellationToken>()), Times.Once);
        _producerServiceMock.Verify(p => p.ProduceAsync(profileCreatedMessage), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenProfileIsNotCreated()
    {
        var profileDto = ProfileFakers.CreateCreateProfileDto().Generate();
        UserProfile? profile = null;
        
        _mapperMock.Setup(m => m.Map<UserProfile>(profileDto))
            .Returns(profile);

        _unitOfWorkMock.Setup(u => u.ProfileRepository.CreateProfileAsync(profile, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Failed to create profile"));

        var act = async () => await _handler.Handle(new CreateProfileCommand(profileDto), CancellationToken.None);

        await act.Should().ThrowAsync<Exception>();
        _unitOfWorkMock.Verify(u => u.ProfileRepository.CreateProfileAsync(profile, It.IsAny<CancellationToken>()), Times.Once);
        _cacheServiceMock.Verify(c => c.SetAsync(It.IsAny<string>(), It.IsAny<ProfileResponseDto>(), null, It.IsAny<CancellationToken>()), Times.Never);
        _producerServiceMock.Verify(p => p.ProduceAsync(It.IsAny<ProfileCreatedMessage>()), Times.Never);
    }
}