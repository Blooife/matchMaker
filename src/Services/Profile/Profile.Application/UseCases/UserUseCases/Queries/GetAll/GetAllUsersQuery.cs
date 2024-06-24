using MediatR;
using Profile.Application.DTOs.Profile.Response;

namespace Profile.Application.UseCases.UserUseCases.Queries.GetAll;

public sealed record GetAllUsersQuery : IRequest<IEnumerable<UserResponseDto>>
{
    
}