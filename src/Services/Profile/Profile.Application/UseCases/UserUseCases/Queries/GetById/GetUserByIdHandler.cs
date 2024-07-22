using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Profile.Response;
using Profile.Application.Exceptions;
using Profile.Application.Services.Interfaces;
using Profile.Domain.Repositories;

namespace Profile.Application.UseCases.UserUseCases.Queries.GetById;

public class GetUserByIdHandler(IUnitOfWork _unitOfWork, IMapper _mapper, ICacheService _cacheService) : IRequestHandler<GetUserByIdQuery, UserResponseDto>
{
    private readonly string _cacheKeyPrefix = "user";
    
    public async Task<UserResponseDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"{_cacheKeyPrefix}:{request.UserId}";
        var cachedData = await _cacheService.GetAsync<UserResponseDto>(cacheKey, cancellationToken);
        
        if (cachedData is not null)
        {
            return cachedData;
        }
        
        var user = await _unitOfWork.UserRepository.FirstOrDefaultAsync(request.UserId, cancellationToken);
        
        if (user is null)
        {
            throw new NotFoundException("User", request.UserId);
        }
        
        var mappedUser = _mapper.Map<UserResponseDto>(user);
        await _cacheService.SetAsync(cacheKey, mappedUser, cancellationToken:cancellationToken);
        
        return mappedUser;
    }
}