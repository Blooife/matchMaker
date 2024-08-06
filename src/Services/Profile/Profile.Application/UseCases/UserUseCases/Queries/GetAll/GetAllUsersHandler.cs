using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Profile.Response;
using Profile.Domain.Interfaces;

namespace Profile.Application.UseCases.UserUseCases.Queries.GetAll;

public class GetAllUsersHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<GetAllUsersQuery, IEnumerable<UserResponseDto>>
{
    public async Task<IEnumerable<UserResponseDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _unitOfWork.UserRepository.GetAllAsync(cancellationToken);
        
        return _mapper.Map<IEnumerable<UserResponseDto>>(users);
    }
}