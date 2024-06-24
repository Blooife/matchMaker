using MediatR;
using Profile.Application.DTOs.Profile.Response;
using Profile.Application.DTOs.User.Request;

namespace Profile.Application.UseCases.UserUseCases.Commands.Update;

public sealed record UpdateUserCommand(UpdateUserDto UpdateUserDto) : IRequest<UserResponseDto>
{
    
}