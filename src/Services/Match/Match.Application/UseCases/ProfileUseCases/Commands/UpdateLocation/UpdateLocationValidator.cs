using FluentValidation;

namespace Match.Application.UseCases.ProfileUseCases.Commands.UpdateLocation;

public class UpdateLocationValidator : AbstractValidator<UpdateLocationCommand>
{
    public UpdateLocationValidator()
    {
        RuleFor(command => command.Dto.ProfileId).NotEmpty();
    }
}