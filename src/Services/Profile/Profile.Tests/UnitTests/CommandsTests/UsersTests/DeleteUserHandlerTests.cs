using System.Linq.Expressions;
using AutoMapper;
using FluentAssertions;
using Moq;
using Profile.Application.DTOs.User.Response;
using Profile.Application.Exceptions;
using Profile.Application.Kafka.Producers;
using Profile.Application.UseCases.UserUseCases.Commands.Delete;
using Profile.Domain.Interfaces.Repositories;
using Profile.Domain.Interfaces.Services;
using Profile.Domain.Models;
using Profile.Tests.Fakers;
using Shared.Messages.Profile;

namespace Profile.Tests.UnitTests.CommandsTests.UsersTests;

public class DeleteUserHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly Mock<IProducerService> _producerServiceMock;
    private readonly DeleteUserHandler _handler;

    public DeleteUserHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _cacheServiceMock = new Mock<ICacheService>();
        _producerServiceMock = new Mock<IProducerService>();

        _handler = new DeleteUserHandler(
            _unitOfWorkMock.Object,
            _mapperMock.Object,
            _cacheServiceMock.Object,
            _producerServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldDeleteUserAndReturnUserResponseDto()
    {
        var userId = "1";
        var user = UserFakers.CreateUser().Generate();
        var profile = ProfileFakers.CreateUserProfile().Generate();
        var userResponseDto = new UserResponseDto();
        var cacheKeyProfile = $"profile:{profile.Id}";

        _unitOfWorkMock.Setup(u => u.UserRepository.FirstOrDefaultAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _unitOfWorkMock.Setup(u => u.ProfileRepository.GetAsync(It.IsAny<Expression<Func<UserProfile, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<UserProfile> { profile });

        _mapperMock.Setup(m => m.Map<UserResponseDto>(user))
            .Returns(userResponseDto);

        var result = await _handler.Handle(new DeleteUserCommand(userId), CancellationToken.None);

        result.Should().BeEquivalentTo(userResponseDto);

        _unitOfWorkMock.Verify(u => u.UserRepository.FirstOrDefaultAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.UserRepository.DeleteUserAsync(user), Times.Once);
        _unitOfWorkMock.Verify(u => u.ProfileRepository.GetAsync(It.IsAny<Expression<Func<UserProfile, bool>>>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.ProfileRepository.DeleteProfileAsync(profile), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        _cacheServiceMock.Verify(c => c.RemoveAsync(cacheKeyProfile, It.IsAny<CancellationToken>()), Times.Once);
        _producerServiceMock.Verify(p => p.ProduceAsync(It.IsAny<ProfileDeletedMessage>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenUserDoesNotExist()
    {
        var userId = "1";

        _unitOfWorkMock.Setup(u => u.UserRepository.FirstOrDefaultAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as User);

        var act = async () => await _handler.Handle(new DeleteUserCommand(userId), CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();

        _unitOfWorkMock.Verify(u => u.UserRepository.FirstOrDefaultAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.UserRepository.DeleteUserAsync(It.IsAny<User>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.ProfileRepository.GetAsync(It.IsAny<Expression<Func<UserProfile, bool>>>(), It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.ProfileRepository.DeleteProfileAsync(It.IsAny<UserProfile>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveAsync(It.IsAny<CancellationToken>()), Times.Never);
        _cacheServiceMock.Verify(c => c.RemoveAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        _producerServiceMock.Verify(p => p.ProduceAsync(It.IsAny<ProfileDeletedMessage>()), Times.Never);
    }
}