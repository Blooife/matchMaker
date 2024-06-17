using AutoMapper;
using MediatR;
using Profile.Domain.Repositories;
using Profile.Domain.Specifications.ProfileSpecifications;
using Shared.Models;

namespace Profile.Application.UseCases.CityUseCases.Commands.AddCityToProfile;

public class AddCityToProfileHandler(IUnitOfWork _unitOfWork) : IRequestHandler<AddCityToProfileCommand, GeneralResponseDto>
{
    public async Task<GeneralResponseDto> Handle(AddCityToProfileCommand request, CancellationToken cancellationToken)
    {
        var profile = await _unitOfWork.ProfileRepository.GetByIdAsync(request.Dto.ProfileId, cancellationToken);
        
        if (profile is null)
        {
            throw new Exception();
        }
        
        var city = await _unitOfWork.CityRepository.GetByIdAsync(request.Dto.CityId, cancellationToken);
        
        if (city is null)
        {
            throw new Exception();
        }
        
        await _unitOfWork.CityRepository.AddCityToProfile(profile, city, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
        
        return new GeneralResponseDto();
    }
}