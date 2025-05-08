using FluentAssertions;
using FluentValidation.TestHelper;
using ToDoApp.API.Requests;
using ToDoApp.API.Requests.Validators;

namespace ToDoApp.Tests.Unit.Validators;

public class CreateToDoRequestValidatorTests
{
    private readonly CreateToDoRequestValidator _validator = new();

    [Fact]
    public void Validate_WithEmptyTitle_ShouldHaveValidationError()
    {
        var request = new CreateToDoRequest("", "desc", DateTime.UtcNow.AddDays(1));
        var result = _validator.TestValidate(request);
        
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Fact]
    public void Validate_WithEmptyDescription_ShouldHaveValidationError()
    {
        var request = new CreateToDoRequest("title", "", DateTime.UtcNow.AddDays(1));
        var result = _validator.TestValidate(request);
        
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Validate_WithPastExpiry_ShouldHaveValidationError()
    {
        var request = new CreateToDoRequest("title", "desc", DateTime.UtcNow.AddDays(-1));
        var result = _validator.TestValidate(request);
        
        result.ShouldHaveValidationErrorFor(x => x.Expiry);
    }

    [Fact]
    public void Validate_WithValidData_ShouldNotHaveValidationErrors()
    {
        var request = new CreateToDoRequest("Valid Title", "Valid Description", DateTime.UtcNow.AddDays(1));
        var result = _validator.TestValidate(request);
        
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_WithTooLongTitle_ShouldHaveValidationError()
    {
        var longTitle = new string('x', 101);
        var request = new CreateToDoRequest(longTitle, "desc", DateTime.UtcNow.AddDays(1));
        var result = _validator.TestValidate(request);
        
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Fact]
    public void Validate_WithTooLongDescription_ShouldHaveValidationError()
    {
        var longDescription = new string('x', 501);
        var request = new CreateToDoRequest("title", longDescription, DateTime.UtcNow.AddDays(1));
        var result = _validator.TestValidate(request);
        
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }
}