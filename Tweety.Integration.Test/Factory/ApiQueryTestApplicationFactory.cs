using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Tweety.Integration.Test.Factory
{
    public class ApiQueryTestApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            // implement later
            /*
            builder.ConfigureTestServices(services => {
                services.RemoveAll<IDbContext>();
                services.TryAddScoped<IDbContext>(sp => new TestDbContext($"use-configuration-to-get-connection-string"));
            });*/
        }
    }
}
