using MediatR;
using Profile.Application.DTOs.Profile.Response;

namespace Profile.Application.UseCases.ProfileUseCases.Commands.Delete;

public sealed record DeleteProfileCommand(string ProfileId) : IRequest<ProfileResponseDto>
{
    
}