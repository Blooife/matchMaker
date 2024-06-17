using AutoMapper;
using MediatR;
using Profile.Application.DTOs.City.Response;
using Profile.Domain.Repositories;

namespace Profile.Application.UseCases.CityUseCases.Queries.GetCityWithCountryById;

public class GetCityWithCountryByIdHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<GetCityWithCountryByIdQuery, CityWithCountryResponseDto>
{
    public async Task<CityWithCountryResponseDto> Handle(GetCityWithCountryByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.CityRepository.GetCityWithCountryById(request.CityId, cancellationToken);
        if (result == null)
        {
            //to do exception
        }
        
        var city =  _mapper.Map<CityWithCountryResponseDto>(result);
        var country = await _unitOfWork.CountryRepository.FirstOrDefaultAsync(city.CountryId, cancellationToken);
        city.CountryName = country.Name;

        return city;
    }
}