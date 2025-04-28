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
            throw new ArgumentException("Title cannot be empty.", nameof(title));

        if (expiry < DateTime.UtcNow.Date)
            throw new ArgumentException("Expiry date cannot be in the past.", nameof(expiry));

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
            throw new ArgumentException("Title cannot be empty.", nameof(title));

        if (expiry.Date < DateTime.UtcNow.Date)
            throw new ArgumentException("Expiry date cannot be earlier than today.", nameof(expiry));
        
        if (percentComplete is < 0 or > 100)
            throw new ArgumentOutOfRangeException(nameof(percentComplete), "Percent complete must be between 0 and 100.");

        Title = title;
        Description = description;
        Expiry = expiry;
        PercentComplete = percentComplete;
    }

    public void SetPercentComplete(int percentComplete)
    {
        if (percentComplete is < 0 or > 100)
            throw new ArgumentOutOfRangeException(nameof(percentComplete), "Percent complete must be between 0 and 100.");

        PercentComplete = percentComplete;
    }

    public void MarkAsDone()
    {
        PercentComplete = 100;
    }
}