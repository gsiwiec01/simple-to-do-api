namespace ToDoApp.Application.Interfaces.Persistence;

/// <summary>
/// Represents a unit of work that manages saving changes to the persistence layer.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Saves all pending changes to the underlying data store.
    /// </summary>
    /// <param name="cancellationToken">Optional cancellation token to cancel the operation.</param>
    /// <returns>The number of state entries written to the database.</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}