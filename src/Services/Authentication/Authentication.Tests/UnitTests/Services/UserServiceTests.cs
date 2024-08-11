using Authentication.BusinessLogic.DTOs.Response;
using Authentication.BusinessLogic.Exceptions;
using Authentication.BusinessLogic.Producers;
using Authentication.BusinessLogic.Services.Implementations;
using Authentication.DataLayer.Models;
using Authentication.DataLayer.Repositories.Interfaces;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using Shared.Messages.Authentication;
using Shared.Models;

namespace Authentication.Tests.UnitTests.Services;

public class UserServiceTests
{
    private readonly UserService _userService;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ILogger<UserService>> _loggerMock;
    private readonly Mock<IProducerService> _producerServiceMock;

    public UserServiceTests()
    {
        _userRepositoryMock= new Mock<IUserRepository>();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILogger<UserService>>();
        _producerServiceMock = new Mock<IProducerService>();

        _userService = new UserService(
            _userRepositoryMock.Object,
            _mapperMock.Object,
            _loggerMock.Object,
            _producerServiceMock.Object
        );
    }
    
    [Fact]
    public async Task DeleteUserByIdAsync_ShouldThrowNotFoundException_WhenUserNotFound()
    {
        var userId = "non-existing-id";
        _userRepositoryMock.Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User)null);

        Func<Task> act = async () => await _userService.DeleteUserByIdAsync(userId, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
        _loggerMock.VerifyLog(LogLevel.Error, Times.Once());
    }

    [Fact]
    public async Task DeleteUserByIdAsync_ShouldThrowDeleteUserException_WhenDeletionFails()
    {
        var userId = "existing-id";
        var user = new User { Id = userId };
        var result = IdentityResult.Failed(new IdentityError { Description = "Error deleting user" });

        _userRepositoryMock.Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _userRepositoryMock.Setup(r => r.DeleteUserByIdAsync(user))
            .ReturnsAsync(result);

        Func<Task> act = async () => await _userService.DeleteUserByIdAsync(userId, CancellationToken.None);

        await act.Should().ThrowAsync<DeleteUserException>()
            .WithMessage(ExceptionMessages.DeleteUserFailed);
        _loggerMock.VerifyLog(LogLevel.Error, Times.Once());
    }

    [Fact]
    public async Task GetAllUsersAsync_ShouldReturnListOfUsers()
    {
        var users = new List<User>
        {
            new User { Id = "1", Email = "user1@example.com" },
            new User { Id = "2", Email = "user2@example.com" }
        };
        var userDtos = users.Select(user => new UserResponseDto { Id = user.Id, Email = user.Email }).ToList();
        foreach (var dto in userDtos)
        {
            dto.Roles = new List<string> { "User" };
        }

        _userRepositoryMock.Setup(r => r.GetAllUsersAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(users);
        _userRepositoryMock.Setup(r => r.GetRolesAsync(It.IsAny<User>()))
            .ReturnsAsync(new List<string> { "User" });

        _mapperMock.Setup(m => m.Map<List<UserResponseDto>>(It.IsAny<List<User>>()))
            .Returns(userDtos);

        var result = await _userService.GetAllUsersAsync(CancellationToken.None);

        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(userDtos);
    }
    
    [Fact]
    public async Task DeleteUserByIdAsync_ShouldDeleteUserAndProduceMessage_WhenUserFoundAndDeletionSucceeds()
    {
        var userId = "existing-id";
        var user = new User { Id = userId };
        var result = IdentityResult.Success; // Успешный результат удаления
        var userDeletedMessage = new UserDeletedMessage { Id = userId }; // Сообщение, которое будет отправлено

        _userRepositoryMock.Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _userRepositoryMock.Setup(r => r.DeleteUserByIdAsync(user))
            .ReturnsAsync(result);

        _mapperMock.Setup(m => m.Map<UserDeletedMessage>(user))
            .Returns(userDeletedMessage);

        var response = await _userService.DeleteUserByIdAsync(userId, CancellationToken.None);

        response.Should().BeEquivalentTo(new GeneralResponseDto { Message = "User deleted successfully" });

        _userRepositoryMock.Verify(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
        _userRepositoryMock.Verify(r => r.DeleteUserByIdAsync(user), Times.Once);
        _mapperMock.Verify(m => m.Map<UserDeletedMessage>(user), Times.Once);
        _producerServiceMock.Verify(p => p.ProduceAsync(userDeletedMessage), Times.Once);
        _loggerMock.VerifyLog(LogLevel.Error, Times.Never());
    }

    [Fact]
    public async Task GetPaginatedUsersAsync_ShouldReturnPagedListOfUsers()
    {
        int pageSize = 10;
        int pageNumber = 1;
        var users = new List<User>
        {
            new User { Id = "1", Email = "user1@example.com" }
        };
        var totalCount = 1;
        var userDtos = users.Select(user => new UserResponseDto { Id = user.Id, Email = user.Email }).ToList();

        _userRepositoryMock.Setup(r => r.GetPagedUsersAsync(pageNumber, pageSize))
            .ReturnsAsync((users, totalCount));
        _userRepositoryMock.Setup(r => r.GetRolesAsync(It.IsAny<User>()))
            .ReturnsAsync(new List<string> { "User" });

        _mapperMock.Setup(m => m.Map<List<UserResponseDto>>(It.IsAny<List<User>>()))
            .Returns(userDtos);

        var result = await _userService.GetPaginatedUsersAsync(pageSize, pageNumber);

        result.Should().BeOfType<PagedList<UserResponseDto>>();
        result.Count.Should().Be(1);
        result.TotalCount.Should().Be(totalCount);
    }

    [Fact]
    public async Task GetUserByIdAsync_ShouldThrowNotFoundException_WhenUserNotFound()
    {
        var userId = "non-existing-id";
        _userRepositoryMock.Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User)null);

        Func<Task<UserResponseDto>> act = async () => await _userService.GetUserByIdAsync(userId, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
        _loggerMock.Verify(l => l.Log(LogLevel.Error, It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((object v, Type t) => v.ToString().Contains($"Get user by id failed: User with id = {userId} was not found")),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
    }

    [Fact]
    public async Task GetUserByEmailAsync_ShouldThrowNotFoundException_WhenUserNotFound()
    {
        var email = "non-existing-email@example.com";
        _userRepositoryMock.Setup(r => r.GetByEmailAsync(email, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User)null);

        Func<Task<UserResponseDto>> act = async () => await _userService.GetUserByEmailAsync(email, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
        _loggerMock.VerifyLog(LogLevel.Error, Times.Once());
    }

    [Fact]
    public async Task GetUsersRolesAsync_ShouldThrowNotFoundException_WhenUserNotFound()
    {
        var userId = "non-existing-id";
        _userRepositoryMock.Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User)null);

        Func<Task<IEnumerable<RoleResponseDto>>> act = async () => await _userService.GetUsersRolesAsync(userId, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
        _loggerMock.VerifyLog(LogLevel.Error, Times.Once());
    }
}