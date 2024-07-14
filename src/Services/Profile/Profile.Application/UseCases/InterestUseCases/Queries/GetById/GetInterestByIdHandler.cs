using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Interest.Response;
using Profile.Application.Exceptions;
using Profile.Domain.Interfaces;

namespace Profile.Application.UseCases.InterestUseCases.Queries.GetById;

public class GetInterestByIdHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<GetInterestByIdQuery, InterestResponseDto>
{
    public async Task<InterestResponseDto> Handle(GetInterestByIdQuery request, CancellationToken cancellationToken)
    {
        var interest = await _unitOfWork.InterestRepository.FirstOrDefaultAsync(request.InterestId, cancellationToken);
        
        if (interest == null)
        {
            throw new NotFoundException("Interest", request.InterestId);
        }
        
        return _mapper.Map<InterestResponseDto>(interest);
    }
}