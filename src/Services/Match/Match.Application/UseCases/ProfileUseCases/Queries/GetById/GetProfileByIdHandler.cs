using AutoMapper;
using Match.Application.DTOs.Profile.Response;
using Match.Application.Exceptions;
using Match.Domain.Repositories;
using MediatR;

namespace Match.Application.UseCases.ProfileUseCases.Queries.GetById;

public class GetProfileByIdHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<GetProfileByIdQuery, ProfileResponseDto>
{
    public async Task<ProfileResponseDto> Handle(GetProfileByIdQuery request, CancellationToken cancellationToken)
    {
        var profile = await _unitOfWork.Profiles.GetByIdAsync(request.ProfileId, cancellationToken);
        
        if (profile is null)
        {
            throw new NotFoundException("Profile", request.ProfileId);
        }
        
        return _mapper.Map<ProfileResponseDto>(profile);
    }
}