using Microsoft.AspNetCore.Mvc.Testing;

namespace ToDoApp.Tests.Integration;

public abstract class IntegrationTestBase : IClassFixture<TestApplicationFactory>
{
    protected readonly HttpClient Client;

    public IntegrationTestBase(TestApplicationFactory factory)
    {
        Client = factory.CreateClient();
    }
}