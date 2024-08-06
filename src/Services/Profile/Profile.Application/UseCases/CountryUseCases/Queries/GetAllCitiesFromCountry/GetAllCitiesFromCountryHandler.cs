using AutoMapper;
using MediatR;
using Profile.Application.DTOs.City.Response;
using Profile.Application.Exceptions;
using Profile.Application.Services.Interfaces;
using Profile.Domain.Interfaces;

namespace Profile.Application.UseCases.CountryUseCases.Queries.GetAllCitiesFromCountry;

public class GetAllCitiesFromCountryHandler(IUnitOfWork _unitOfWork, IMapper _mapper, ICacheService _cacheService) : IRequestHandler<GetAllCitiesFromCountryQuery, IEnumerable<CityResponseDto>>
{
    private readonly string _cacheKeyPrefix = "country";
    
    public async Task<IEnumerable<CityResponseDto>> Handle(GetAllCitiesFromCountryQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"{_cacheKeyPrefix}:{request.CountryId}:cities";
        var cachedData = await _cacheService.GetAsync<IEnumerable<CityResponseDto>>(cacheKey, cancellationToken);
        
        if (cachedData is not null)
        {
            return cachedData;
        }

        var country = await _unitOfWork.CountryRepository.FirstOrDefaultAsync(request.CountryId, cancellationToken);

        if (country is null)
        {
            throw new NotFoundException("Country", request.CountryId);    
        }
        
        var cities = await _unitOfWork.CountryRepository.GetAllCitiesFromCountryAsync(request.CountryId, cancellationToken);
        
        var mappedCities = _mapper.Map<List<CityResponseDto>>(cities);
        await _cacheService.SetAsync(cacheKey, mappedCities, cancellationToken:cancellationToken);
        
        return mappedCities;
    }
}