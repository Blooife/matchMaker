using System.Linq.Expressions;
using AutoMapper;
using FluentAssertions;
using Moq;
using Profile.Application.DTOs.Interest.Response;
using Profile.Application.DTOs.Profile.Response;
using Profile.Application.Exceptions;
using Profile.Application.UseCases.InterestUseCases.Commands.RemoveInterestFromProfile;
using Profile.Domain.Interfaces.Repositories;
using Profile.Domain.Interfaces.Services;
using Profile.Domain.Models;
using Profile.Tests.Fakers;

namespace Profile.Tests.UnitTests.CommandsTests.InterestsTests;

public class RemoveInterestFromProfileHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly RemoveInterestFromProfileHandler _handler;

    public RemoveInterestFromProfileHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _cacheServiceMock = new Mock<ICacheService>();
        _handler = new RemoveInterestFromProfileHandler(_unitOfWorkMock.Object, _mapperMock.Object, _cacheServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldRemoveInterestFromProfile_WhenInterestAndProfileExist()
    {
        var removeDto = InterestFakers.CreateRemoveInterestFromProfileDto().Generate();
        var command = new RemoveInterestFromProfileCommand(removeDto);
        var profile = ProfileFakers.CreateUserProfile().Generate();
        var profileResponseDto = ProfileFakers.CreateProfileResponseDto().Generate();
        var interest = new Interest()
        {
            Id = command.Dto.InterestId
        };
        profile.Interests.Add(interest);
        var resDtos = InterestFakers.CreateInterestResponseDto().Generate(5);
        
        _cacheServiceMock.Setup(cs => cs.GetAsync(
                It.IsAny<string>(), 
                It.IsAny<Func<Task<ProfileResponseDto?>>>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(profileResponseDto);  
        _mapperMock.Setup(m => m.Map<UserProfile>(profileResponseDto))
            .Returns(profile);
        _unitOfWorkMock.Setup(u => u.InterestRepository.FirstOrDefaultAsync(removeDto.InterestId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(interest);
        _unitOfWorkMock.Setup(u => u.InterestRepository.RemoveInterestFromProfileAsync(profile, It.IsAny<Interest>()))
            .Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.SaveAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _mapperMock.Setup(m => m.Map<List<InterestResponseDto>>(profile.Interests))
            .Returns(resDtos);

        var result = await _handler.Handle(command, CancellationToken.None);

        _unitOfWorkMock.Verify(u => u.InterestRepository.RemoveInterestFromProfileAsync(profile, It.IsAny<Interest>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        _cacheServiceMock.Verify(c => c.SetAsync(It.IsAny<string>(), It.IsAny<ProfileResponseDto>(), null, It.IsAny<CancellationToken>()), Times.Once);
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(resDtos);

    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenProfileDoesNotExist()
    {
        var command = new RemoveInterestFromProfileCommand(InterestFakers.CreateRemoveInterestFromProfileDto().Generate());

        _unitOfWorkMock.Setup(u => u.ProfileRepository.GetAllProfileInfoAsync(It.IsAny<Expression<Func<UserProfile, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as UserProfile);

        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenInterestDoesNotExist()
    {
        var command = new RemoveInterestFromProfileCommand(InterestFakers.CreateRemoveInterestFromProfileDto().Generate());
        var profile = ProfileFakers.CreateUserProfile().Generate();
        var profileResponseDto = ProfileFakers.CreateProfileResponseDto().Generate();

        _cacheServiceMock.Setup(cs => cs.GetAsync(
                It.IsAny<string>(), 
                It.IsAny<Func<Task<ProfileResponseDto?>>>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(profileResponseDto);  
        _mapperMock.Setup(m => m.Map<UserProfile>(profileResponseDto))
            .Returns(profile);
        _unitOfWorkMock.Setup(u => u.InterestRepository.FirstOrDefaultAsync(command.Dto.InterestId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Interest);

        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Handle_ShouldThrowNotContainsException_WhenInterestNotAssociatedWithProfile()
    {
        var command = new RemoveInterestFromProfileCommand(InterestFakers.CreateRemoveInterestFromProfileDto().Generate());
        var profileResponseDto = ProfileFakers.CreateProfileResponseDto().Generate();
        var profile = ProfileFakers.CreateUserProfile().Generate();
        var interest = InterestFakers.CreateInterest().Generate();
        
        _cacheServiceMock.Setup(cs => cs.GetAsync(
                It.IsAny<string>(), 
                It.IsAny<Func<Task<ProfileResponseDto?>>>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(profileResponseDto);  
        _mapperMock.Setup(m => m.Map<UserProfile>(profileResponseDto))
            .Returns(profile);
        _unitOfWorkMock.Setup(u => u.InterestRepository.FirstOrDefaultAsync(command.Dto.InterestId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(interest);
        
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<NotContainsException>();
    }
}