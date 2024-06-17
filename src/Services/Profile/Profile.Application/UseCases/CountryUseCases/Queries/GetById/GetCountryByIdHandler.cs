using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Country.Response;
using Profile.Domain.Repositories;

namespace Profile.Application.UseCases.CountryUseCases.Queries.GetById;

public class GetCountryByIdHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<GetCountryByIdQuery, CountryResponseDto>
{
    public async Task<CountryResponseDto> Handle(GetCountryByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.CountryRepository.FirstOrDefaultAsync(request.CountryId, cancellationToken);
        
        if (result == null)
        {
            //to do exception
        }
        
        return _mapper.Map<CountryResponseDto>(result);
    }
}