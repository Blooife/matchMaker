using System.Linq.Expressions;
using Moq;
using AutoMapper;
using FluentAssertions;
using Profile.Application.DTOs.Education.Request;
using Profile.Application.DTOs.Education.Response;
using Profile.Application.DTOs.Profile.Response;
using Profile.Application.Exceptions;
using Profile.Application.UseCases.EducationUseCases.Commands.AddEducationToProfile;
using Profile.Domain.Interfaces.Repositories;
using Profile.Domain.Interfaces.Services;
using Profile.Domain.Models;
using Profile.Tests.Fakers;

namespace Profile.Tests.UnitTests.CommandsTests.EducationTests;

public class AddEducationToProfileHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly AddEducationToProfileHandler _handler;

    public AddEducationToProfileHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _cacheServiceMock = new Mock<ICacheService>();
        _handler = new AddEducationToProfileHandler(_unitOfWorkMock.Object, _mapperMock.Object, _cacheServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenProfileDoesNotExist()
    {
        var command = new AddEducationToProfileCommand(EducationFakers.CreateAddEducationToProfileDto().Generate());

        _unitOfWorkMock.Setup(u => u.ProfileRepository.GetAllProfileInfoAsync(It.IsAny<Expression<Func<UserProfile, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as UserProfile);

        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenEducationDoesNotExist()
    {
        var command = new AddEducationToProfileCommand(EducationFakers.CreateAddEducationToProfileDto().Generate());
        var profile = ProfileFakers.CreateUserProfile().Generate();
        var profileResponseDto = ProfileFakers.CreateProfileResponseDto().Generate();

        _cacheServiceMock.Setup(cs => cs.GetAsync(
                It.IsAny<string>(), 
                It.IsAny<Func<Task<ProfileResponseDto?>>>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(profileResponseDto);  
        _mapperMock.Setup(m => m.Map<UserProfile>(profileResponseDto))
            .Returns(profile);
        _unitOfWorkMock.Setup(u => u.EducationRepository.FirstOrDefaultAsync(command.Dto.EducationId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Education);

        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Handle_ShouldThrowAlreadyContainsException_WhenProfileAlreadyContainsEducation()
    {
        var command = new AddEducationToProfileCommand(EducationFakers.CreateAddEducationToProfileDto().Generate());
        var profileResponseDto = ProfileFakers.CreateProfileResponseDto().Generate();
        var profile = ProfileFakers.CreateUserProfile().Generate();
        var education = EducationFakers.CreateEducation().Generate();
        profile.ProfileEducations.Add(new ProfileEducation()
        {
            EducationId = command.Dto.EducationId,
            ProfileId = command.Dto.ProfileId,
        });
        
        _cacheServiceMock.Setup(cs => cs.GetAsync(
                It.IsAny<string>(), 
                It.IsAny<Func<Task<ProfileResponseDto?>>>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(profileResponseDto);  
        _mapperMock.Setup(m => m.Map<UserProfile>(profileResponseDto))
            .Returns(profile);
        _unitOfWorkMock.Setup(u => u.EducationRepository.FirstOrDefaultAsync(command.Dto.EducationId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(education);
        
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<AlreadyContainsException>();
    }

    [Fact]
    public async Task Handle_ShouldAddEducationToProfile_WhenProfileDoesNotContainEducation()
    {
        var command = new AddEducationToProfileCommand(EducationFakers.CreateAddEducationToProfileDto().Generate());
        var profileResponseDto = ProfileFakers.CreateProfileResponseDto().Generate();
        var profile = ProfileFakers.CreateUserProfile().Generate();
        var education = EducationFakers.CreateEducation().Generate();

        _cacheServiceMock.Setup(cs => cs.GetAsync(
                It.IsAny<string>(), 
                It.IsAny<Func<Task<ProfileResponseDto?>>>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(profileResponseDto);  
        _mapperMock.Setup(m => m.Map<UserProfile>(profileResponseDto))
            .Returns(profile);
        _unitOfWorkMock.Setup(u => u.EducationRepository.FirstOrDefaultAsync(command.Dto.EducationId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(education);
        _mapperMock.Setup(m => m.Map<ProfileEducation>(It.IsAny<AddEducationToProfileDto>()))
            .Returns(EducationFakers.CreateProfileEducation().Generate());
        _mapperMock.Setup(m => m.Map<List<ProfileEducationResponseDto>>(profile.ProfileEducations))
            .Returns(EducationFakers.CreateProfileEducationResponseDto().Generate(1));

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().HaveCount(1);
        _unitOfWorkMock.Verify(u => u.EducationRepository.AddEducationToProfileAsync(profile, It.IsAny<ProfileEducation>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
