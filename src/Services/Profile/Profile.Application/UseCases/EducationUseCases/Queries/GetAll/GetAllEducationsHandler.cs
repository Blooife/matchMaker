using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Education.Response;
using Profile.Domain.Interfaces;

namespace Profile.Application.UseCases.EducationUseCases.Queries.GetAll;

public class GetAllEducationsHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<GetAllEducationsQuery, IEnumerable<EducationResponseDto>>
{
    public async Task<IEnumerable<EducationResponseDto>> Handle(GetAllEducationsQuery request, CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.EducationRepository.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<EducationResponseDto>>(result);
    }
}