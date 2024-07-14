using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Education.Response;
using Profile.Application.Exceptions;
using Profile.Domain.Interfaces;

namespace Profile.Application.UseCases.EducationUseCases.Queries.GetById;

public class GetEducationByIdHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<GetEducationByIdQuery, EducationResponseDto>
{
    public async Task<EducationResponseDto> Handle(GetEducationByIdQuery request, CancellationToken cancellationToken)
    {
        var education = await _unitOfWork.EducationRepository.FirstOrDefaultAsync(request.EducationId, cancellationToken);
        
        if (education == null)
        {
            throw new NotFoundException("Education", request.EducationId);
        }
        
        return _mapper.Map<EducationResponseDto>(education);
    }
}