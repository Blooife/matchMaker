using AutoMapper;
using Match.Application.DTOs.Profile.Response;
using Match.Application.Exceptions;
using Match.Domain.Repositories;
using MediatR;
using MongoDB.Driver.GeoJsonObjectModel;
using Profile = Match.Domain.Models.Profile;

namespace Match.Application.UseCases.ProfileUseCases.Commands.UpdateLocation;

public class UpdateLocationHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<UpdateLocationCommand, ProfileResponseDto>
{
    public async Task<ProfileResponseDto> Handle(UpdateLocationCommand request, CancellationToken cancellationToken)
    {
        var profileRepository = _unitOfWork.Profiles;
        var dto = request.Dto;
        var profile = await profileRepository.GetByIdAsync(dto.ProfileId, cancellationToken);
        
        if (profile is null)
        {
            throw new NotFoundException("Profile", dto.ProfileId);
        }

        if (dto.Longitude is not null && dto.Latitude is not null)
        {
            profile.Location =
                new GeoJsonPoint<GeoJson2DCoordinates>(new GeoJson2DCoordinates(dto.Latitude.Value, dto.Longitude.Value));
        }
        else
        {
            profile.Location = null;
        }
        
        await profileRepository.UpdateAsync(profile, cancellationToken);
        
        return _mapper.Map<ProfileResponseDto>(profile);
    }
}