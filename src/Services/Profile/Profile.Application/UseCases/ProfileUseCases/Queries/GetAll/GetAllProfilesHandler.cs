using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Profile.Response;
using Profile.Domain.Repositories;

namespace Profile.Application.UseCases.ProfileUseCases.Queries.GetAll;

public class GetAllProfilesHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<GetAllProfilesQuery, IEnumerable<ProfileResponseDto>>
{
    public async Task<IEnumerable<ProfileResponseDto>> Handle(GetAllProfilesQuery request, CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.ProfileRepository.GetAllProfilesAsync(cancellationToken);
        return _mapper.Map<IEnumerable<ProfileResponseDto>>(result);
    }
}