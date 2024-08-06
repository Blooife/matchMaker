using FluentValidation;

namespace Profile.Application.UseCases.EducationUseCases.Commands.RemoveEducationFromProfile;

public class RemoveEducationFromProfileValidator : AbstractValidator<RemoveEducationFromProfileCommand>
{
    public RemoveEducationFromProfileValidator()
    {
        RuleFor(command => command.Dto.ProfileId).NotEmpty().WithMessage("Profile id cant be empty");
        
        RuleFor(command => command.Dto.EducationId).NotEmpty().WithMessage("Education id cant be empty");
    }
}