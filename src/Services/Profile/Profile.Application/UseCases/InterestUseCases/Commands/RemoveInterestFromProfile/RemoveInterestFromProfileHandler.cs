using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Interest.Response;
using Profile.Application.Exceptions;
using Profile.Application.Exceptions.Messages;
using Profile.Application.Services.Interfaces;
using Profile.Domain.Repositories;
using Profile.Domain.Specifications.ProfileSpecifications;

namespace Profile.Application.UseCases.InterestUseCases.Commands.RemoveInterestFromProfile;

public class RemoveInterestFromProfileHandler(IUnitOfWork _unitOfWork, IMapper _mapper, ICacheService _cacheService) : IRequestHandler<RemoveInterestFromProfileCommand, List<InterestResponseDto>>
{
    private readonly string _cacheKeyPrefix = "interest";
    
    public async Task<List<InterestResponseDto>> Handle(RemoveInterestFromProfileCommand request, CancellationToken cancellationToken)
    {
        var profileWithInterests = await _unitOfWork.InterestRepository.GetProfileWithInterestsAsync(request.Dto.ProfileId, cancellationToken);
        
        if (profileWithInterests is null)
        {
            throw new NotFoundException("Profile", request.Dto.ProfileId);
        }
        
        var interest = await _unitOfWork.InterestRepository.FirstOrDefaultAsync(request.Dto.InterestId, cancellationToken);
        
        if (interest is null)
        {
            throw new NotFoundException("Interest", request.Dto.InterestId);
        }

        var isProfileContainsInterest = profileWithInterests.ContainsInterest(request.Dto.InterestId);

        if (!isProfileContainsInterest)
        {
            throw new NotContainsException(ExceptionMessages.ProfileNotContainsInterest);
        }
        
        var interestToRemove = profileWithInterests.Interests.First(i=>i.Id == request.Dto.InterestId);
        await _unitOfWork.InterestRepository.RemoveInterestFromProfileAsync(profileWithInterests, interestToRemove, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
        
        var cacheKey = $"{_cacheKeyPrefix}:profile:{request.Dto.ProfileId}";
        var mappedInterests= _mapper.Map<List<InterestResponseDto>>(profileWithInterests.Interests);
        await _cacheService.SetAsync(cacheKey, mappedInterests, cancellationToken: cancellationToken);
        
        return mappedInterests;
    }
}