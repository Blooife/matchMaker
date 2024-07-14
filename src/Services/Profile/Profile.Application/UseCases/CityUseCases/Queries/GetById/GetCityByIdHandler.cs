using AutoMapper;
using MediatR;
using Profile.Application.DTOs.City.Response;
using Profile.Application.Exceptions;
using Profile.Domain.Interfaces;

namespace Profile.Application.UseCases.CityUseCases.Queries.GetById;

public class GetCityByIdHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<GetCityByIdQuery, CityResponseDto>
{
    public async Task<CityResponseDto> Handle(GetCityByIdQuery request, CancellationToken cancellationToken)
    {
        var city = await _unitOfWork.CityRepository.FirstOrDefaultAsync(request.CityId, cancellationToken);
        
        if (city is null)
        {
            throw new NotFoundException("City", request.CityId);
        }
        
        return _mapper.Map<CityResponseDto>(city);
    }
}