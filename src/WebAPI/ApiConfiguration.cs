namespace SB.WebAPI
{
    using FluentValidation.AspNetCore;
    using Hellang.Middleware.ProblemDetails;
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Newtonsoft.Json;
    using System;

    public static class ApiConfiguration
    {
        public static IServiceCollection ConfigureServices(IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            return services
                .AddCustomProblemDetails(environment)
                .AddVersionedApiExplorer()
                .AddApiVersioning(setup =>
                {
                    setup.DefaultApiVersion = new ApiVersion(1, 0);
                    setup.AssumeDefaultVersionWhenUnspecified = true;
                })
                .AddServiceBus(configuration)
                .AddMvcCore()
                .AddAuthorization()
                .AddApplicationPart(typeof(ApiConfiguration).Assembly)
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                })
                .AddApiExplorer()
                .AddFluentValidation()
                .Services;
        }

        public static IApplicationBuilder Configure(
            IApplicationBuilder app,
            Func<IApplicationBuilder, IApplicationBuilder> preConfigure,
            Func<IApplicationBuilder, IApplicationBuilder> postConfigure)
        {
            var applicationBuilder = preConfigure(app)
                .UseProblemDetails()
                .UseRouting();

            return postConfigure(applicationBuilder)
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Home}/{action=Index}/{id?}");
                });
        }
    }
}
