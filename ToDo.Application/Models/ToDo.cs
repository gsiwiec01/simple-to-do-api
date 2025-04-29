using ToDoApp.Application.Exceptions;

namespace ToDoApp.Application.Models;

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

    public void SetPercentComplete(int percentComplete)
    {
        if (percentComplete is < 0 or > 100)
            throw new ToDoPercentCompleteMustBeBetweenException();

        PercentComplete = percentComplete;
    }

    public void MarkAsDone()
    {
        PercentComplete = 100;
    }
}