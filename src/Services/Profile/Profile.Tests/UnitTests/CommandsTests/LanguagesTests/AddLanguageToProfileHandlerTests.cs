using System.Linq.Expressions;
using AutoMapper;
using FluentAssertions;
using Moq;
using Profile.Application.DTOs.Language.Request;
using Profile.Application.DTOs.Language.Response;
using Profile.Application.DTOs.Profile.Response;
using Profile.Application.Exceptions;
using Profile.Application.UseCases.LanguageUseCases.Commands.AddLanguageToProfile;
using Profile.Domain.Interfaces.Repositories;
using Profile.Domain.Interfaces.Services;
using Profile.Domain.Models;
using Profile.Tests.Fakers;

namespace Profile.Tests.UnitTests.CommandsTests.LanguagesTests;

public class AddLanguageToProfileHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly AddLanguageToProfileHandler _handler;

    public AddLanguageToProfileHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _cacheServiceMock = new Mock<ICacheService>();
        _handler = new AddLanguageToProfileHandler(_unitOfWorkMock.Object, _mapperMock.Object, _cacheServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenProfileDoesNotExist()
    {
        var command = new AddLanguageToProfileCommand(LanguageFakers.CreateAddLanguageToProfileDto().Generate());

        _unitOfWorkMock.Setup(u => u.ProfileRepository.GetAllProfileInfoAsync(It.IsAny<Expression<Func<UserProfile, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as UserProfile);

        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenLanguageDoesNotExist()
    {
        var command = new AddLanguageToProfileCommand(LanguageFakers.CreateAddLanguageToProfileDto().Generate());
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
    public async Task Handle_ShouldThrowAlreadyContainsException_WhenProfileAlreadyContainsLanguage()
    {
        var command = new AddLanguageToProfileCommand(LanguageFakers.CreateAddLanguageToProfileDto().Generate());
        var profileResponseDto = ProfileFakers.CreateProfileResponseDto().Generate();
        var profile = ProfileFakers.CreateUserProfile().Generate();
        var language = LanguageFakers.CreateLanguage().Generate();
        profile.Languages.Add(new Language()
        {
            Id = command.Dto.LanguageId,
        });
        
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

        await act.Should().ThrowAsync<AlreadyContainsException>();
    }

    [Fact]
    public async Task Handle_ShouldAddLanguageToProfile_WhenProfileDoesNotContainLanguage()
    {
        var command = new AddLanguageToProfileCommand(LanguageFakers.CreateAddLanguageToProfileDto().Generate());
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
        _mapperMock.Setup(m => m.Map<Language>(It.IsAny<AddLanguageToProfileDto>()))
            .Returns(LanguageFakers.CreateLanguage().Generate());
        _mapperMock.Setup(m => m.Map<List<LanguageResponseDto>>(profile.Languages))
            .Returns(LanguageFakers.CreateLanguageResponseDto().Generate(1));

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().HaveCount(1);
        _unitOfWorkMock.Verify(u => u.LanguageRepository.AddLanguageToProfileAsync(profile, It.IsAny<Language>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}