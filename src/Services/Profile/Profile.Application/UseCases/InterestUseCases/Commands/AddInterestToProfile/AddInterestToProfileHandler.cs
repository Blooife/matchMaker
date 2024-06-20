using MediatR;
using Profile.Application.Exceptions;
using Profile.Application.Exceptions.Messages;
using Profile.Domain.Repositories;
using Profile.Domain.Specifications.ProfileSpecifications;
using Shared.Models;

namespace Profile.Application.UseCases.InterestUseCases.Commands.AddInterestToProfile;

public class AddInterestToProfileHandler(IUnitOfWork _unitOfWork) : IRequestHandler<AddInterestToProfileCommand, GeneralResponseDto>
{
    public async Task<GeneralResponseDto> Handle(AddInterestToProfileCommand request, CancellationToken cancellationToken)
    {
        var profileWithInterests = await _unitOfWork.InterestRepository.GetUserWithInterests(request.Dto.ProfileId, cancellationToken);
        
        if (profileWithInterests is null)
        {
            throw new NotFoundException("Profile", request.Dto.ProfileId);
        }
        
        var interest = await _unitOfWork.InterestRepository.GetByIdAsync(request.Dto.InterestId, cancellationToken);
        
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
        
        return new GeneralResponseDto();
    }
}