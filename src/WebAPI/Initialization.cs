using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SB.Infrastructure.ServiceBus.Services;
using System.Threading.Tasks;

namespace SB.WebAPI
{
    /// <summary>
    /// API Initialization
    /// </summary>
    public static class Initialization
    {
        /// <summary>
        /// Configure services
        /// </summary>
        /// <param name="app">Application Builder</param>
        // This method can be called on Startup to Synchronize ServiceBus with the Database
        public static async Task SynchronizeServiceBusAsync(IApplicationBuilder app)
        {
            var service = app.ApplicationServices.GetService<IServiceBusService>();
            await service.SynchronizeServiceBusWithDatabaseAsync();
        }
    }
}
