using ToDoApp.Application.Commands;
using ToDoApp.Application.DTOs;
using ToDoApp.Application.Enums;
using ToDoApp.Application.Models;

namespace ToDoApp.Application.Services.Interfaces;

/// <summary>
/// Service handling operations related to ToDo tasks.
/// </summary>
public interface IToDoService
{ 
    /// <summary>
    /// Retrieves a list of all ToDo tasks.
    /// </summary>
    /// <returns>A list of all tasks as DTOs.</returns>
    Task<List<ToDoDto>> GetAllAsync();

    /// <summary>
    /// Retrieves the details of a ToDo task by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the task.</param>
    /// <returns>The task DTO, or null if it does not exist.</returns>
    Task<ToDoDto?> GetByIdAsync(Guid id);

    /// <summary>
    /// Retrieves ToDo tasks within a specific upcoming time range (e.g., today, tomorrow, this week).
    /// </summary>
    /// <param name="scope">The scope specifying the time range.</param>
    /// <returns>A list of tasks within the specified scope.</returns>
    Task<List<ToDoDto>> GetIncomingAsync(IncomingScope scope);

    /// <summary>
    /// Creates a new ToDo task based on the provided data.
    /// </summary>
    /// <param name="command">The data for the new task.</param>
    Task CreateAsync(CreateToDoCommand command);

    /// <summary>
    /// Updates an existing ToDo task based on the provided data.
    /// </summary>
    /// <param name="command">The updated task data.</param>
    Task UpdateAsync(UpdateToDoCommand command);

    /// <summary>
    /// Sets the completion percentage of a ToDo task.
    /// </summary>
    /// <param name="command">The data specifying the task and new completion percentage.</param>
    Task SetPercentCompleteAsync(SetPercentCompleteCommand command);

    /// <summary>
    /// Marks a ToDo task as completed.
    /// </summary>
    /// <param name="id">The identifier of the task.</param>
    Task MarkAsDoneAsync(Guid id);

    /// <summary>
    /// Deletes a ToDo task.
    /// </summary>
    /// <param name="id">The identifier of the task to delete.</param>
    Task DeleteAsync(Guid id);
}