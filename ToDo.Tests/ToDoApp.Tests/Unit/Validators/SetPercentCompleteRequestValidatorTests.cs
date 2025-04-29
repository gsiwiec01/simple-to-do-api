using FluentAssertions;
using FluentValidation.TestHelper;
using ToDoApp.API.Requests;
using ToDoApp.API.Requests.Validators;

namespace ToDoApp.Tests.Unit.Validators;

public class SetPercentCompleteRequestValidatorTests
{
    private readonly SetPercentCompleteRequestValidator _validator = new();

    [Theory]
    [InlineData(-1)]
    [InlineData(101)]
    public void Validate_WithPercentOutOfRange_ShouldHaveValidationError(int value)
    {
        var request = new SetPercentCompleteRequest(value);
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.PercentComplete);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(50)]
    [InlineData(100)]
    public void Validate_WithValidPercent_ShouldNotHaveValidationError(int value)
    {
        var request = new SetPercentCompleteRequest(value);
        var result = _validator.TestValidate(request);
        result.IsValid.Should().BeTrue();
    }
}
