using AutoMapper;
using Match.Application.DTOs.Profile.Response;
using Match.Domain.Repositories;
using MediatR;

namespace Match.Application.UseCases.ProfileUseCases.Queries.GetById;

public class GetProfileByIdHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<GetProfileByIdQuery, ProfileResponseDto>
{
    public async Task<ProfileResponseDto> Handle(GetProfileByIdQuery request, CancellationToken cancellationToken)
    {
        var profileRepository = _unitOfWork.Profiles;
        var profile = await profileRepository.GetByIdAsync(request.ProfileId, cancellationToken);
        
        if (profile == null)
        {
            
        }
        
        return _mapper.Map<ProfileResponseDto>(profile);
    }
}