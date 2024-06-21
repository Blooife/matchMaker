using FluentValidation;

namespace Profile.Application.UseCases.ProfileUseCases.Commands.Create;

public class CreateProfileValidator : AbstractValidator<CreateProfileCommand>
{
    public CreateProfileValidator()
    {
        RuleFor(command => command.CreateProfileDto.Name).MinimumLength(2).MaximumLength(50).WithMessage("Name must be greater than 2 and less than 50 characters");
        RuleFor(command => command.CreateProfileDto.LastName).MinimumLength(2).MaximumLength(50).WithMessage("Lastname must be greater than 2 and less than 50 characters");
        RuleFor(command => command.CreateProfileDto.Bio).MinimumLength(50).MaximumLength(500).WithMessage("Bio must be greater than 50 and less than 500 characters");
        RuleFor(command => command.CreateProfileDto.Height).GreaterThanOrEqualTo(100).LessThanOrEqualTo(220).WithMessage("Height must be greater than 100 and less than 220");
        RuleFor(command => command.CreateProfileDto.BirthDate)
            .LessThanOrEqualTo(DateTime.Today.AddYears(-16))
            .WithMessage("Age must be at least 16 years.");

    }
}