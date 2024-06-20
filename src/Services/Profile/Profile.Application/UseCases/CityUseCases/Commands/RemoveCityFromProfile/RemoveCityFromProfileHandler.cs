using MediatR;
using Profile.Application.Exceptions;
using Profile.Domain.Repositories;
using Shared.Models;

namespace Profile.Application.UseCases.CityUseCases.Commands.RemoveCityFromProfile;

public class RemoveCityFromProfileHandler(IUnitOfWork _unitOfWork) : IRequestHandler<RemoveCityFromProfileCommand, GeneralResponseDto>
{
    public async Task<GeneralResponseDto> Handle(RemoveCityFromProfileCommand request, CancellationToken cancellationToken)
    {
        var profile = await _unitOfWork.ProfileRepository.GetByIdAsync(request.Dto.ProfileId, cancellationToken);
        
        if (profile is null)
        {
            throw new NotFoundException("Profile", request.Dto.ProfileId);
        }
        
        await _unitOfWork.CityRepository.RemoveCityFromProfile(profile, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
        
        return new GeneralResponseDto();
    }
}