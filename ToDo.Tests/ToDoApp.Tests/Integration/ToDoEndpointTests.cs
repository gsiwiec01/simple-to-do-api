using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using ToDoApp.API.Requests;
using ToDoApp.Application.DTOs;
using ToDoApp.Application.Enums;
using ToDoApp.Persistence.Data;
using ToDoApp.Tests.Common;

namespace ToDoApp.Tests.Integration;

public class ToDoEndpointTests : IntegrationTestBase
{
    public ToDoEndpointTests(TestApplicationFactory factory) : base(factory)
    {
    }

    private CreateToDoRequest NewRequest(string title, DateTime expiry)
        => new(title, "Test description", expiry);

    private async Task<Guid> CreateToDo(string title, DateTime expiry)
    {
        var response = await Client.PostAsJsonAsync("/todos", NewRequest(title, expiry));
        response.EnsureSuccessStatusCode();

        var location = response.Headers.Location!.ToString();
        return Guid.Parse(location.Split("/").Last());
    }

    private async Task ClearDatabase()
    {
        var scopeFactory = Factory.Services.GetRequiredService<IServiceScopeFactory>();
        
        using var scope = scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ToDoContext>();

        dbContext.Todos.RemoveRange(dbContext.Todos);
        await dbContext.SaveChangesAsync();
    }

