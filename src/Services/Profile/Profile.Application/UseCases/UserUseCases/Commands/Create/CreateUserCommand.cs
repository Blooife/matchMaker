using MediatR;
using Profile.Application.DTOs.Profile.Response;
using Profile.Application.DTOs.User.Request;

namespace Profile.Application.UseCases.UserUseCases.Commands.Create;

public sealed record CreateUserCommand(CreateUserDto CreateUserDto) : IRequest<UserResponseDto>
{
    
}