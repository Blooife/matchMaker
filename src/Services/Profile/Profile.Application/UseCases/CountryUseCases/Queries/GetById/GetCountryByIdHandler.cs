using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Country.Response;
using Profile.Application.Exceptions;
using Profile.Application.Services.Interfaces;
using Profile.Domain.Repositories;

namespace Profile.Application.UseCases.CountryUseCases.Queries.GetById;

public class GetCountryByIdHandler(IUnitOfWork _unitOfWork, IMapper _mapper, ICacheService _cacheService) : IRequestHandler<GetCountryByIdQuery, CountryResponseDto>
{
    private readonly string _cacheKeyPrefix = "country";
    
    public async Task<CountryResponseDto> Handle(GetCountryByIdQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"{_cacheKeyPrefix}:{request.CountryId}";
        var cachedData = await _cacheService.GetAsync<CountryResponseDto>(cacheKey, cancellationToken);
        
        if (cachedData is not null)
        {
            return cachedData;
        }
        
        var country = await _unitOfWork.CountryRepository.FirstOrDefaultAsync(request.CountryId, cancellationToken);
        
        if (country is null)
        {
            throw new NotFoundException("Country", request.CountryId);
        }
        
        var mappedCountry = _mapper.Map<CountryResponseDto>(country);
        
        if (mappedCountry is not null)
        {
            await _cacheService.SetAsync(cacheKey, mappedCountry, cancellationToken:cancellationToken);
        }

        return mappedCountry;
    }
}