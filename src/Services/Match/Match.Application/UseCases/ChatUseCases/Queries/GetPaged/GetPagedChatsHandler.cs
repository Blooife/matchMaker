using AutoMapper;
using Match.Application.DTOs.Chat.Response;
using Match.Application.Exceptions;
using Match.Domain.Interfaces;
using MediatR;
using Shared.Models;

namespace Match.Application.UseCases.ChatUseCases.Queries.GetPaged;

public class GetPagedChatsHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<GetPagedChatsQuery, PagedList<ChatResponseDto>>
{
    public async Task<PagedList<ChatResponseDto>> Handle(GetPagedChatsQuery request, CancellationToken cancellationToken)
    {
        var profile = await _unitOfWork.Profiles.GetByIdAsync(request.ProfileId, cancellationToken);

        if (profile is null)
        {
            throw new NotFoundException("Profile", request.ProfileId);
        }

        var result = await _unitOfWork.Chats.GetPagedAsync(request.ProfileId, request.PageNumber, request.PageSize,
            cancellationToken);
        
        ///what if profile already deleted. think about it. think about when creating chat add names
        var profileIds = result.Item1
            .SelectMany(chat => new[] { chat.FirstProfileId, chat.SecondProfileId })
            .Distinct()
            .ToList();
        
        var profiles = await _unitOfWork.Profiles.GetAsync(p => profileIds.Contains(p.Id), cancellationToken);

        var profileDictionary = profiles.ToDictionary(p => p.Id);

        var chatResponseDtos = result.Item1.Select(chat =>
        {
            var otherProfileId = chat.FirstProfileId == request.ProfileId ? chat.SecondProfileId : chat.FirstProfileId;
            var otherProfile = profileDictionary[otherProfileId];

            return new ChatResponseDto
            {
                Id = chat.Id,
                FirstProfileId = chat.FirstProfileId,
                SecondProfileId = chat.SecondProfileId,
                ProfileName = otherProfile.Name,
                ProfileLastName = otherProfile.LastName,
                MainImageUrl = otherProfile.MainImageUrl
            };
        }).ToList();

        return new PagedList<ChatResponseDto>(chatResponseDtos, result.Item2, request.PageNumber, request.PageSize);
    }
}