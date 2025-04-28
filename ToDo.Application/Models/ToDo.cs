namespace ToDoApp.Application.Models;

public class ToDo
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public DateTime Expiry { get; private set; }
    public int PercentComplete { get; private set; }
    public bool IsDone { get; private set; }

    private ToDo()
    {
    }
}