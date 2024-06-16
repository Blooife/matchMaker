using Authentication.BusinessLogic.DTOs.Request;
using FluentValidation;

namespace Authentication.BusinessLogic.Validators;

public class UserValidator : AbstractValidator<UserRequestDto>
{
    public UserValidator()
    {
        RuleFor(dto => dto.Email)
            .NotEmpty().WithMessage("Email is required")
            .MaximumLength(100).WithMessage("Email length must be less than 100")
            .EmailAddress().WithMessage("Invalid email");
        
        RuleFor(dto => dto.Password)
            .NotEmpty().WithMessage("Password is required");
    }
}