using MediatR;
using Profile.Application.DTOs.Profile.Response;

namespace Profile.Application.UseCases.UserUseCases.Commands.Delete;

public sealed record DeleteUserCommand(string UserId) : IRequest<UserResponseDto>
{
    
}