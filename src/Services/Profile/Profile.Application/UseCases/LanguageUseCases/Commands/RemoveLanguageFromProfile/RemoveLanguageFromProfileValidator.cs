using FluentValidation;

namespace Profile.Application.UseCases.LanguageUseCases.Commands.RemoveLanguageFromProfile;

public class RemoveLanguageFromProfileValidator : AbstractValidator<RemoveLanguageFromProfileCommand>
{
    public RemoveLanguageFromProfileValidator()
    {
        RuleFor(command => command.Dto.ProfileId).NotEmpty().WithMessage("Profile id cant be empty");
        
        RuleFor(command => command.Dto.LanguageId).NotEmpty().WithMessage("Language id cant be empty");
    }
}