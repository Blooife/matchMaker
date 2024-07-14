using MediatR;
using Profile.Application.Exceptions;
using Profile.Application.Exceptions.Messages;
using Profile.Domain.Interfaces;
using Profile.Domain.Specifications.ProfileSpecifications;
using Shared.Models;

namespace Profile.Application.UseCases.InterestUseCases.Commands.RemoveInterestFromProfile;

public class RemoveInterestFromProfileHandler(IUnitOfWork _unitOfWork) : IRequestHandler<RemoveInterestFromProfileCommand, GeneralResponseDto>
{
    public async Task<GeneralResponseDto> Handle(RemoveInterestFromProfileCommand request, CancellationToken cancellationToken)
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

        if (!isContains)
        {
            throw new NotContainsException(ExceptionMessages.ProfileNotContainsInterest);
        }
        
        var interestToRemove = profileWithInterests.Interests.First(i=>i.Id == request.Dto.InterestId);
        await _unitOfWork.InterestRepository.RemoveInterestFromProfile(profileWithInterests, interestToRemove);
        await _unitOfWork.SaveAsync(cancellationToken);
        
        return new GeneralResponseDto();
    }
}