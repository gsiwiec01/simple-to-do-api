using FluentValidation;

namespace ToDoApp.API.Requests.Validators;

public sealed class CreateToDoRequestValidator : AbstractValidator<CreateToDoRequest>
{
    public CreateToDoRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(500);

        RuleFor(x => x.Expiry)
            .GreaterThan(DateTime.UtcNow).WithMessage("Expiry must be in the future.");
    }
}