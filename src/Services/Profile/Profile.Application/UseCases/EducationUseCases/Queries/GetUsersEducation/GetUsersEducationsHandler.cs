using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Education.Response;
using Profile.Application.Exceptions;
using Profile.Domain.Repositories;

namespace Profile.Application.UseCases.EducationUseCases.Queries.GetUsersEducation;

public class GetUsersEducationsHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<GetUsersEducationsQuery, IEnumerable<UserEducationResponseDto>>
{
    public async Task<IEnumerable<UserEducationResponseDto>> Handle(GetUsersEducationsQuery request, CancellationToken cancellationToken)
    {
        var profile = await _unitOfWork.ProfileRepository.GetByIdAsync(request.ProfileId, cancellationToken);
        
        if (profile is null)
        {
            throw new NotFoundException("Profile", request.ProfileId);
        }
        
        var education = await _unitOfWork.EducationRepository.GetUsersEducation(profile, cancellationToken);
        
        return _mapper.Map<IEnumerable<UserEducationResponseDto>>(education);
    }
}