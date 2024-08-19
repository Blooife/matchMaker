using Moq;
using AutoMapper;
using Microsoft.Extensions.Logging;
using FluentAssertions;
using Authentication.BusinessLogic.DTOs.Response;
using Authentication.BusinessLogic.Exceptions;
using Authentication.BusinessLogic.Services.Implementations;
using Authentication.DataLayer.Models;
using Authentication.DataLayer.Repositories.Interfaces;
using Authentication.Tests.UnitTests;
using Authentication.Tests.Fakers;
using Bogus;
using Microsoft.AspNetCore.Identity;
using Shared.Models;

public class RoleServiceTests
{
    private readonly Mock<IRoleRepository> _roleRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ILogger<RoleService>> _loggerMock;
    private readonly RoleService _roleService;
    private readonly Faker<User> _userFaker = TestDataGenerator.CreateUser();
    private readonly Faker<RoleResponseDto> _roleResponseDtoFaker = TestDataGenerator.CreateRoleResponseDto();

    public RoleServiceTests()
    {
        _roleRepositoryMock = new Mock<IRoleRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILogger<RoleService>>();
        _roleService = new RoleService(_roleRepositoryMock.Object, _userRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetAllRolesAsync_ReturnsRoles_WhenRolesExist()
    {
        var roles = new List<Role> ();
        var roleResponseDtos = _roleResponseDtoFaker.Generate(2);

        _roleRepositoryMock.Setup(r => r.GetAllRolesAsync(It.IsAny<CancellationToken>()))
                           .ReturnsAsync(roles);
        _mapperMock.Setup(m => m.Map<IEnumerable<RoleResponseDto>>(roles))
                   .Returns(roleResponseDtos);

        var result = await _roleService.GetAllRolesAsync(CancellationToken.None);

        result.Should().BeEquivalentTo(roleResponseDtos);
    }

    [Fact]
    public async Task AssignRoleAsync_ReturnsSuccess_WhenRoleAssignedSuccessfully()
    {
        var roleName = _roleResponseDtoFaker.Generate().Name;
        var user = _userFaker.Generate();
        var result = IdentityResult.Success;
        
        var expectedResponse = new GeneralResponseDto { Message = "Role assigned successfully" };

        _userRepositoryMock.Setup(r => r.GetByEmailAsync(user.Email!, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(user);
        _roleRepositoryMock.Setup(r => r.RoleExistsAsync(roleName))
                           .ReturnsAsync(true);
        _userRepositoryMock.Setup(r => r.AddToRoleAsync(user, roleName))
                           .ReturnsAsync(result);

        var response = await _roleService.AssignRoleAsync(user.Email!, roleName, CancellationToken.None);

        response.Should().BeEquivalentTo(expectedResponse);
    }

    [Fact]
    public async Task AssignRoleAsync_ShouldThrowNotFoundException_WhenUserNotFound()
    {
        var email = _userFaker.Generate().Email!;
        var roleName = _roleResponseDtoFaker.Generate().Name;

        _userRepositoryMock.Setup(r => r.GetByEmailAsync(email, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(null as User);

        Func<Task> act = async () => await _roleService.AssignRoleAsync(email, roleName, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
        _loggerMock.VerifyLog(LogLevel.Error, Times.Once());
    }

    [Fact]
    public async Task AssignRoleAsync_ShouldThrowAssignRoleException_WhenRoleDoesNotExist()
    {
        var email = _userFaker.Generate().Email!;
        var roleName = _roleResponseDtoFaker.Generate().Name;
        var user = _userFaker.Generate();

        _userRepositoryMock.Setup(r => r.GetByEmailAsync(email, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(user);
        _roleRepositoryMock.Setup(r => r.RoleExistsAsync(roleName))
                           .ReturnsAsync(false);

        Func<Task> act = async () => await _roleService.AssignRoleAsync(email, roleName, CancellationToken.None);

        await act.Should().ThrowAsync<AssignRoleException>()
                 .WithMessage(ExceptionMessages.RoleNotExists);
        _loggerMock.VerifyLog(LogLevel.Error, Times.Once());
    }
    
    [Fact]
    public async Task AssignRoleAsync_ShouldThrowAssignRoleException_WhenAssigningRoleFails()
    {
        var email = _userFaker.Generate().Email!;
        var roleName = _roleResponseDtoFaker.Generate().Name;
        var user = _userFaker.Generate();
        var result = IdentityResult.Failed(new IdentityError());


        _userRepositoryMock.Setup(r => r.GetByEmailAsync(email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _roleRepositoryMock.Setup(r => r.RoleExistsAsync(roleName))
            .ReturnsAsync(true);
        _userRepositoryMock.Setup(r => r.AddToRoleAsync(user, roleName))
            .ReturnsAsync(result);
        
        Func<Task> act = async () => await _roleService.AssignRoleAsync(email, roleName, CancellationToken.None);

        await act.Should().ThrowAsync<AssignRoleException>();
        _loggerMock.VerifyLog(LogLevel.Error, Times.Once());
    }

    [Fact]
    public async Task RemoveUserFromRoleAsync_ReturnsSuccess_WhenRoleRemovedSuccessfully()
    {
        var email = _userFaker.Generate().Email!;
        var roleName = _roleResponseDtoFaker.Generate().Name;
        var user = _userFaker.Generate();
        var roles = new List<string> { "Admin", "User" };
        var result = IdentityResult.Success;
        var expectedResponse = new GeneralResponseDto { Message = "Role removed successfully" };

        _userRepositoryMock.Setup(r => r.GetByEmailAsync(email, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(user);
        _userRepositoryMock.Setup(r => r.GetRolesAsync(user))
                           .ReturnsAsync(roles);
        _userRepositoryMock.Setup(r => r.RemoveFromRoleAsync(user, roleName))
                           .ReturnsAsync(result);
        _roleRepositoryMock.Setup(r => r.RoleExistsAsync(roleName))
            .ReturnsAsync(true);

        var response = await _roleService.RemoveUserFromRoleAsync(email, roleName, CancellationToken.None);

        response.Should().BeEquivalentTo(expectedResponse);
        _loggerMock.VerifyLog(LogLevel.Error, Times.Never());
    }

    [Fact]
    public async Task RemoveUserFromRoleAsync_ShouldThrowNotFoundException_WhenUserNotFound()
    {
        var email = _userFaker.Generate().Email!;
        var roleName = _roleResponseDtoFaker.Generate().Name;

        _userRepositoryMock.Setup(r => r.GetByEmailAsync(email, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(null as User);

        Func<Task> act = async () => await _roleService.RemoveUserFromRoleAsync(email, roleName, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
        _loggerMock.VerifyLog(LogLevel.Error, Times.Once());
    }

    [Fact]
    public async Task RemoveUserFromRoleAsync_ShouldThrowRemoveRoleException_WhenUserHasOnlyOneRole()
    {
        var email = _userFaker.Generate().Email!;
        var roleName = _roleResponseDtoFaker.Generate().Name;
        var user = _userFaker.Generate();
        var roles = new List<string> { "Admin" };

        _userRepositoryMock.Setup(r => r.GetByEmailAsync(email, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(user);
        _userRepositoryMock.Setup(r => r.GetRolesAsync(user))
                           .ReturnsAsync(roles);

        Func<Task> act = async () => await _roleService.RemoveUserFromRoleAsync(email, roleName, CancellationToken.None);

        await act.Should().ThrowAsync<RemoveRoleException>();
        _loggerMock.VerifyLog(LogLevel.Error, Times.Once());
    }

    [Fact]
    public async Task RemoveUserFromRoleAsync_ShouldThrowRemoveRoleException_WhenRemovingRoleFails()
    {
        var email = _userFaker.Generate().Email!;
        var roleName = _roleResponseDtoFaker.Generate().Name;
        var user = _userFaker.Generate();
        var roles = new List<string> { "Admin", "User" };
        var result = IdentityResult.Failed();
        
        _userRepositoryMock.Setup(r => r.GetByEmailAsync(email, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(user);
        _userRepositoryMock.Setup(r => r.GetRolesAsync(user))
                           .ReturnsAsync(roles);
        _userRepositoryMock.Setup(r => r.RemoveFromRoleAsync(user, roleName))
                           .ReturnsAsync(result);
        
        Func<Task> act = async () => await _roleService.RemoveUserFromRoleAsync(email, roleName, CancellationToken.None);

        await act.Should().ThrowAsync<RemoveRoleException>();
        _loggerMock.VerifyLog(LogLevel.Error, Times.Once());
    }
}