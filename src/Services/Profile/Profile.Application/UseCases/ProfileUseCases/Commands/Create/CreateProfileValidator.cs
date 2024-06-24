using FluentValidation;

namespace Profile.Application.UseCases.ProfileUseCases.Commands.Create;

public class CreateProfileValidator : AbstractValidator<CreateProfileCommand>
{
    public CreateProfileValidator()
    {
        RuleFor(command => command.CreateProfileDto.Name)
            .MinimumLength(2)
            .MaximumLength(50)
            .WithMessage("Name must be greater than 2 and less than 50 characters");
        RuleFor(command => command.CreateProfileDto.LastName)
            .MinimumLength(2)
            .MaximumLength(50)
            .WithMessage("Lastname must be greater than 2 and less than 50 characters");
        RuleFor(command => command.CreateProfileDto.Bio)
            .MinimumLength(10)
            .MaximumLength(500)
            .WithMessage("Bio must be greater than 50 and less than 500 characters");
        RuleFor(command => command.CreateProfileDto.Height)
            .GreaterThanOrEqualTo(100)
            .LessThanOrEqualTo(220)
            .WithMessage("Height must be greater than 100 and less than 220");
        RuleFor(command => command.CreateProfileDto.BirthDate)
            .LessThanOrEqualTo(DateTime.Today.AddYears(-16))
            .WithMessage("Age must be at least 16 years.");
        RuleFor(command => command.CreateProfileDto.MaxDistance)
            .NotEmpty().GreaterThanOrEqualTo(0)
            .WithMessage("Max distance must be >= 0");
        RuleFor(command => command.CreateProfileDto.AgeFrom)
            .NotEmpty().GreaterThanOrEqualTo(0)
            .WithMessage("Age from must be >= 0");
        RuleFor(command => command.CreateProfileDto.AgeTo)
            .NotEmpty().GreaterThanOrEqualTo(0)
            .WithMessage("Age to must be >= 0");
        RuleFor(command => command.CreateProfileDto.AgeFrom)
            .LessThan(command => command.CreateProfileDto.AgeTo)
            .WithMessage("Age from must be less than age to");
    }
}