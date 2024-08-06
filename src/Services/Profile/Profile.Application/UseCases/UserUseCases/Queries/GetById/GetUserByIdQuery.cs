using MediatR;
using Profile.Application.DTOs.Profile.Response;

namespace Profile.Application.UseCases.UserUseCases.Queries.GetById;

public sealed record GetUserByIdQuery(string UserId) : IRequest<UserResponseDto>
{
    
}