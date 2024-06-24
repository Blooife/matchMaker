using AutoMapper;
using MediatR;
using Profile.Application.DTOs.City.Response;
using Profile.Application.Exceptions;
using Profile.Domain.Repositories;

namespace Profile.Application.UseCases.CountryUseCases.Queries.GetAllCitiesFromCountry;

public class GetAllCitiesFromCountryHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<GetAllCitiesFromCountryQuery, IEnumerable<CityResponseDto>>
{
    public async Task<IEnumerable<CityResponseDto>> Handle(GetAllCitiesFromCountryQuery request, CancellationToken cancellationToken)
    {
        var country = await _unitOfWork.CountryRepository.FirstOrDefaultAsync(request.CountryId, cancellationToken);

        if (country is null)
        {
            throw new NotFoundException("Country", request.CountryId);    
        }
        
        var cities = await _unitOfWork.CountryRepository.GetAllCitiesFromCountryAsync(request.CountryId, cancellationToken);
        
        return _mapper.Map<IEnumerable<CityResponseDto>>(cities);
    }
}