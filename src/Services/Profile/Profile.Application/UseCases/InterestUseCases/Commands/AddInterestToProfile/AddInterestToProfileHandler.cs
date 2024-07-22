using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Interest.Response;
using Profile.Application.Exceptions;
using Profile.Application.Exceptions.Messages;
using Profile.Application.Services.Interfaces;
using Profile.Domain.Repositories;
using Profile.Domain.Specifications.ProfileSpecifications;

namespace Profile.Application.UseCases.InterestUseCases.Commands.AddInterestToProfile;

public class AddInterestToProfileHandler(IUnitOfWork _unitOfWork, IMapper _mapper, ICacheService _cacheService) : IRequestHandler<AddInterestToProfileCommand, List<InterestResponseDto>>
{
    private readonly string _cacheKeyPrefix = "interest";
    
    public async Task<List<InterestResponseDto>> Handle(AddInterestToProfileCommand request, CancellationToken cancellationToken)
    {
        var profileWithInterests = await _unitOfWork.InterestRepository.GetProfileWithInterests(request.Dto.ProfileId, cancellationToken);
        
        if (profileWithInterests is null)
        {
            throw new NotFoundException("Profile", request.Dto.ProfileId);
        }
        
        var interest = await _unitOfWork.InterestRepository.FirstOrDefaultAsync(request.Dto.InterestId, cancellationToken);
        
        if (interest is null)
        {
            throw new NotFoundException("Interest", request.Dto.InterestId);
        }

        var isContains = profileWithInterests.ContainsInterest(request.Dto.InterestId);

        if (isContains)
        {
            throw new AlreadyContainsException(ExceptionMessages.ProfileContainsInterest);
        }
        
        var lessThan = profileWithInterests.InterestsLessThan(6);

        if (!lessThan)
        {
            throw new Exception("You exceeded maximum amount of interests");
        }
        
        await _unitOfWork.InterestRepository.AddInterestToProfile(profileWithInterests, interest);
        await _unitOfWork.SaveAsync(cancellationToken);
        
        var cacheKey = $"{_cacheKeyPrefix}:profile:{request.Dto.ProfileId}";
        var mappedInterest= _mapper.Map<List<InterestResponseDto>>(profileWithInterests.Interests);
        await _cacheService.SetAsync(cacheKey, mappedInterest,
            cancellationToken: cancellationToken);
        
        return mappedInterest;
    }
}