using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Language.Response;
using Profile.Application.UseCases.ProfileUseCases.Queries.GetById;
using Profile.Domain.Repositories;

namespace Profile.Application.UseCases.LanguageUseCases.Queries.GetById;

public class GetLanguageByIdHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<GetLanguageByIdQuery, LanguageResponseDto>
{
    public async Task<LanguageResponseDto> Handle(GetLanguageByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.LanguageRepository.GetByIdAsync(request.LanguageId, cancellationToken);
        if (result == null)
        {
            //to do exception
        }
        return _mapper.Map<LanguageResponseDto>(result);
    }
}