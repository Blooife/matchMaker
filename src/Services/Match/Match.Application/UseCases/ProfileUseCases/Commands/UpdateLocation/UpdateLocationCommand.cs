using Match.Application.DTOs.Profile.Request;
using Match.Application.DTOs.Profile.Response;
using MediatR;

namespace Match.Application.UseCases.ProfileUseCases.Commands.UpdateLocation;

public sealed record UpdateLocationCommand(UpdateLocationDto Dto) : IRequest<ProfileResponseDto>
{
    
}