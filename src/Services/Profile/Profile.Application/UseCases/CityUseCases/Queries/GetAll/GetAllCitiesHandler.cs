using AutoMapper;
using MediatR;
using Profile.Application.DTOs.City.Response;
using Profile.Domain.Interfaces.Repositories;
using Profile.Domain.Interfaces.Services;

namespace Profile.Application.UseCases.CityUseCases.Queries.GetAll;

public class GetAllCitiesHandler(IUnitOfWork _unitOfWork, IMapper _mapper, ICacheService _cacheService) : IRequestHandler<GetAllCitiesQuery, IEnumerable<CityResponseDto>>
{
    private readonly string _cacheKeyPrefix = "cities";
    
    public async Task<IEnumerable<CityResponseDto>> Handle(GetAllCitiesQuery request, CancellationToken cancellationToken)
    {
        var cachedData = await _cacheService.GetAsync<IEnumerable<CityResponseDto>>(_cacheKeyPrefix, cancellationToken);
        
        if (cachedData is not null)
        {
            return cachedData;
        }
        
        var cities = await _unitOfWork.CityRepository.GetAllAsync(cancellationToken);
        
        var mappedCities = _mapper.Map<List<CityResponseDto>>(cities);
        
        await _cacheService.SetAsync(_cacheKeyPrefix, mappedCities, cancellationToken:cancellationToken);
        
        return mappedCities;
    }
}