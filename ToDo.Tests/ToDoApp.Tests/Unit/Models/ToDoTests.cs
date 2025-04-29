using ToDoApp.Application.Exceptions;

namespace ToDoApp.Tests.Unit;

public class ToDoTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreateToDo()
    {
        // Arrange
        var id = Guid.NewGuid();
        var title = "Test todo";
        var description = "Test description";
        var expiry = DateTime.UtcNow.AddDays(1);

        // Act
        var todo = Application.Models.ToDo.Create(id, title, description, expiry);

        // Assert
        Assert.Equal(id, todo.Id);
        Assert.Equal(title, todo.Title);
        Assert.Equal(description, todo.Description);
        Assert.Equal(expiry, todo.Expiry);
        Assert.Equal(0, todo.PercentComplete);
    }

    [Fact]
    public void Create_WithEmptyTitle_ShouldThrowException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var expiry = DateTime.UtcNow.AddDays(1);

        // Act & Assert
        Assert.Throws<ToDoTitleCannotBeEmptyException>(() =>
            Application.Models.ToDo.Create(id, "", "desc", expiry));
    }

    [Fact]
    public void Create_WithPastExpiry_ShouldThrowException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var expiry = DateTime.UtcNow.AddDays(-1);

        // Act & Assert
        Assert.Throws<ToDoExpiryDateCannotBeInThePastException>(() =>
            Application.Models.ToDo.Create(id, "Test", "desc", expiry));
    }

    [Fact]
    public void Update_WithValidData_ShouldUpdateProperties()
    {
        // Arrange
        var todo = Application.Models.ToDo.Create(Guid.NewGuid(), "Title", "Desc", DateTime.UtcNow.AddDays(1));
        var newTitle = "New Title";
        var newDesc = "New Description";
        var newExpiry = DateTime.UtcNow.AddDays(2);
        var newPercent = 50;

        // Act
        todo.Update(newTitle, newDesc, newExpiry, newPercent);

        // Assert
        Assert.Equal(newTitle, todo.Title);
        Assert.Equal(newDesc, todo.Description);
        Assert.Equal(newExpiry, todo.Expiry);
        Assert.Equal(newPercent, todo.PercentComplete);
    }

    [Fact]
    public void Update_WithInvalidTitle_ShouldThrowException()
    {
        var todo = Application.Models.ToDo.Create(Guid.NewGuid(), "Title", "Desc", DateTime.UtcNow.AddDays(1));

        Assert.Throws<ToDoTitleCannotBeEmptyException>(() =>
            todo.Update("", "desc", DateTime.UtcNow.AddDays(1), 10));
    }

    [Fact]
    public void Update_WithPastExpiry_ShouldThrowException()
    {
        var todo = Application.Models.ToDo.Create(Guid.NewGuid(), "Title", "Desc", DateTime.UtcNow.AddDays(1));

        Assert.Throws<ToDoExpiryDateCannotBeInThePastException>(() =>
            todo.Update("New Title", "desc", DateTime.UtcNow.AddDays(-1), 10));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(101)]
    public void Update_WithInvalidPercent_ShouldThrowException(int percent)
    {
        var todo = Application.Models.ToDo.Create(Guid.NewGuid(), "Title", "Desc", DateTime.UtcNow.AddDays(1));

        Assert.Throws<ToDoPercentCompleteMustBeBetweenException>(() =>
            todo.Update("New Title", "desc", DateTime.UtcNow.AddDays(1), percent));
    }

    [Fact]
    public void SetPercentComplete_WithValidValue_ShouldSet()
    {
        var todo = Application.Models.ToDo.Create(Guid.NewGuid(), "Title", "Desc", DateTime.UtcNow.AddDays(1));
        todo.SetPercentComplete(75);

        Assert.Equal(75, todo.PercentComplete);
    }

    [Theory]
    [InlineData(-5)]
    [InlineData(105)]
    public void SetPercentComplete_WithInvalidValue_ShouldThrowException(int value)
    {
        var todo = Application.Models.ToDo.Create(Guid.NewGuid(), "Title", "Desc", DateTime.UtcNow.AddDays(1));

        Assert.Throws<ToDoPercentCompleteMustBeBetweenException>(() =>
            todo.SetPercentComplete(value));
    }

    [Fact]
    public void MarkAsDone_ShouldSetPercentCompleteTo100()
    {
        var todo = Application.Models.ToDo.Create(Guid.NewGuid(), "Title", "Desc", DateTime.UtcNow.AddDays(1));

        todo.MarkAsDone();

        Assert.Equal(100, todo.PercentComplete);
    }
}