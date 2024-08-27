using AutoMapper;
using FluentAssertions;
using Moq;
using Profile.Application.DTOs.User.Response;
using Profile.Application.Exceptions;
using Profile.Application.UseCases.UserUseCases.Commands.Create;
using Profile.Domain.Interfaces.Repositories;
using Profile.Domain.Models;
using Profile.Tests.Fakers;

namespace Profile.Tests.UnitTests.CommandsTests.UsersTests;

public class CreateUserHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly CreateUserHandler _handler;

    public CreateUserHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _handler = new CreateUserHandler(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldCreateUserAndReturnUserResponseDto()
    {
        var createUserDto = UserFakers.CreateCreateUserDto().Generate();
        var user = UserFakers.CreateUser().Generate();
        var userResponseDto = new UserResponseDto();

        _unitOfWorkMock.Setup(u => u.UserRepository.FirstOrDefaultAsync(createUserDto.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User)null!);

        _mapperMock.Setup(m => m.Map<User>(createUserDto))
            .Returns(user);

        _unitOfWorkMock.Setup(u => u.UserRepository.CreateUserAsync(user, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _unitOfWorkMock.Setup(u => u.SaveAsync(It.IsAny<CancellationToken>()));

        _mapperMock.Setup(m => m.Map<UserResponseDto>(user))
            .Returns(userResponseDto);

        var result = await _handler.Handle(new CreateUserCommand(createUserDto), CancellationToken.None);

        result.Should().BeEquivalentTo(userResponseDto);

        _unitOfWorkMock.Verify(u => u.UserRepository.FirstOrDefaultAsync(createUserDto.Id, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.UserRepository.CreateUserAsync(user, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        _mapperMock.Verify(m => m.Map<UserResponseDto>(user), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowAlreadyExistsException_WhenUserAlreadyExists()
    {
        var createUserDto = UserFakers.CreateCreateUserDto().Generate();
        var existingUser = UserFakers.CreateUser().Generate();

        _unitOfWorkMock.Setup(u => u.UserRepository.FirstOrDefaultAsync(createUserDto.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingUser);

        var act = async () => await _handler.Handle(new CreateUserCommand(createUserDto), CancellationToken.None);

        await act.Should().ThrowAsync<AlreadyExistsException>();

        _unitOfWorkMock.Verify(u => u.UserRepository.FirstOrDefaultAsync(createUserDto.Id, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.UserRepository.CreateUserAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveAsync(It.IsAny<CancellationToken>()), Times.Never);
        _mapperMock.Verify(m => m.Map<User>(createUserDto), Times.Never);
    }
}