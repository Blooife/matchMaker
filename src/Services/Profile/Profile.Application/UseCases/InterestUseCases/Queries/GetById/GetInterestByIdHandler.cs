using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Interest.Response;
using Profile.Application.UseCases.ProfileUseCases.Queries.GetById;
using Profile.Domain.Repositories;

namespace Profile.Application.UseCases.InterestUseCases.Queries.GetById;

public class GetInterestByIdHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<GetInterestByIdQuery, InterestResponseDto>
{
    public async Task<InterestResponseDto> Handle(GetInterestByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.InterestRepository.GetByIdAsync(request.InterestId, cancellationToken);
        if (result == null)
        {
            //to do exception
        }
        return _mapper.Map<InterestResponseDto>(result);
    }
}