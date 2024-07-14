using AutoMapper;
using MediatR;
using Profile.Application.DTOs.City.Response;
using Profile.Domain.Interfaces;

namespace Profile.Application.UseCases.CityUseCases.Queries.GetAll;

public class GetAllCitysHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<GetAllCitiesQuery, IEnumerable<CityResponseDto>>
{
    public async Task<IEnumerable<CityResponseDto>> Handle(GetAllCitiesQuery request, CancellationToken cancellationToken)
    {
        var cities = await _unitOfWork.CityRepository.GetAllAsync(cancellationToken);
        
        return _mapper.Map<IEnumerable<CityResponseDto>>(cities);
    }
}