using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Education.Response;
using Profile.Application.Exceptions;
using Profile.Domain.Repositories;

namespace Profile.Application.UseCases.EducationUseCases.Queries.GetProfilesEducation;

public class GetProfilesEducationsHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<GetProfilesEducationsQuery, IEnumerable<ProfileEducationResponseDto>>
{
    public async Task<IEnumerable<ProfileEducationResponseDto>> Handle(GetProfilesEducationsQuery request, CancellationToken cancellationToken)
    {
        var profile = await _unitOfWork.ProfileRepository.FirstOrDefaultAsync(request.ProfileId, cancellationToken);
        
        if (profile is null)
        {
            throw new NotFoundException("Profile", request.ProfileId);
        }
        
        var education = await _unitOfWork.EducationRepository.GetProfilesEducation(profile, cancellationToken);
        
        return _mapper.Map<IEnumerable<ProfileEducationResponseDto>>(education);
    }
}