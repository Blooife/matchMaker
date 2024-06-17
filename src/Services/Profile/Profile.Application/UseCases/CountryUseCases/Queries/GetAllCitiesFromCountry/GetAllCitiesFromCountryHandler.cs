using AutoMapper;
using MediatR;
using Profile.Application.DTOs.City.Response;
using Profile.Domain.Repositories;

namespace Profile.Application.UseCases.CountryUseCases.Queries.GetAllCitiesFromCountry;

public class GetAllCitiesFromCountryHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<GetAllCitiesFromCountryQuery, IEnumerable<CityResponseDto>>
{
    public async Task<IEnumerable<CityResponseDto>> Handle(GetAllCitiesFromCountryQuery request, CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.CountryRepository.GetAllCitiesFromCountryAsync(request.CountryId, cancellationToken);
        
        return _mapper.Map<IEnumerable<CityResponseDto>>(result);
    }
}