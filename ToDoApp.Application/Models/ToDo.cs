using ToDoApp.Application.Exceptions;

namespace ToDoApp.Application.Models;

/// <summary>
/// Represents a ToDo task
/// </summary>
public class ToDo
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public DateTime Expiry { get; private set; }
    public int PercentComplete { get; private set; }

    private ToDo()
    {
    }
    
    /// <summary>
    /// Creates a new ToDo task.
    /// </summary>
    /// <param name="id">The unique identifier of the task.</param>
    /// <param name="title">The title of the task.</param>
    /// <param name="description">The description of the task.</param>
    /// <param name="expiry">The expiry date of the task.</param>
    /// <returns>A new instance of <see cref="ToDo"/>.</returns>
    /// <exception cref="ToDoTitleCannotBeEmptyException">Thrown when the title is null, empty, or whitespace.</exception>
    /// <exception cref="ToDoExpiryDateCannotBeInThePastException">Thrown when the expiry date is set in the past.</exception>
    public static ToDo Create(Guid id, string title, string description, DateTime expiry)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ToDoTitleCannotBeEmptyException();

        if (expiry < DateTime.UtcNow.Date)
            throw new ToDoExpiryDateCannotBeInThePastException();

        return new ToDo
        {
            Id = id,
            Title = title,
            Description = description,
            Expiry = expiry,
            PercentComplete = 0,
        };
    }
    
    /// <summary>
    /// Updates the details of the existing ToDo task.
    /// </summary>
    /// <param name="title">The new title of the task.</param>
    /// <param name="description">The new description of the task.</param>
    /// <param name="expiry">The new expiry date of the task.</param>
    /// <param name="percentComplete">The new completion percentage.</param>
    /// <exception cref="ToDoTitleCannotBeEmptyException">Thrown when the title is null, empty, or whitespace.</exception>
    /// <exception cref="ToDoExpiryDateCannotBeInThePastException">Thrown when the expiry date is set in the past.</exception>
    /// <exception cref="ToDoPercentCompleteMustBeBetweenException">Thrown when the percentage is not between 0 and 100.</exception>
    public void Update(string title, string description, DateTime expiry, int percentComplete)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ToDoTitleCannotBeEmptyException();

        if (expiry.Date < DateTime.UtcNow.Date)
            throw new ToDoExpiryDateCannotBeInThePastException();

        if (percentComplete is < 0 or > 100)
            throw new ToDoPercentCompleteMustBeBetweenException();

        Title = title;
        Description = description;
        Expiry = expiry;
        PercentComplete = percentComplete;
    }
    
    /// <summary>
    /// Sets the percentage of task completion.
    /// </summary>
    /// <param name="percentComplete">The new completion percentage.</param>
    /// <exception cref="ToDoPercentCompleteMustBeBetweenException">Thrown when the percentage is not between 0 and 100.</exception>
    public void SetPercentComplete(int percentComplete)
    {
        if (percentComplete is < 0 or > 100)
            throw new ToDoPercentCompleteMustBeBetweenException();

        PercentComplete = percentComplete;
    }
    
    /// <summary>
    /// Marks the task as fully completed by setting the completion percentage to 100%.
    /// </summary>
    public void MarkAsDone()
    {
        PercentComplete = 100;
    }
}