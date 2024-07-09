using AutoMapper;
using Match.Application.Exceptions;
using Match.Domain.Repositories;
using MediatR;
using Shared.Models;

namespace Match.Application.UseCases.ProfileUseCases.Commands.Delete;

public class DeleteProfileHandler(IUnitOfWork _unitOfWork) : IRequestHandler<DeleteProfileCommand, GeneralResponseDto>
{
    public async Task<GeneralResponseDto> Handle(DeleteProfileCommand request, CancellationToken cancellationToken)
    {
        var profile = await _unitOfWork.Profiles.GetByIdAsync(request.ProfileId, cancellationToken);
        
        if (profile is null)
        {
            throw new NotFoundException("Profile", request.ProfileId);
        }
        
        await _unitOfWork.Profiles.DeleteAsync(profile, cancellationToken);
        await _unitOfWork.Chats.DeleteManyAsync(
            chat => chat.FirstProfileId == profile.Id || chat.SecondProfileId == profile.Id, cancellationToken);
        var chats = await _unitOfWork.Chats.GetChatsByProfileIdAsync(profile.Id, cancellationToken);
        var chatIds = chats.Select(c => c.Id).ToList();
        await _unitOfWork.Messages.DeleteManyAsync(message => chatIds.Contains(message.ChatId), cancellationToken);
        await _unitOfWork.Matches.DeleteManyAsync(
            match => match.FirstProfileId == profile.Id || match.SecondProfileId == profile.Id, cancellationToken);
        
        return new GeneralResponseDto();
    }
}