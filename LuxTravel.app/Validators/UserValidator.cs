using FluentValidation;
using LuxTravel.app.Models;

namespace LuxTravel.app.Validators;

internal class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name must not be empty!");
        RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name must not be empty!");
        RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("A valid email is required!");
        RuleFor(x => x.UserName).NotEmpty().WithMessage("Username must not be empty!");
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6).WithMessage("Password must be at least 6 characters long!");
    }
}
