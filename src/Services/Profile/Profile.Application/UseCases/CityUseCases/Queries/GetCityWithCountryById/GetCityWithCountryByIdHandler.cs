using AutoMapper;
using MediatR;
using Profile.Application.DTOs.City.Response;
using Profile.Application.Exceptions;
using Profile.Application.Services.Interfaces;
using Profile.Domain.Repositories;

namespace Profile.Application.UseCases.CityUseCases.Queries.GetCityWithCountryById;

public class GetCityWithCountryByIdHandler(IUnitOfWork _unitOfWork, IMapper _mapper, ICacheService _cacheService) : IRequestHandler<GetCityWithCountryByIdQuery, CityWithCountryResponseDto>
{
    private readonly string _cacheKeyPrefix = "city";
    
    public async Task<CityWithCountryResponseDto> Handle(GetCityWithCountryByIdQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"{_cacheKeyPrefix}:{request.CityId}:withCountry";
        var cachedData = await _cacheService.GetAsync<CityWithCountryResponseDto>(cacheKey, cancellationToken);
        
        if (cachedData is not null)
        {
            return cachedData;
        }

        var city = await _unitOfWork.CityRepository.GetCityWithCountryById(request.CityId, cancellationToken);
        
        if (city is null)
        {
            throw new NotFoundException("City", request.CityId);
        }
        
        var mappedCity = _mapper.Map<CityWithCountryResponseDto>(city);
        await _cacheService.SetAsync(cacheKey, mappedCity, cancellationToken:cancellationToken);
        
        return mappedCity;
    }
}