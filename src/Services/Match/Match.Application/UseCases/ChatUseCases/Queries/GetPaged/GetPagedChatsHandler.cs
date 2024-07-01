using AutoMapper;
using Match.Application.Exceptions;
using Match.Domain.Models;
using Match.Domain.Repositories;
using MediatR;
using Shared.Models;

namespace Match.Application.UseCases.ChatUseCases.Queries.GetPaged;

public class GetPagedChatsHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<GetPagedChatsQuery, PagedList<Chat>>
{
    public async Task<PagedList<Chat>> Handle(GetPagedChatsQuery request, CancellationToken cancellationToken)
    {
        var profile = await _unitOfWork.Profiles.GetByIdAsync(request.ProfileId, cancellationToken);

        if (profile is null)
        {
            throw new NotFoundException("Profile", request.ProfileId);
        }

        var chats = await _unitOfWork.Chats.GetPagedAsync(request.ProfileId, request.PageNumber, request.PageSize,
            cancellationToken);

        return chats;
    }
}