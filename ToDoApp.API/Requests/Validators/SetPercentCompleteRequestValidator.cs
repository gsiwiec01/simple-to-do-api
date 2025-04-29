using FluentValidation;

namespace ToDoApp.API.Requests.Validators;

public class SetPercentCompleteRequestValidator : AbstractValidator<SetPercentCompleteRequest>
{
    public SetPercentCompleteRequestValidator()
    {
        RuleFor(x => x.PercentComplete)
            .InclusiveBetween(0, 100);
    }
}