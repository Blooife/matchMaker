using Authentication.BusinessLogic.DTOs.Response;
using Authentication.BusinessLogic.Exceptions;
using Authentication.BusinessLogic.Producers;
using Authentication.BusinessLogic.Services.Implementations;
using Authentication.DataLayer.Models;
using Authentication.DataLayer.Repositories.Interfaces;
using Authentication.Tests.Fakers;
using AutoMapper;
using Bogus;
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
    private readonly Faker<User> _userFaker = TestDataGenerator.CreateUser();

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
        var userId = _userFaker.Generate().Id;
        _userRepositoryMock.Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as User);

        Func<Task> act = async () => await _userService.DeleteUserByIdAsync(userId, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
        _loggerMock.VerifyLog(LogLevel.Error, Times.Once());
    }

    [Fact]
    public async Task DeleteUserByIdAsync_ShouldThrowDeleteUserException_WhenDeletionFails()
    {
        var user = _userFaker.Generate();
        var result = IdentityResult.Failed();

        _userRepositoryMock.Setup(r => r.GetByIdAsync(user.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _userRepositoryMock.Setup(r => r.DeleteUserByIdAsync(user))
            .ReturnsAsync(result);

        Func<Task> act = async () => await _userService.DeleteUserByIdAsync(user.Id, CancellationToken.None);

        await act.Should().ThrowAsync<DeleteUserException>()
            .WithMessage(ExceptionMessages.DeleteUserFailed);
        _loggerMock.VerifyLog(LogLevel.Error, Times.Once());
    }
    
    [Fact]
    public async Task DeleteUserByIdAsync_ShouldDeleteUserAndProduceMessage_WhenUserFoundAndDeletionSucceeds()
    {
        var user = _userFaker.Generate();
        var userDeletedMessage = new UserDeletedMessage { Id = user.Id };
        var result = IdentityResult.Success; 

        _userRepositoryMock.Setup(r => r.GetByIdAsync(user.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _userRepositoryMock.Setup(r => r.DeleteUserByIdAsync(user))
            .ReturnsAsync(result);

        _mapperMock.Setup(m => m.Map<UserDeletedMessage>(user))
            .Returns(userDeletedMessage);

        var response = await _userService.DeleteUserByIdAsync(user.Id, CancellationToken.None);

        response.Should().BeEquivalentTo(new GeneralResponseDto { Message = "User deleted successfully" });

        _userRepositoryMock.Verify(r => r.GetByIdAsync(user.Id, It.IsAny<CancellationToken>()), Times.Once);
        _userRepositoryMock.Verify(r => r.DeleteUserByIdAsync(user), Times.Once);
        _mapperMock.Verify(m => m.Map<UserDeletedMessage>(user), Times.Once);
        _producerServiceMock.Verify(p => p.ProduceAsync(userDeletedMessage), Times.Once);
        _loggerMock.VerifyLog(LogLevel.Error, Times.Never());
    }

    [Fact]
    public async Task GetPaginatedUsersAsync_ReturnsPagedListOfUsers()
    {
        int pageSize = 10;
        int pageNumber = 1;
        var users = _userFaker.Generate(5);
        
        var totalCount = 5;
        var userDtos = users.Select(user => new UserResponseDto { Id = user.Id, Email = user.Email }).ToList();

        _userRepositoryMock.Setup(r => r.GetPagedUsersAsync(pageNumber, pageSize))
            .ReturnsAsync((users, totalCount));
        _userRepositoryMock.Setup(r => r.GetRolesAsync(It.IsAny<User>()))
            .ReturnsAsync(new List<string> { "User" });

        _mapperMock.Setup(m => m.Map<List<UserResponseDto>>(It.IsAny<List<User>>()))
            .Returns(userDtos);

        var result = await _userService.GetPaginatedUsersAsync(pageSize, pageNumber);

        result.Should().BeOfType<PagedList<UserResponseDto>>();
        result.Count.Should().Be(5);
        result.TotalCount.Should().Be(totalCount);
    }
    
    [Fact]
    public async Task GetPaginatedUsersAsync_ReturnsPagedListOfUsers_UsersCountLessThanTotal()
    {
        int pageSize = 2;
        int pageNumber = 1;
        var users = _userFaker.Generate(5);
        
        var totalCount = 5;
        var userDtos = users.Select(user => new UserResponseDto { Id = user.Id, Email = user.Email }).ToList();

        _userRepositoryMock.Setup(r => r.GetPagedUsersAsync(pageNumber, pageSize))
            .ReturnsAsync((users.Slice(pageNumber*pageSize - pageSize,pageSize), totalCount));
        _userRepositoryMock.Setup(r => r.GetRolesAsync(It.IsAny<User>()))
            .ReturnsAsync(new List<string> { "User" });

        _mapperMock.Setup(m => m.Map<List<UserResponseDto>>(It.IsAny<List<User>>()))
            .Returns(userDtos);

        var result = await _userService.GetPaginatedUsersAsync(pageSize, pageNumber);

        result.Should().BeOfType<PagedList<UserResponseDto>>();
        result.Count.Should().Be(2);
        result.TotalCount.Should().Be(totalCount);
    }

    [Fact]
    public async Task GetUserByIdAsync_ShouldThrowNotFoundException_WhenUserNotFound()
    {
        var userId = _userFaker.Generate().Id;
        _userRepositoryMock.Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as User);

        Func<Task<UserResponseDto>> act = async () => await _userService.GetUserByIdAsync(userId, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
        _loggerMock.VerifyLog(LogLevel.Error, Times.Once());
    }

    [Fact]
    public async Task GetUserByEmailAsync_ShouldThrowNotFoundException_WhenUserNotFound()
    {
        var email = _userFaker.Generate().Email;
        _userRepositoryMock.Setup(r => r.GetByEmailAsync(email!, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as User);

        Func<Task<UserResponseDto>> act = async () => await _userService.GetUserByEmailAsync(email!, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
        _loggerMock.VerifyLog(LogLevel.Error, Times.Once());
    }
    
    [Fact]
    public async Task GetUserByIdAsync_UserExists_ReturnsUserResponseDto()
    {
        var user = _userFaker.Generate();
        var userResponseDto = new UserResponseDto { Id = user.Id, Email = user.Email! };

        _userRepositoryMock.Setup(x => x.GetByIdAsync(user.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _mapperMock.Setup(x => x.Map<UserResponseDto>(user))
            .Returns(userResponseDto);
        
        var result = await _userService.GetUserByIdAsync(user.Id, CancellationToken.None);

        result.Should().NotBeNull();
        result.Id.Should().BeEquivalentTo(user.Id);
        _mapperMock.Verify(x => x.Map<UserResponseDto>(user), Times.Once);
    }

    [Fact]
    public async Task GetUserByEmailAsync_UserExists_ReturnsUserResponseDto()
    {
        var user =_userFaker.Generate();
        var userResponseDto = new UserResponseDto { Id = user.Id, Email = user.Email! };

        _userRepositoryMock.Setup(x => x.GetByEmailAsync(user.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _mapperMock.Setup(x => x.Map<UserResponseDto>(user))
            .Returns(userResponseDto);
        
        var result = await _userService.GetUserByEmailAsync(user.Id, CancellationToken.None);

        result.Should().NotBeNull();
        result.Id.Should().BeEquivalentTo(user.Id);
        _mapperMock.Verify(x => x.Map<UserResponseDto>(user), Times.Once);
    }
}