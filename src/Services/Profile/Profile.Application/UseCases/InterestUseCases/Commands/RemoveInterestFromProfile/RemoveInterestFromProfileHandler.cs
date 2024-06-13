using AutoMapper;
using MediatR;
using Profile.Domain.Repositories;
using Shared.Models;

namespace Profile.Application.UseCases.InterestUseCases.Commands.RemoveInterestFromProfile;

public class RemoveInterestFromProfileHandler(IUnitOfWork _unitOfWork) : IRequestHandler<RemoveInterestFromProfileCommand, GeneralResponseDto>
{
    public async Task<GeneralResponseDto> Handle(RemoveInterestFromProfileCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.InterestRepository.RemoveInterestFromProfile(request.Dto.ProfileId, request.Dto.InterestId, cancellationToken);
        return new GeneralResponseDto();
    }
}