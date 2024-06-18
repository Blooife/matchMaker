using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Language.Response;
using Profile.Application.Exceptions;
using Profile.Application.UseCases.ProfileUseCases.Queries.GetById;
using Profile.Domain.Repositories;

namespace Profile.Application.UseCases.LanguageUseCases.Queries.GetById;

public class GetLanguageByIdHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<GetLanguageByIdQuery, LanguageResponseDto>
{
    public async Task<LanguageResponseDto> Handle(GetLanguageByIdQuery request, CancellationToken cancellationToken)
    {
        var language = await _unitOfWork.LanguageRepository.FirstOrDefaultAsync(request.LanguageId, cancellationToken);
        
        if (language == null)
        {
            throw new NotFoundException("Language", request.LanguageId);
        }
        
        return _mapper.Map<LanguageResponseDto>(language);
    }
}