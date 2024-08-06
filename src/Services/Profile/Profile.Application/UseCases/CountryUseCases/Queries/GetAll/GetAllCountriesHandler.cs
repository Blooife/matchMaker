using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Country.Response;
using Profile.Domain.Interfaces;

namespace Profile.Application.UseCases.CountryUseCases.Queries.GetAll;

public class GetAllCountriesHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<GetAllCountriesQuery, IEnumerable<CountryResponseDto>>
{
    public async Task<IEnumerable<CountryResponseDto>> Handle(GetAllCountriesQuery request, CancellationToken cancellationToken)
    {
        var countries = await _unitOfWork.CountryRepository.GetAllAsync(cancellationToken);
        
        return _mapper.Map<IEnumerable<CountryResponseDto>>(countries);
    }
}