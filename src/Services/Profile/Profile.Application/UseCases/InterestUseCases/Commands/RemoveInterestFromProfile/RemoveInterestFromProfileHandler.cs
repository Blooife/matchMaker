using AutoMapper;
using MediatR;
using Profile.Domain.Repositories;
using Profile.Domain.Specifications.ProfileSpecifications;
using Shared.Models;

namespace Profile.Application.UseCases.InterestUseCases.Commands.RemoveInterestFromProfile;

public class RemoveInterestFromProfileHandler(IUnitOfWork _unitOfWork) : IRequestHandler<RemoveInterestFromProfileCommand, GeneralResponseDto>
{
    public async Task<GeneralResponseDto> Handle(RemoveInterestFromProfileCommand request, CancellationToken cancellationToken)
    {
        var profileWithInterests = await _unitOfWork.InterestRepository.GetUserWithInterests(request.Dto.ProfileId, cancellationToken);
        
        if (profileWithInterests is null)
        {
            throw new Exception();
        }
        
        var interest = await _unitOfWork.InterestRepository.GetByIdAsync(request.Dto.InterestId, cancellationToken);
        
        if (interest is null)
        {
            throw new Exception();
        }

        var isContains = profileWithInterests.ContainsInterest(request.Dto.InterestId);

        if (!isContains)
        {
            throw new Exception();
        }
        
        await _unitOfWork.InterestRepository.RemoveInterestFromProfile(profileWithInterests, interest);
        await _unitOfWork.SaveAsync(cancellationToken);
        
        return new GeneralResponseDto();
    }
}