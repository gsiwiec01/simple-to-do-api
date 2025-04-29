using FluentValidation;

namespace ToDoApp.API.Requests.Validators;

public sealed class UpdateToDoRequestValidator : AbstractValidator<UpdateToDoRequest>
{
    public UpdateToDoRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(500);

        RuleFor(x => x.Expiry)
            .GreaterThan(DateTime.UtcNow).WithMessage("Expiry must be in the future.");

        RuleFor(x => x.PercentComplete)
            .InclusiveBetween(0, 100);
    }
}