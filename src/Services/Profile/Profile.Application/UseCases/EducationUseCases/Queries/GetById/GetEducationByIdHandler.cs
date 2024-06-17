using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Education.Response;
using Profile.Application.UseCases.ProfileUseCases.Queries.GetById;
using Profile.Domain.Repositories;

namespace Profile.Application.UseCases.EducationUseCases.Queries.GetById;

public class GetEducationByIdHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<GetEducationByIdQuery, EducationResponseDto>
{
    public async Task<EducationResponseDto> Handle(GetEducationByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.EducationRepository.FirstOrDefaultAsync(request.EducationId, cancellationToken);
        if (result == null)
        {
            //to do exception
        }
        return _mapper.Map<EducationResponseDto>(result);
    }
}