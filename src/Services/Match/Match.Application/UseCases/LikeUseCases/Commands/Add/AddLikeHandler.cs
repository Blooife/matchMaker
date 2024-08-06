using AutoMapper;
using Match.Application.DTOs.Like.Response;
using Match.Application.Exceptions;
using Match.Domain.Models;
using Match.Domain.Interfaces;
using MediatR;

namespace Match.Application.UseCases.LikeUseCases.Commands.Add;

public class AddLikeHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<AddLikeCommand, LikeResponseDto>
{
    public async Task<LikeResponseDto> Handle(AddLikeCommand request, CancellationToken cancellationToken)
    {
        var likeEntity = _mapper.Map<Like>(request.Dto);

        var likerProfile = await _unitOfWork.Profiles.GetByIdAsync(likeEntity.ProfileId, cancellationToken);
        var likedProfile = await _unitOfWork.Profiles.GetByIdAsync(likeEntity.TargetProfileId, cancellationToken);

        if (likerProfile is null)
        {
            throw new NotFoundException("Liker profile", request.Dto.ProfileId);
        }
        
        if (likedProfile is null)
        {
            throw new NotFoundException("Liked profile", request.Dto.TargetProfileId);
        }
        
        var mutualLike = await _unitOfWork.Likes.CheckMutualLikeAsync(likeEntity, cancellationToken);
            
        if (mutualLike is not null)
        {
            var matchEntity = new MatchEntity()
            {
                FirstProfileId = likeEntity.ProfileId,
                SecondProfileId = likeEntity.TargetProfileId,
                Timestamp = DateTime.UtcNow
            };
            await _unitOfWork.Matches.CreateAsync(matchEntity, cancellationToken);
            await _unitOfWork.Likes.DeleteAsync(mutualLike, cancellationToken);
        }
        else
        {
            await _unitOfWork.Likes.CreateAsync(likeEntity, cancellationToken);
        }
        
        return _mapper.Map<LikeResponseDto>(likeEntity);
    }
}