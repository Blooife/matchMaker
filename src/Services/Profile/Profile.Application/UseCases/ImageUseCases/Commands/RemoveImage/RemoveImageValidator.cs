using FluentValidation;

namespace Profile.Application.UseCases.ImageUseCases.Commands.RemoveImage;

public class RemoveImageValidator : AbstractValidator<RemoveImageCommand>
{
    public RemoveImageValidator()
    {
        RuleFor(command => command.Dto.ProfileId).NotEmpty().WithMessage("Profile id cant be empty");
        
        RuleFor(command => command.Dto.ImageId).NotEmpty().WithMessage("Image id cant be empty");
    }
}