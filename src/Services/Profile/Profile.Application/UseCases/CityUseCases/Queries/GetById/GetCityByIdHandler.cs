using AutoMapper;
using MediatR;
using Profile.Application.DTOs.City.Response;
using Profile.Domain.Repositories;

namespace Profile.Application.UseCases.CityUseCases.Queries.GetById;

public class GetCityByIdHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<GetCityByIdQuery, CityResponseDto>
{
    public async Task<CityResponseDto> Handle(GetCityByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.CityRepository.FirstOrDefaultAsync(request.CityId, cancellationToken);
        if (result == null)
        {
            //to do exception
        }
        
        return _mapper.Map<CityResponseDto>(result);
    }
}