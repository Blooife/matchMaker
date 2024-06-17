using MediatR;
using Profile.Application.DTOs.City.Request;
using Shared.Models;

namespace Profile.Application.UseCases.CityUseCases.Commands.RemoveCityFromProfile;

public sealed record RemoveCityFromProfileCommand(RemoveCityFromProfileDto Dto) : IRequest<GeneralResponseDto>
{
    
}