using System.Linq.Expressions;
using AutoMapper;
using FluentAssertions;
using Moq;
using Profile.Application.DTOs.Profile.Response;
using Profile.Application.Exceptions;
using Profile.Application.Kafka.Producers;
using Profile.Application.UseCases.ProfileUseCases.Commands.Update;
using Profile.Domain.Interfaces.Repositories;
using Profile.Domain.Interfaces.Services;
using Profile.Domain.Models;
using Profile.Tests.Fakers;
using Shared.Messages.Profile;

namespace Profile.Tests.UnitTests.CommandsTests.ProfilesTests;

public class UpdateProfileHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly Mock<IProducerService> _producerServiceMock;
    private readonly UpdateProfileHandler _handler;

    public UpdateProfileHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _cacheServiceMock = new Mock<ICacheService>();
        _producerServiceMock = new Mock<IProducerService>();
        
        _handler = new UpdateProfileHandler(
            _unitOfWorkMock.Object, 
            _mapperMock.Object, 
            _cacheServiceMock.Object,
            _producerServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldUpdateProfileAndReturnProfileResponseDto()
    {
        var profileDto = ProfileFakers.CreateUpdateProfileDto().Generate();
        var profile = ProfileFakers.CreateUserProfile().Generate();
        var profileResponseDto = ProfileFakers.CreateProfileResponseDto().Generate();
        var profileUpdatedMessage = new ProfileUpdatedMessage();
        var cacheKey = $"profile:{profile.Id}";
        
        _unitOfWorkMock.Setup(u => u.ProfileRepository.FirstOrDefaultAsync(profileDto.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(profile);

        _mapperMock.Setup(m => m.Map<UserProfile>(profileDto))
            .Returns(profile);
        
        _unitOfWorkMock.Setup(u => u.ProfileRepository.UpdateProfileAsync(profile))
            .ReturnsAsync(profile);
        
        _unitOfWorkMock.Setup(u => u.ProfileRepository.GetAllProfileInfoAsync(
            It.IsAny<Expression<Func<UserProfile, bool>>>(), 
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(profile);

        _mapperMock.Setup(m => m.Map<ProfileResponseDto>(profile))
            .Returns(profileResponseDto);
        
        _mapperMock.Setup(m => m.Map<ProfileUpdatedMessage>(profile))
            .Returns(profileUpdatedMessage);

        var result = await _handler.Handle(new UpdateProfileCommand(profileDto), CancellationToken.None);

        result.Should().BeEquivalentTo(profileResponseDto);
        
        _unitOfWorkMock.Verify(u => u.ProfileRepository.FirstOrDefaultAsync(profileDto.Id, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.ProfileRepository.UpdateProfileAsync(profile), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.ProfileRepository.GetAllProfileInfoAsync(
            It.IsAny<Expression<Func<UserProfile, bool>>>(), 
            It.IsAny<CancellationToken>()), 
            Times.Once);
        
        _cacheServiceMock.Verify(c => c.SetAsync(cacheKey, profileResponseDto, null, It.IsAny<CancellationToken>()), Times.Once);
        _producerServiceMock.Verify(p => p.ProduceAsync(profileUpdatedMessage), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenProfileDoesNotExist()
    {
        var profileDto = ProfileFakers.CreateUpdateProfileDto().Generate();
        
        _unitOfWorkMock.Setup(u => u.ProfileRepository.FirstOrDefaultAsync(profileDto.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as UserProfile);

        var act = async () => await _handler.Handle(new UpdateProfileCommand(profileDto), CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
        
        _unitOfWorkMock.Verify(u => u.ProfileRepository.FirstOrDefaultAsync(profileDto.Id, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.ProfileRepository.UpdateProfileAsync(It.IsAny<UserProfile>()), Times.Never);
        _cacheServiceMock.Verify(c => c.SetAsync(It.IsAny<string>(), It.IsAny<ProfileResponseDto>(), null, It.IsAny<CancellationToken>()), Times.Never);
        _producerServiceMock.Verify(p => p.ProduceAsync(It.IsAny<ProfileUpdatedMessage>()), Times.Never);
    }
}