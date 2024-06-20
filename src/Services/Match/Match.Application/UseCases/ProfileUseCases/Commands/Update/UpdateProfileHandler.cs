using Match.Application.DTOs.Profile.Response;
using MediatR;

namespace Match.Application.UseCases.ProfileUseCases.Commands.Update;

public class UpdateProfileHandler : IRequestHandler<UpdateProfileCommand, ProfileResponseDto>
{
    public async Task<ProfileResponseDto> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}