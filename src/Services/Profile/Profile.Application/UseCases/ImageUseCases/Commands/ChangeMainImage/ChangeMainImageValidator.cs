using FluentValidation;

namespace Profile.Application.UseCases.ImageUseCases.Commands.ChangeMainImage;

public class ChangeMainImageValidator : AbstractValidator<ChangeMainImageCommand>
{
    public ChangeMainImageValidator()
    {
        RuleFor(command => command.Dto.ProfileId).NotEmpty().WithMessage("Profile id cant be empty");
        
        RuleFor(command => command.Dto.ImageId).NotEmpty().WithMessage("Image id cant be empty");
    }
}