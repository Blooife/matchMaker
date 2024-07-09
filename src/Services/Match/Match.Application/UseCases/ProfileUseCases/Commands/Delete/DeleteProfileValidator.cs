using FluentValidation;

namespace Match.Application.UseCases.ProfileUseCases.Commands.Delete;

public class DeleteProfileValidator : AbstractValidator<DeleteProfileCommand>
{
    public DeleteProfileValidator()
    {
        RuleFor(command => command.ProfileId).NotEmpty();
    }
}