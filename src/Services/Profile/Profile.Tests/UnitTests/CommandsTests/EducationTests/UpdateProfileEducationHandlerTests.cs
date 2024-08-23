using System.Linq.Expressions;
using AutoMapper;
using FluentAssertions;
using Moq;
using Profile.Application.DTOs.Education.Response;
using Profile.Application.DTOs.Profile.Response;
using Profile.Application.Exceptions;
using Profile.Application.UseCases.EducationUseCases.Commands.Update;
using Profile.Domain.Interfaces.Repositories;
using Profile.Domain.Interfaces.Services;
using Profile.Domain.Models;
using Profile.Tests.Fakers;

namespace Profile.Tests.UnitTests.CommandsTests.EducationTests;

public class UpdateProfileEducationHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly UpdateProfileEducationHandler _handler;

    public UpdateProfileEducationHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _cacheServiceMock = new Mock<ICacheService>();
        _handler = new UpdateProfileEducationHandler(_unitOfWorkMock.Object, _mapperMock.Object, _cacheServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldUpdateEducationInProfile_WhenEducationAndProfileExist()
    {
        var profileId = "profile-id";
        var command = new UpdateProfileEducationCommand(EducationFakers.CreateUpdateProfileEducationDto().Generate());
        var educationId = command.Dto.EducationId;
        var newDescription = command.Dto.Description;
        var profile = new UserProfile
        {
            Id = profileId,
            ProfileEducations = new List<ProfileEducation>
            {
                new ProfileEducation
                {
                    ProfileId = profileId,
                    EducationId = educationId,
                    Description = "Old description"
                }
            }
        };
        var education = new Education { Id = educationId };
        var profileResponseDto = ProfileFakers.CreateProfileResponseDto().Generate();

        _cacheServiceMock.Setup(cs => cs.GetAsync(
                It.IsAny<string>(), 
                It.IsAny<Func<Task<ProfileResponseDto?>>>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(profileResponseDto);  
        _mapperMock.Setup(m => m.Map<UserProfile>(profileResponseDto))
            .Returns(profile);
        _unitOfWorkMock.Setup(u => u.EducationRepository.FirstOrDefaultAsync(educationId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(education);
        _unitOfWorkMock.Setup(u => u.EducationRepository.UpdateProfilesEducationAsync(It.IsAny<ProfileEducation>(), newDescription))
            .Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.SaveAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _mapperMock.Setup(m => m.Map<ProfileResponseDto>(profile))
            .Returns(new ProfileResponseDto { Education = new List<ProfileEducationResponseDto>() });
        _mapperMock.Setup(m => m.Map<ProfileEducationResponseDto>(It.IsAny<ProfileEducation>()))
            .Returns(new ProfileEducationResponseDto { EducationId = educationId, Description = newDescription });

        var result = await _handler.Handle(command, CancellationToken.None);

        _unitOfWorkMock.Verify(u => u.EducationRepository.UpdateProfilesEducationAsync(It.IsAny<ProfileEducation>(), newDescription), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        _cacheServiceMock.Verify(c => c.SetAsync(It.IsAny<string>(), It.IsAny<ProfileResponseDto>(), null, It.IsAny<CancellationToken>()), Times.Once);
        result.Should().NotBeNull();
        result.Description.Should().BeEquivalentTo(newDescription);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenProfileDoesNotExist()
    {
        var command = new UpdateProfileEducationCommand(EducationFakers.CreateUpdateProfileEducationDto().Generate());

        _unitOfWorkMock.Setup(u => u.ProfileRepository.GetAllProfileInfoAsync(It.IsAny<Expression<Func<UserProfile, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as UserProfile);

        var act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenEducationDoesNotExist()
    {
        var command = new UpdateProfileEducationCommand(EducationFakers.CreateUpdateProfileEducationDto().Generate());
        var profile = ProfileFakers.CreateUserProfile();

        _cacheServiceMock.Setup(cs => cs.GetAsync(
                It.IsAny<string>(), 
                It.IsAny<Func<Task<ProfileResponseDto?>>>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(It.IsAny<ProfileResponseDto>());  
        _mapperMock.Setup(m => m.Map<UserProfile>(It.IsAny<ProfileResponseDto>()))
            .Returns(profile);
        _unitOfWorkMock.Setup(u => u.EducationRepository.FirstOrDefaultAsync(command.Dto.EducationId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Education);

        var act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Handle_ShouldThrowNotContainsException_WhenEducationNotAssociatedWithProfile()
    {
        var command = new UpdateProfileEducationCommand(EducationFakers.CreateUpdateProfileEducationDto().Generate());
        var profile = ProfileFakers.CreateUserProfile();
        var education = new Education { Id = command.Dto.EducationId};

        _cacheServiceMock.Setup(cs => cs.GetAsync(
                It.IsAny<string>(), 
                It.IsAny<Func<Task<ProfileResponseDto?>>>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(It.IsAny<ProfileResponseDto>());  
        _mapperMock.Setup(m => m.Map<UserProfile>(It.IsAny<ProfileResponseDto>()))
            .Returns(profile);
        _unitOfWorkMock.Setup(u => u.EducationRepository.FirstOrDefaultAsync(command.Dto.EducationId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(education);

        var act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<NotContainsException>();
    }
}
