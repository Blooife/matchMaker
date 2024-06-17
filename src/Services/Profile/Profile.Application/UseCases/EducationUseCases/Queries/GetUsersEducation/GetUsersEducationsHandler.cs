using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Education.Response;
using Profile.Domain.Repositories;

namespace Profile.Application.UseCases.EducationUseCases.Queries.GetUsersEducation;

public class GetUsersEducationsHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<GetUsersEducationsQuery, IEnumerable<EducationResponseDto>>
{
    public async Task<IEnumerable<EducationResponseDto>> Handle(GetUsersEducationsQuery request, CancellationToken cancellationToken)
    {
        var profile = await _unitOfWork.ProfileRepository.GetByIdAsync(request.ProfileId, cancellationToken);
        if (profile == null)
        {
            //
        }
        
        var education = await _unitOfWork.EducationRepository.GetUsersEducation(profile, cancellationToken);
        return _mapper.Map<IEnumerable<EducationResponseDto>>(education);
    }
}