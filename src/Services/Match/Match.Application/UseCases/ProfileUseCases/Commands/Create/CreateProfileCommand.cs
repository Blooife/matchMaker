using Match.Application.DTOs.Profile.Request;
using Match.Application.DTOs.Profile.Response;
using MediatR;

namespace Match.Application.UseCases.ProfileUseCases.Commands.Create;

public sealed record CreateProfileCommand(CreateProfileDto Dto) : IRequest<ProfileResponseDto>
{
    
}