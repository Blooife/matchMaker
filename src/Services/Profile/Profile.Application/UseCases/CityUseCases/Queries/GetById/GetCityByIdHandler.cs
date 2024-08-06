using AutoMapper;
using MediatR;
using Profile.Application.DTOs.City.Response;
using Profile.Application.Exceptions;
using Profile.Application.Services.Interfaces;
using Profile.Domain.Interfaces;

namespace Profile.Application.UseCases.CityUseCases.Queries.GetById;

public class GetCityByIdHandler(IUnitOfWork _unitOfWork, IMapper _mapper, ICacheService _cacheService) : IRequestHandler<GetCityByIdQuery, CityResponseDto>
{
    private readonly string _cacheKeyPrefix = "city";
    public async Task<CityResponseDto> Handle(GetCityByIdQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"{_cacheKeyPrefix}:{request.CityId}";
        var cachedData = await _cacheService.GetAsync<CityResponseDto>(cacheKey, cancellationToken);
        
        if (cachedData is not null)
        {
            return cachedData;
        }
        
        var city = await _unitOfWork.CityRepository.FirstOrDefaultAsync(request.CityId, cancellationToken);
        
        if (city is null)
        {
            throw new NotFoundException("City", request.CityId);
        }
        
        var mappedCity = _mapper.Map<CityResponseDto>(city);
        await _cacheService.SetAsync(cacheKey, mappedCity, cancellationToken:cancellationToken);
        
        return mappedCity;
    }
}