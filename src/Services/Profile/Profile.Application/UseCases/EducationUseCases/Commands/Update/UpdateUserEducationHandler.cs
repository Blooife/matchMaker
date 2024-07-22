using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Education.Response;
using Profile.Application.Exceptions;
using Profile.Application.Exceptions.Messages;
using Profile.Application.Services.Interfaces;
using Profile.Domain.Models;
using Profile.Domain.Repositories;
using Profile.Domain.Specifications.ProfileSpecifications;
using Shared.Models;

namespace Profile.Application.UseCases.EducationUseCases.Commands.Update;

public class UpdateProfileEducationHandler(IUnitOfWork _unitOfWork, IMapper _mapper, ICacheService _cacheService) : IRequestHandler<UpdateProfileEducationCommand, GeneralResponseDto>
{
    private readonly string _cacheKeyPrefix = "education";
    
    public async Task<GeneralResponseDto> Handle(UpdateProfileEducationCommand request, CancellationToken cancellationToken)
    {
        var profileWithEducation = await _unitOfWork.EducationRepository.GetProfileWithEducationAsync(request.Dto.ProfileId, cancellationToken);
        
        if (profileWithEducation is null)
        {
            throw new NotFoundException("Profile", request.Dto.ProfileId);
        }
        
        var education = await _unitOfWork.EducationRepository.FirstOrDefaultAsync(request.Dto.EducationId, cancellationToken);
        
        if (education is null)
        {
            throw new NotFoundException("Education", request.Dto.EducationId);
        }
        
        var isContains = profileWithEducation.ContainsEducation(request.Dto.EducationId);

        if (!isContains)
        {
            throw new NotContainsException(ExceptionMessages.ProfileNotContainsEducation);
        }

        ProfileEducation userEducation = profileWithEducation.ProfileEducations.First(userEducation=>userEducation.EducationId == request.Dto.EducationId);
        
        await _unitOfWork.EducationRepository.UpdateProfilesEducationAsync(userEducation, request.Dto.Description);
        await _unitOfWork.SaveAsync(cancellationToken);
        
        var cacheKey = $"{_cacheKeyPrefix}:profile:{request.Dto.ProfileId}";
        var mappedEducations = _mapper.Map<List<ProfileEducationResponseDto>>(profileWithEducation.ProfileEducations);
        await _cacheService.SetAsync(cacheKey, mappedEducations, cancellationToken:cancellationToken);
        
        
        return new GeneralResponseDto();
    }
}