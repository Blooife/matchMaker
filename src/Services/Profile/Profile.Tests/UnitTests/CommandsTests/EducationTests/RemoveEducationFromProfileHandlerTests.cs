using System.Linq.Expressions;
using AutoMapper;
using FluentAssertions;
using Moq;
using Profile.Application.DTOs.Education.Response;
using Profile.Application.DTOs.Profile.Response;
using Profile.Application.Exceptions;
using Profile.Application.UseCases.EducationUseCases.Commands.RemoveEducationFromProfile;
using Profile.Domain.Interfaces.Repositories;
using Profile.Domain.Interfaces.Services;
using Profile.Domain.Models;
using Profile.Tests.Fakers;

namespace Profile.Tests.UnitTests.CommandsTests.EducationTests;

public class RemoveEducationFromProfileHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly RemoveEducationFromProfileHandler _handler;

    public RemoveEducationFromProfileHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _cacheServiceMock = new Mock<ICacheService>();
        _handler = new RemoveEducationFromProfileHandler(_unitOfWorkMock.Object, _mapperMock.Object, _cacheServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldRemoveEducationFromProfile_WhenEducationAndProfileExist()
    {
        var removeDto = EducationFakers.CreateRemoveEducationFromProfileDto().Generate();
        var command = new RemoveEducationFromProfileCommand(removeDto);
        var profile = ProfileFakers.CreateUserProfile().Generate();
        var profileResponseDto = ProfileFakers.CreateProfileResponseDto().Generate();
        var education = new Education()
        {
            Id = command.Dto.EducationId
        };
        var profileEducation = new ProfileEducation()
        {
            EducationId = command.Dto.EducationId,
            ProfileId = command.Dto.ProfileId
        };
        profile.ProfileEducations.Add(profileEducation);
        var resDtos = EducationFakers.CreateProfileEducationResponseDto().Generate(5);
        
        _cacheServiceMock.Setup(cs => cs.GetAsync(
                It.IsAny<string>(), 
                It.IsAny<Func<Task<ProfileResponseDto?>>>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(profileResponseDto);  
        _mapperMock.Setup(m => m.Map<UserProfile>(profileResponseDto))
            .Returns(profile);
        _unitOfWorkMock.Setup(u => u.EducationRepository.FirstOrDefaultAsync(removeDto.EducationId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(education);
        _unitOfWorkMock.Setup(u => u.EducationRepository.RemoveEducationFromProfileAsync(It.IsAny<UserProfile>(), It.IsAny<ProfileEducation>()))
            .Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.SaveAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _mapperMock.Setup(m => m.Map<List<ProfileEducationResponseDto>>(profile.ProfileEducations))
            .Returns(resDtos);

        var result = await _handler.Handle(command, CancellationToken.None);

        _unitOfWorkMock.Verify(u => u.EducationRepository.RemoveEducationFromProfileAsync(It.IsAny<UserProfile>(), It.IsAny<ProfileEducation>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        _cacheServiceMock.Verify(c => c.SetAsync(It.IsAny<string>(), It.IsAny<ProfileResponseDto>(), null, It.IsAny<CancellationToken>()), Times.Once);
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(resDtos);

    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenProfileDoesNotExist()
    {
        var command = new RemoveEducationFromProfileCommand(EducationFakers.CreateRemoveEducationFromProfileDto().Generate());

        _unitOfWorkMock.Setup(u => u.ProfileRepository.GetAllProfileInfoAsync(It.IsAny<Expression<Func<UserProfile, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as UserProfile);

        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenEducationDoesNotExist()
    {
        var command = new RemoveEducationFromProfileCommand(EducationFakers.CreateRemoveEducationFromProfileDto().Generate());
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
    public async Task Handle_ShouldThrowNotContainsException_WhenEducationNotAssociatedWithProfile()
    {
        var command = new RemoveEducationFromProfileCommand(EducationFakers.CreateRemoveEducationFromProfileDto().Generate());
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
        
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<NotContainsException>();
    }
}