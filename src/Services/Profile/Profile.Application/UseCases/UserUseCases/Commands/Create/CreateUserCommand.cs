using MediatR;
using Profile.Application.DTOs.User.Request;
using Profile.Application.DTOs.User.Response;

namespace Profile.Application.UseCases.UserUseCases.Commands.Create;

public sealed record CreateUserCommand(CreateUserDto CreateUserDto) : IRequest<UserResponseDto>;