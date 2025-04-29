using ToDoApp.Tests.Integration;

namespace ToDoApp.Tests.Common;

public abstract class IntegrationTestBase : IClassFixture<TestApplicationFactory>
{
    protected TestApplicationFactory Factory { get; private set; }
    protected HttpClient Client { get; private set; }

    public IntegrationTestBase(TestApplicationFactory factory)
    {
        Factory = factory;
        Client = Factory.CreateClient();
    }
}