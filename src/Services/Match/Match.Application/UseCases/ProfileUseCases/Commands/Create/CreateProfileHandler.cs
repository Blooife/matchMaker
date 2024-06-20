using Match.Application.DTOs.Profile.Response;
using MediatR;

namespace Match.Application.UseCases.ProfileUseCases.Commands.Create;

public class CreateProfileHandler : IRequestHandler<CreateProfileCommand, ProfileResponseDto>
{
    public async Task<ProfileResponseDto> Handle(CreateProfileCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}