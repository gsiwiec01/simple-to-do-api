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
        return new ToDo()
        {
            Id = id,
            Title = title,
            Description = description,
            Expiry = expiry,
            PercentComplete = 0
        };
    }

    public void Update(string title, string description, DateTime expiry, int percentComplete)
    {
        Title = title;
        Description = description;
        Expiry = expiry;
        PercentComplete = percentComplete;
    }
}