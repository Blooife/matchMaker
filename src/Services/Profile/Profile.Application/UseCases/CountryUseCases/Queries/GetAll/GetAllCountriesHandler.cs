using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Country.Response;
using Profile.Domain.Interfaces.Repositories;
using Profile.Domain.Interfaces.Services;

namespace Profile.Application.UseCases.CountryUseCases.Queries.GetAll;

public class GetAllCountriesHandler(IUnitOfWork _unitOfWork, IMapper _mapper, ICacheService _cacheService) : IRequestHandler<GetAllCountriesQuery, IEnumerable<CountryResponseDto>>
{
    private readonly string _cacheKeyPrefix = "countries";
    
    public async Task<IEnumerable<CountryResponseDto>> Handle(GetAllCountriesQuery request, CancellationToken cancellationToken)
    {
        var cachedData = await _cacheService.GetAsync<IEnumerable<CountryResponseDto>>(_cacheKeyPrefix, cancellationToken);
        
        if (cachedData is not null)
        {
            return cachedData;
        }
        
        var countries = await _unitOfWork.CountryRepository.GetAllAsync(cancellationToken);
        
        var mappedCountries = _mapper.Map<List<CountryResponseDto>>(countries);
        
        await _cacheService.SetAsync(_cacheKeyPrefix, mappedCountries, cancellationToken:cancellationToken);
        
        return mappedCountries;
    }
}