using System.Linq.Expressions;
using AutoMapper;
using FluentAssertions;
using Moq;
using Profile.Application.DTOs.Interest.Request;
using Profile.Application.DTOs.Interest.Response;
using Profile.Application.DTOs.Profile.Response;
using Profile.Application.Exceptions;
using Profile.Application.UseCases.InterestUseCases.Commands.AddInterestToProfile;
using Profile.Domain.Interfaces.Repositories;
using Profile.Domain.Interfaces.Services;
using Profile.Domain.Models;
using Profile.Tests.Fakers;

namespace Profile.Tests.UnitTests.CommandsTests.InterestsTests;

public class AddInterestToProfileHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly AddInterestToProfileHandler _handler;

    public AddInterestToProfileHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _cacheServiceMock = new Mock<ICacheService>();
        _handler = new AddInterestToProfileHandler(_unitOfWorkMock.Object, _mapperMock.Object, _cacheServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenProfileDoesNotExist()
    {
        var command = new AddInterestToProfileCommand(InterestFakers.CreateAddInterestToProfileDto().Generate());

        _unitOfWorkMock.Setup(u => u.ProfileRepository.GetAllProfileInfoAsync(It.IsAny<Expression<Func<UserProfile, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as UserProfile);

        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenInterestDoesNotExist()
    {
        var command = new AddInterestToProfileCommand(InterestFakers.CreateAddInterestToProfileDto().Generate());
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
    public async Task Handle_ShouldThrowAlreadyContainsException_WhenProfileAlreadyContainsInterest()
    {
        var command = new AddInterestToProfileCommand(InterestFakers.CreateAddInterestToProfileDto().Generate());
        var profileResponseDto = ProfileFakers.CreateProfileResponseDto().Generate();
        var profile = ProfileFakers.CreateUserProfile().Generate();
        var interest = InterestFakers.CreateInterest().Generate();
        profile.Interests.Add(new Interest()
        {
            Id = command.Dto.InterestId,
        });
        
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

        await act.Should().ThrowAsync<AlreadyContainsException>();
    }

    [Fact]
    public async Task Handle_ShouldAddInterestToProfile_WhenProfileDoesNotContainInterest()
    {
        var command = new AddInterestToProfileCommand(InterestFakers.CreateAddInterestToProfileDto().Generate());
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
        _mapperMock.Setup(m => m.Map<Interest>(It.IsAny<AddInterestToProfileDto>()))
            .Returns(InterestFakers.CreateInterest().Generate());
        _mapperMock.Setup(m => m.Map<List<InterestResponseDto>>(profile.Interests))
            .Returns(InterestFakers.CreateInterestResponseDto().Generate(1));

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().HaveCount(1);
        _unitOfWorkMock.Verify(u => u.InterestRepository.AddInterestToProfileAsync(profile, It.IsAny<Interest>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}