    #region GET /todos/ 
    [Fact]
    public async Task GetAllTodos_WhenNoTodosExist_ShouldReturnEmptyList()
    {
        // Act
        await ClearDatabase();
        var response = await Client.GetAsync("/todos");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var todos = await response.Content.ReadFromJsonAsync<List<ToDoDto>>();
        todos.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllTodos_WhenSomeTodosExist_ShouldReturnListWithTodos()
    {
        // Arrange
        await CreateToDo("Todo1", DateTime.UtcNow.AddHours(1));
        await CreateToDo("Todo2", DateTime.UtcNow.AddHours(0));

        // Act
        var response = await Client.GetAsync("/todos");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var todos = await response.Content.ReadFromJsonAsync<List<ToDoDto>>();
        todos.Should().NotBeEmpty();
        todos.Count.Should().BeGreaterThanOrEqualTo(2);

        todos.Should().Contain(x => x.Title == "Todo1");
        todos.Should().Contain(x => x.Title == "Todo2");
    }

    [Fact]
    public async Task GetAllTodos_ResponseShouldHaveExpectedFields()
    {
        // Arrange
        await CreateToDo("TodoWithFields", DateTime.UtcNow.AddHours(1));

        // Act
        var response = await Client.GetAsync("/todos");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var todos = await response.Content.ReadFromJsonAsync<List<ToDoDto>>();

        todos.Should().NotBeEmpty();
        var todo = todos[0];
        
        Assert.NotNull(todo.Id);
        Assert.NotNull(todo.Title);
        Assert.NotNull(todo.Description);
        Assert.NotNull(todo.Expiry);
        Assert.NotNull(todo.PercentComplete);
    }
    #endregion

    #region GET /todos/{id}
    [Fact]
    public async Task GetTodoById_WhenTodoExists_ShouldReturnTodo()
    {
        // Arrange
        var todoId = await CreateToDo("ExistingTodo", DateTime.UtcNow.AddHours(1));

        // Act
        var response = await Client.GetAsync($"/todos/{todoId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var todo = await response.Content.ReadFromJsonAsync<ToDoDto>();
        todo.Should().NotBeNull();
        todo!.Id.Should().Be(todoId);
        todo.Title.Should().Be("ExistingTodo");
    }

    [Fact]
    public async Task GetTodoById_WhenTodoDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var response = await Client.GetAsync($"/todos/{nonExistentId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    #endregion

    #region GET /todos/incoming
    
    [Theory]
    [InlineData(nameof(IncomingScope.Week))]
    [InlineData(nameof(IncomingScope.Tomorrow))]
    [InlineData(nameof(IncomingScope.Today))]
    // TODO: This test doesn't always work correctly — it needs to be refactored so that the application uses TimeProvider.
    public async Task GetIncomingTodos_ShouldReturnTodosWithinScope(string scope)
    {
        // Arrange
        await ClearDatabase();
        await CreateToDo("TodoToday", DateTime.UtcNow.AddMinutes(1));
        await CreateToDo("TodoTomorrow", DateTime.UtcNow.AddDays(1));
        await CreateToDo("TodoWeek", DateTime.UtcNow.AddDays(6));

        // Act
        var response = await Client.GetAsync($"/todos/incoming?scope={scope}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var todos = await response.Content.ReadFromJsonAsync<List<ToDoDto>>();
        todos.Should().NotBeNull();

        switch (scope)
        {
            case "Today":
                todos.Should().ContainSingle(x => x.Title == "TodoToday");
                break;
            case "Tomorrow":
                todos.Should().ContainSingle(x => x.Title == "TodoTomorrow");
                break;
            case "Week":
                todos.Count.Should().Be(2);
                todos.Should().Contain(x => x.Title == "TodoToday");
                todos.Should().Contain(x => x.Title == "TodoTomorrow");
                break;
        }
    }

    #endregion
    
    #region POST /todos/
    [Fact]
    public async Task CreateToDo_WhenRequestIsValid_ShouldCreateToDo()
    {
        // Arrange
        var request = new CreateToDoRequest("NewTodo", "Test description", DateTime.UtcNow.AddDays(1));

        // Act
        var response = await Client.PostAsJsonAsync("/todos", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var location = response.Headers.Location!.ToString();
        location.Should().NotBeNullOrEmpty();

        var createdId = Guid.Parse(location.Split("/").Last());
        createdId.Should().NotBeEmpty();

        var getResponse = await Client.GetAsync($"/todos/{createdId}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var todo = await getResponse.Content.ReadFromJsonAsync<ToDoDto>();
        todo.Should().NotBeNull();
        todo!.Title.Should().Be(request.Title);
        todo.Description.Should().Be(request.Description);
        todo.Expiry.Should().BeCloseTo(request.Expiry, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public async Task CreateToDo_WhenRequestIsInvalid_ShouldReturnBadRequest_TitleIsEmpty()
    {
        // Arrange
        var invalidRequest = new CreateToDoRequest("", "Test description", DateTime.UtcNow.AddDays(1));

        // Act
        var response = await Client.PostAsJsonAsync("/todos", invalidRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task CreateToDo_WhenRequestIsInvalid_ShouldReturnBadRequest_ExpiryDateIsInThePast()
    {
        // Arrange
        var invalidRequest = new CreateToDoRequest("Test", "Test description", DateTime.UtcNow.AddDays(-1));

        // Act
        var response = await Client.PostAsJsonAsync("/todos", invalidRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    #endregion

    #region PUT /todos/{id}
    [Fact]
    public async Task UpdateToDo_WhenRequestIsValid_ShouldUpdateToDo()
    {
        // Arrange
        var todoId = await CreateToDo("OldTitle", DateTime.UtcNow.AddDays(1));
        var updateRequest = new UpdateToDoRequest("UpdatedTitle", "Updated description", DateTime.UtcNow.AddDays(2), 50);

        // Act
        var response = await Client.PutAsJsonAsync($"/todos/{todoId}", updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verify the updated ToDo
        var getResponse = await Client.GetAsync($"/todos/{todoId}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var todo = await getResponse.Content.ReadFromJsonAsync<ToDoDto>();
        todo.Should().NotBeNull();
        todo!.Title.Should().Be(updateRequest.Title);
        todo.Description.Should().Be(updateRequest.Description);
        todo.Expiry.Should().BeCloseTo(updateRequest.Expiry, TimeSpan.FromSeconds(1));
        todo.PercentComplete.Should().Be(updateRequest.PercentComplete);
    }

    [Fact]
    public async Task UpdateToDo_WhenToDoDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();
        var updateRequest = new UpdateToDoRequest("UpdatedTitle", "Updated description", DateTime.UtcNow.AddDays(2), 50);

        // Act
        var response = await Client.PutAsJsonAsync($"/todos/{nonExistentId}", updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateToDo_WhenRequestIsInvalid_ShouldReturnBadRequest()
    {
        // Arrange
        var todoId = await CreateToDo("OldTitle", DateTime.UtcNow.AddDays(1));
        var invalidRequest = new UpdateToDoRequest("", "Updated description", DateTime.UtcNow.AddDays(2), 50); // Empty title

        // Act
        var response = await Client.PutAsJsonAsync($"/todos/{todoId}", invalidRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    #endregion
    
    #region PATCH /todos/{id}/percent
    [Fact]
    public async Task SetPercentComplete_WhenRequestIsValid_ShouldUpdatePercentComplete()
    {
        // Arrange
        var todoId = await CreateToDo("TestTodo", DateTime.UtcNow.AddDays(1));
        var request = new SetPercentCompleteRequest(75);

        // Act
        var response = await Client.PatchAsJsonAsync($"/todos/{todoId}/percent", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verify the updated percent complete
        var getResponse = await Client.GetAsync($"/todos/{todoId}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var todo = await getResponse.Content.ReadFromJsonAsync<ToDoDto>();
        todo.Should().NotBeNull();
        todo!.PercentComplete.Should().Be(request.PercentComplete);
    }

    [Fact]
    public async Task SetPercentComplete_WhenToDoDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();
        var request = new SetPercentCompleteRequest(50);

        // Act
        var response = await Client.PatchAsJsonAsync($"/todos/{nonExistentId}/percent", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task SetPercentComplete_WhenRequestIsInvalid_ShouldReturnBadRequest()
    {
        // Arrange
        var todoId = await CreateToDo("TestTodo", DateTime.UtcNow.AddDays(1));
        var invalidRequest = new SetPercentCompleteRequest(-10); // Invalid percent complete

        // Act
        var response = await Client.PatchAsJsonAsync($"/todos/{todoId}/percent", invalidRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    #endregion

    #region PATCH /todos/{id}/done
    [Fact]
    public async Task MarkAsDone_WhenToDoExists_ShouldMarkAsDone()
    {
        // Arrange
        var todoId = await CreateToDo("TestTodo", DateTime.UtcNow.AddDays(1));

        // Act
        var response = await Client.PatchAsync($"/todos/{todoId}/done", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verify the ToDo is marked as done
        var getResponse = await Client.GetAsync($"/todos/{todoId}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var todo = await getResponse.Content.ReadFromJsonAsync<ToDoDto>();
        todo.Should().NotBeNull();
        todo!.PercentComplete.Should().Be(100);
    }

    [Fact]
    public async Task MarkAsDone_WhenToDoDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var response = await Client.PatchAsync($"/todos/{nonExistentId}/done", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    #endregion
    
    #region DELETE /todos/{id}
    [Fact]
    public async Task DeleteToDo_WhenToDoExists_ShouldDeleteToDo()
    {
        // Arrange
        var todoId = await CreateToDo("TestTodo", DateTime.UtcNow.AddDays(1));

        // Act
        var response = await Client.DeleteAsync($"/todos/{todoId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verify the ToDo is deleted
        var getResponse = await Client.GetAsync($"/todos/{todoId}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteToDo_WhenToDoDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var response = await Client.DeleteAsync($"/todos/{nonExistentId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    #endregion
}
