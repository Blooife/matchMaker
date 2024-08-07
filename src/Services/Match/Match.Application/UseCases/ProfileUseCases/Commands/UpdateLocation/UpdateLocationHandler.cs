using AutoMapper;
using Match.Application.Exceptions;
using Match.Domain.Interfaces;
using MediatR;
using MongoDB.Driver.GeoJsonObjectModel;

namespace Match.Application.UseCases.ProfileUseCases.Commands.UpdateLocation;

public class UpdateLocationHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<UpdateLocationCommand>
{
    public async Task Handle(UpdateLocationCommand request, CancellationToken cancellationToken)
    {
        var profile = await _unitOfWork.Profiles.GetByIdAsync(request.Dto.ProfileId, cancellationToken);
        
        if (profile is null)
        {
            throw new NotFoundException("Profile", request.Dto.ProfileId);
        }

        if (request.Dto.Longitude is not null && request.Dto.Latitude is not null)
        {
            profile.Location =
                new GeoJsonPoint<GeoJson2DCoordinates>(new GeoJson2DCoordinates(request.Dto.Latitude.Value, request.Dto.Longitude.Value));
        }
        else
        {
            profile.Location = null;
        }
        
        await _unitOfWork.Profiles.UpdateAsync(profile, cancellationToken);
    }
}