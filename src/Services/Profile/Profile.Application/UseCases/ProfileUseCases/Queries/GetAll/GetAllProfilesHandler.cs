using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Profile.Response;
using Profile.Domain.Interfaces;

namespace Profile.Application.UseCases.ProfileUseCases.Queries.GetAll;

public class GetAllProfilesHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<GetAllProfilesQuery, IEnumerable<ProfileResponseDto>>
{
    public async Task<IEnumerable<ProfileResponseDto>> Handle(GetAllProfilesQuery request, CancellationToken cancellationToken)
    {
        var profiles = await _unitOfWork.ProfileRepository.GetAllAsync(cancellationToken);
        
        return _mapper.Map<IEnumerable<ProfileResponseDto>>(profiles);
    }
}