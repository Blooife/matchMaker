using MediatR;
using Profile.Application.DTOs.City.Request;
using Shared.Models;

namespace Profile.Application.UseCases.CityUseCases.Commands.AddCityToProfile;

public sealed record AddCityToProfileCommand(AddCityToProfileDto Dto) : IRequest<GeneralResponseDto>
{
    
}