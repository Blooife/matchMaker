using MediatR;
using Profile.Application.DTOs.User.Response;

namespace Profile.Application.UseCases.UserUseCases.Commands.Delete;

public sealed record DeleteUserCommand(string UserId) : IRequest<UserResponseDto>;