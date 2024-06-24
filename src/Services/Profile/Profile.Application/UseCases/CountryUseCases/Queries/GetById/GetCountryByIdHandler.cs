using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Country.Response;
using Profile.Application.Exceptions;
using Profile.Domain.Repositories;

namespace Profile.Application.UseCases.CountryUseCases.Queries.GetById;

public class GetCountryByIdHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<GetCountryByIdQuery, CountryResponseDto>
{
    public async Task<CountryResponseDto> Handle(GetCountryByIdQuery request, CancellationToken cancellationToken)
    {
        var country = await _unitOfWork.CountryRepository.FirstOrDefaultAsync(request.CountryId, cancellationToken);
        
        if (country is null)
        {
            throw new NotFoundException("Country", request.CountryId);
        }
        
        return _mapper.Map<CountryResponseDto>(country);
    }
}