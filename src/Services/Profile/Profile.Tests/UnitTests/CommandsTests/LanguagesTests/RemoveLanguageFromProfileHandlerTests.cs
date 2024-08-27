using System.Linq.Expressions;
using AutoMapper;
using FluentAssertions;
using Moq;
using Profile.Application.DTOs.Language.Response;
using Profile.Application.DTOs.Profile.Response;
using Profile.Application.Exceptions;
using Profile.Application.UseCases.LanguageUseCases.Commands.RemoveLanguageFromProfile;
using Profile.Domain.Interfaces.Repositories;
using Profile.Domain.Interfaces.Services;
using Profile.Domain.Models;
using Profile.Tests.Fakers;

namespace Profile.Tests.UnitTests.CommandsTests.LanguagesTests;

public class RemoveLanguageFromProfileHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly RemoveLanguageFromProfileHandler _handler;

    public RemoveLanguageFromProfileHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _cacheServiceMock = new Mock<ICacheService>();
        _handler = new RemoveLanguageFromProfileHandler(_unitOfWorkMock.Object, _mapperMock.Object, _cacheServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldRemoveLanguageFromProfile_WhenLanguageAndProfileExist()
    {
        var removeDto = LanguageFakers.CreateRemoveLanguageFromProfileDto().Generate();
        var command = new RemoveLanguageFromProfileCommand(removeDto);
        var profile = ProfileFakers.CreateUserProfile().Generate();
        var profileResponseDto = ProfileFakers.CreateProfileResponseDto().Generate();
        var language = new Language()
        {
            Id = command.Dto.LanguageId
        };
        profile.Languages.Add(language);
        var resDtos = LanguageFakers.CreateLanguageResponseDto().Generate(5);
        
        _cacheServiceMock.Setup(cs => cs.GetAsync(
                It.IsAny<string>(), 
                It.IsAny<Func<Task<ProfileResponseDto?>>>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(profileResponseDto);  
        _mapperMock.Setup(m => m.Map<UserProfile>(profileResponseDto))
            .Returns(profile);
        _unitOfWorkMock.Setup(u => u.LanguageRepository.FirstOrDefaultAsync(removeDto.LanguageId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(language);
        _unitOfWorkMock.Setup(u => u.LanguageRepository.RemoveLanguageFromProfileAsync(profile, It.IsAny<Language>()))
            .Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.SaveAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _mapperMock.Setup(m => m.Map<List<LanguageResponseDto>>(profile.Languages))
            .Returns(resDtos);

        var result = await _handler.Handle(command, CancellationToken.None);

        _unitOfWorkMock.Verify(u => u.LanguageRepository.RemoveLanguageFromProfileAsync(profile, It.IsAny<Language>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        _cacheServiceMock.Verify(c => c.SetAsync(It.IsAny<string>(), It.IsAny<ProfileResponseDto>(), null, It.IsAny<CancellationToken>()), Times.Once);
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(resDtos);

    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenProfileDoesNotExist()
    {
        var command = new RemoveLanguageFromProfileCommand(LanguageFakers.CreateRemoveLanguageFromProfileDto().Generate());

        _unitOfWorkMock.Setup(u => u.ProfileRepository.GetAllProfileInfoAsync(It.IsAny<Expression<Func<UserProfile, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as UserProfile);

        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenLanguageDoesNotExist()
    {
        var command = new RemoveLanguageFromProfileCommand(LanguageFakers.CreateRemoveLanguageFromProfileDto().Generate());
        var profile = ProfileFakers.CreateUserProfile().Generate();
        var profileResponseDto = ProfileFakers.CreateProfileResponseDto().Generate();

        _cacheServiceMock.Setup(cs => cs.GetAsync(
                It.IsAny<string>(), 
                It.IsAny<Func<Task<ProfileResponseDto?>>>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(profileResponseDto);  
        _mapperMock.Setup(m => m.Map<UserProfile>(profileResponseDto))
            .Returns(profile);
        _unitOfWorkMock.Setup(u => u.LanguageRepository.FirstOrDefaultAsync(command.Dto.LanguageId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Language);

        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Handle_ShouldThrowNotContainsException_WhenLanguageNotAssociatedWithProfile()
    {
        var command = new RemoveLanguageFromProfileCommand(LanguageFakers.CreateRemoveLanguageFromProfileDto().Generate());
        var profileResponseDto = ProfileFakers.CreateProfileResponseDto().Generate();
        var profile = ProfileFakers.CreateUserProfile().Generate();
        var language = LanguageFakers.CreateLanguage().Generate();
        
        _cacheServiceMock.Setup(cs => cs.GetAsync(
                It.IsAny<string>(), 
                It.IsAny<Func<Task<ProfileResponseDto?>>>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(profileResponseDto);  
        _mapperMock.Setup(m => m.Map<UserProfile>(profileResponseDto))
            .Returns(profile);
        _unitOfWorkMock.Setup(u => u.LanguageRepository.FirstOrDefaultAsync(command.Dto.LanguageId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(language);
        
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<NotContainsException>();
    }
}