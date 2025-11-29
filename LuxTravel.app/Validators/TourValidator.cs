using FluentValidation;
using LuxTravel.app.Models;

namespace LuxTravel.app.Validators;

internal class TourValidator : AbstractValidator<Tour>
{
    public TourValidator()
    {
        RuleFor(tour => tour.Name)
            .NotEmpty().WithMessage("Tour name is required.")
            .MaximumLength(100).WithMessage("Tour name cannot exceed 100 characters.");
        RuleFor(tour => tour.StartingPoint)
            .NotEmpty().WithMessage("Starting point is required.")
            .MaximumLength(100).WithMessage("Starting point cannot exceed 100 characters.");
        RuleFor(tour => tour.Destination)
            .NotEmpty().WithMessage("Destination is required.")
            .MaximumLength(100).WithMessage("Destination cannot exceed 100 characters.");
        RuleFor(tour => tour.Description)
            .NotEmpty().WithMessage("Tour description is required.")
            .MaximumLength(1000).WithMessage("Tour description cannot exceed 1000 characters.");
        RuleFor(tour => tour.Price)
            .GreaterThan(0).WithMessage("Tour price must be greater than zero.");
        RuleFor(tour => tour.DurationDays)
            .GreaterThan(0).WithMessage("Tour duration must be at least one day.");
        RuleFor(tour => tour.StartDate)
            .GreaterThanOrEqualTo(DateTime.Today).WithMessage("Tour start date cannot be in the past.");
        RuleFor(tour => tour.EndDate).NotEmpty()
            .GreaterThan(tour => tour.StartDate).WithMessage("Tour end date must be after the start date.");
        RuleFor(tour => tour.MaxParticipants)
            .GreaterThan(0).WithMessage("Maximum participants must be greater than zero.");
        RuleFor(tour => tour.MinAge)
            .GreaterThan(0).WithMessage("Minimum age must be greater than zero.");
    }
}
