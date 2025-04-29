using ToDoApp.Application.Models;

namespace ToDoApp.Application.Interfaces.Persistence;

/// <summary>
/// Repository interface for managing ToDo entities.
/// </summary>
public interface IToDoRepository
{
    /// <summary>
    /// Retrieves all ToDo entities.
    /// </summary>
    /// <returns>A list of all ToDo entities.</returns>
    Task<List<ToDo>> GetAllAsync();

    /// <summary>
    /// Retrieves a ToDo entity by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the ToDo entity.</param>
    /// <returns>The ToDo entity, or null if it does not exist.</returns>
    Task<ToDo?> GetByIdAsync(Guid id);

    /// <summary>
    /// Retrieves ToDo entities with expiry dates between the specified start and end dates.
    /// </summary>
    /// <param name="start">The start of the date range (inclusive).</param>
    /// <param name="end">The end of the date range (inclusive).</param>
    /// <returns>A list of ToDo entities within the specified date range.</returns>
    Task<List<ToDo>> GetIncomingBetweenAsync(DateTime start, DateTime end);

    /// <summary>
    /// Adds a new ToDo entity.
    /// </summary>
    /// <param name="todo">The ToDo entity to add.</param>
    Task AddAsync(ToDo todo);

    /// <summary>
    /// Updates an existing ToDo entity.
    /// </summary>
    /// <param name="todo">The ToDo entity to update.</param>
    void Update(ToDo todo);

    /// <summary>
    /// Deletes a ToDo entity.
    /// </summary>
    /// <param name="todo">The ToDo entity to delete.</param>
    void Delete(ToDo todo);
}