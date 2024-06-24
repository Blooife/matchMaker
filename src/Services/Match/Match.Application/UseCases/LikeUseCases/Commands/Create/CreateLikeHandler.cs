using AutoMapper;
using Match.Application.DTOs.Like.Response;
using Match.Application.Exceptions;
using Match.Domain.Models;
using Match.Domain.Repositories;
using MediatR;

namespace Match.Application.UseCases.LikeUseCases.Commands.Create;

public class CreateLikeHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<CreateLikeCommand, LikeResponseDto>
{
    public async Task<LikeResponseDto> Handle(CreateLikeCommand request, CancellationToken cancellationToken)
    {
        var likeRepository = _unitOfWork.Likes;
        var likeEntity = _mapper.Map<Like>(request.Dto);

        var profileRepository = _unitOfWork.Profiles;
        var profile1 = await profileRepository.GetByIdAsync(likeEntity.ProfileId, cancellationToken);
        var profile2 = await profileRepository.GetByIdAsync(likeEntity.TargetProfileId, cancellationToken);

        if (profile1 is null || profile2 is null)
        {
            throw new NotFoundException();
        }
        
        var mutualLike = await likeRepository.CheckMutualLike(likeEntity, cancellationToken);
            
        if (mutualLike is not null)
        {
            var matchEntity = new MatchEntity()
            {
                ProfileId1 = likeEntity.ProfileId,
                ProfileId2 = likeEntity.TargetProfileId,
                Timestamp = DateTime.UtcNow
            };
            await _unitOfWork.Matches.CreateAsync(matchEntity, cancellationToken);
            await _unitOfWork.Likes.DeleteAsync(mutualLike, cancellationToken);
        }
        else
        {
            await likeRepository.CreateAsync(likeEntity, cancellationToken);
        }
        
        return _mapper.Map<LikeResponseDto>(likeEntity);
    }
}