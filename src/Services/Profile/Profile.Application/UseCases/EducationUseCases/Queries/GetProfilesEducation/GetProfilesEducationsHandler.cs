using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Education.Response;
using Profile.Application.Exceptions;
using Profile.Application.Services.Interfaces;
using Profile.Domain.Repositories;

namespace Profile.Application.UseCases.EducationUseCases.Queries.GetProfilesEducation;

public class GetProfilesEducationsHandler(IUnitOfWork _unitOfWork, IMapper _mapper, ICacheService _cacheService) : IRequestHandler<GetProfilesEducationsQuery, IEnumerable<ProfileEducationResponseDto>>
{
    private readonly string _cacheKeyPrefix = "education";
    
    public async Task<IEnumerable<ProfileEducationResponseDto>> Handle(GetProfilesEducationsQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"{_cacheKeyPrefix}:profile:{request.ProfileId}";
        var cachedData = await _cacheService.GetAsync<List<ProfileEducationResponseDto>>(cacheKey, cancellationToken);
        
        if (cachedData is not null)
        {
            return cachedData;
        }
        
        var profile = await _unitOfWork.ProfileRepository.FirstOrDefaultAsync(request.ProfileId, cancellationToken);
        
        if (profile is null)
        {
            throw new NotFoundException("Profile", request.ProfileId);
        }
        
        var education = await _unitOfWork.EducationRepository.GetProfilesEducation(profile, cancellationToken);
        
        var mappedEducation = _mapper.Map<List<ProfileEducationResponseDto>>(education);
        await _cacheService.SetAsync(cacheKey, mappedEducation, cancellationToken: cancellationToken);
        
        return mappedEducation;
    }
}