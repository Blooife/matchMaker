using AutoMapper;
using MediatR;
using Profile.Application.DTOs.City.Response;
using Profile.Application.Exceptions;
using Profile.Domain.Interfaces;

namespace Profile.Application.UseCases.CityUseCases.Queries.GetCityWithCountryById;

public class GetCityWithCountryByIdHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<GetCityWithCountryByIdQuery, CityWithCountryResponseDto>
{
    public async Task<CityWithCountryResponseDto> Handle(GetCityWithCountryByIdQuery request, CancellationToken cancellationToken)
    {
        var city = await _unitOfWork.CityRepository.GetCityWithCountryById(request.CityId, cancellationToken);
        
        if (city is null)
        {
            throw new NotFoundException("City", request.CityId);
        }
        
        var mappedCity =  _mapper.Map<CityWithCountryResponseDto>(city);

        return mappedCity;
    }
}