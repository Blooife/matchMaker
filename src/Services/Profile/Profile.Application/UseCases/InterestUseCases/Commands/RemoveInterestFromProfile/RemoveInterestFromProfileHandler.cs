using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Interest.Response;
using Profile.Application.DTOs.Profile.Response;
using Profile.Application.Exceptions;
using Profile.Application.Exceptions.Messages;
using Profile.Application.Services.Interfaces;

using Profile.Domain.Interfaces;
using Profile.Domain.Models;
using Profile.Domain.Specifications.ProfileSpecifications;

namespace Profile.Application.UseCases.InterestUseCases.Commands.RemoveInterestFromProfile;

public class RemoveInterestFromProfileHandler(IUnitOfWork _unitOfWork, IMapper _mapper, ICacheService _cacheService) : IRequestHandler<RemoveInterestFromProfileCommand, List<InterestResponseDto>>
{
    private readonly string _cacheKeyPrefix = "profile";
    
    public async Task<List<InterestResponseDto>> Handle(RemoveInterestFromProfileCommand request, CancellationToken cancellationToken)
    {
        var cacheKey = $"{_cacheKeyPrefix}:{request.Dto.ProfileId}";
        var profileResponseDto = await _cacheService.GetAsync(cacheKey, async () =>
        {
            var profile = await _unitOfWork.ProfileRepository.GetAllProfileInfoAsync(userProfile => userProfile.Id == request.Dto.ProfileId, cancellationToken);
            
            return _mapper.Map<ProfileResponseDto>(profile);
        }, cancellationToken);

        var profile = _mapper.Map<UserProfile>(profileResponseDto);
        
        if (profile is null)
        {
            throw new NotFoundException("Profile", request.Dto.ProfileId);
        }
        
        var interest = await _unitOfWork.InterestRepository.FirstOrDefaultAsync(request.Dto.InterestId, cancellationToken);
        
        if (interest is null)
        {
            throw new NotFoundException("Interest", request.Dto.InterestId);
        }

        var isProfileContainsInterest = profile.ContainsInterest(request.Dto.InterestId);

        if (!isProfileContainsInterest)
        {
            throw new NotContainsException(ExceptionMessages.ProfileNotContainsInterest);
        }
        
        var interestToRemove = profile.Interests.First(i=>i.Id == request.Dto.InterestId);
        await _unitOfWork.InterestRepository.RemoveInterestFromProfileAsync(profile, interestToRemove, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
        
        await _cacheService.SetAsync(cacheKey, _mapper.Map<ProfileResponseDto>(profile),
            cancellationToken: cancellationToken);
        
        return _mapper.Map<List<InterestResponseDto>>(profile.Interests);
    }
}