using MediatR;
using Profile.Application.Exceptions;
using Profile.Domain.Repositories;
using Shared.Models;

namespace Profile.Application.UseCases.CityUseCases.Commands.AddCityToProfile;

public class AddCityToProfileHandler(IUnitOfWork _unitOfWork) : IRequestHandler<AddCityToProfileCommand, GeneralResponseDto>
{
    public async Task<GeneralResponseDto> Handle(AddCityToProfileCommand request, CancellationToken cancellationToken)
    {
        var profile = await _unitOfWork.ProfileRepository.GetByIdAsync(request.Dto.ProfileId, cancellationToken);
        
        if (profile is null)
        {
            throw new NotFoundException("Profile", request.Dto.ProfileId);
        }
        
        var city = await _unitOfWork.CityRepository.GetByIdAsync(request.Dto.CityId, cancellationToken);
        
        if (city is null)
        {
            throw new NotFoundException("City", request.Dto.CityId);
        }
        
        await _unitOfWork.CityRepository.AddCityToProfile(profile, city, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
        
        return new GeneralResponseDto();
    }
}