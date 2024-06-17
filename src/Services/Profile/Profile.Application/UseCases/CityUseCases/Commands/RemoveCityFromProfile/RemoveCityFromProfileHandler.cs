using AutoMapper;
using MediatR;
using Profile.Domain.Repositories;
using Profile.Domain.Specifications.ProfileSpecifications;
using Shared.Models;

namespace Profile.Application.UseCases.CityUseCases.Commands.RemoveCityFromProfile;

public class RemoveCityFromProfileHandler(IUnitOfWork _unitOfWork) : IRequestHandler<RemoveCityFromProfileCommand, GeneralResponseDto>
{
    public async Task<GeneralResponseDto> Handle(RemoveCityFromProfileCommand request, CancellationToken cancellationToken)
    {
        var profile = await _unitOfWork.ProfileRepository.GetByIdAsync(request.Dto.ProfileId, cancellationToken);
        
        if (profile is null)
        {
            throw new Exception();
        }
        
        await _unitOfWork.CityRepository.RemoveCityFromProfile(profile, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
        
        return new GeneralResponseDto();
    }
}