using AutoMapper;
using MediatR;
using Profile.Domain.Repositories;
using Shared.Models;

namespace Profile.Application.UseCases.InterestUseCases.Commands.AddInterestToProfile;

public class AddInterestToProfileHandler(IUnitOfWork _unitOfWork) : IRequestHandler<AddInterestToProfileCommand, GeneralResponseDto>
{
    public async Task<GeneralResponseDto> Handle(AddInterestToProfileCommand request, CancellationToken cancellationToken)
    {
        var profile = await _unitOfWork.ProfileRepository.GetProfileByIdAsync(request.Dto.ProfileId, cancellationToken);
        if (profile == null)
        {
            //to do exc
        }

        var interest = await _unitOfWork.InterestRepository.GetByIdAsync(request.Dto.InterestId, cancellationToken);
        if (interest == null)
        {
            //to do exc
        }

        var interests = await _unitOfWork.InterestRepository.GetUsersInterests(profile, cancellationToken);
        if (interests.Count > 4)
        {
            //to do exc
        }
        await _unitOfWork.InterestRepository.AddInterestToProfile(profile, interest, cancellationToken);
        return new GeneralResponseDto();
    }
}