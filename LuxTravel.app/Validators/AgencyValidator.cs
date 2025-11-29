using FluentValidation;
using LuxTravel.app.Models;

namespace LuxTravel.app.Validators;
internal class AgencyValidator : AbstractValidator<Agency>
{
    public AgencyValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name must not be empty!");
        RuleFor(x => x.Country).NotEmpty().WithMessage("Country must not be empty!");
        RuleFor(x => x.City).NotEmpty().WithMessage("Country must not be empty!");
        RuleFor(x => x.Address).NotEmpty().WithMessage("Adress must not be empty!");
        RuleFor(x => x.Description).NotEmpty().MinimumLength(6).WithMessage("Description must be at least 6 characters long!");
    }
}
