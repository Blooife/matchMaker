using Match.Application.DTOs.Profile.Response;
using MediatR;
using Shared.Models;

namespace Match.Application.UseCases.ProfileUseCases.Queries.GetPagedRecs;

public sealed record GetPagedRecsQuery(string ProfileId, int PageNumber, int PageSize) : IRequest<PagedList<FullProfileResponseDto>>
{
    
}