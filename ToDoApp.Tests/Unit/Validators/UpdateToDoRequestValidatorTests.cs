using ToDoApp.API.Requests;
using ToDoApp.API.Requests.Validators;

namespace ToDoApp.Tests.Unit.Validators;

using Xunit;
using FluentValidation.TestHelper;
using FluentAssertions;
using System;

public class UpdateToDoRequestValidatorTests
{
    private readonly UpdateToDoRequestValidator _validator = new();

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Validate_WithEmptyOrNullTitle_ShouldHaveValidationError(string invalidTitle)
    {
        var request = new UpdateToDoRequest(invalidTitle, "desc", DateTime.UtcNow.AddDays(1), 50);
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Validate_WithEmptyOrNullDescription_ShouldHaveValidationError(string invalidDescription)
    {
        var request = new UpdateToDoRequest("title", invalidDescription, DateTime.UtcNow.AddDays(1), 50);
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Validate_WithExpiryInThePast_ShouldHaveValidationError()
    {
        var request = new UpdateToDoRequest("title", "desc", DateTime.UtcNow.AddMinutes(-1), 50);
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Expiry);
    }

    [Theory]
    [InlineData(-10)]
    [InlineData(101)]
    public void Validate_WithPercentCompleteOutOfRange_ShouldHaveValidationError(int invalidPercent)
    {
        var request = new UpdateToDoRequest("title", "desc", DateTime.UtcNow.AddDays(1), invalidPercent);
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.PercentComplete);
    }

    [Fact]
    public void Validate_WithValidRequest_ShouldNotHaveValidationError()
    {
        var request = new UpdateToDoRequest("Valid Title", "Valid Description", DateTime.UtcNow.AddDays(2), 80);
        var result = _validator.TestValidate(request);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_WithTooLongTitle_ShouldHaveValidationError()
    {
        var longTitle = new string('x', 101);
        var request = new UpdateToDoRequest(longTitle, "desc", DateTime.UtcNow.AddDays(1), 50);
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Fact]
    public void Validate_WithTooLongDescription_ShouldHaveValidationError()
    {
        var longDesc = new string('x', 501);
        var request = new UpdateToDoRequest("title", longDesc, DateTime.UtcNow.AddDays(1), 50);
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }
}

