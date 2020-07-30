using Microsoft.AspNetCore.Mvc.Testing;

namespace WebAPI.IntegrationTests
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<Startup>
    {
    }
}