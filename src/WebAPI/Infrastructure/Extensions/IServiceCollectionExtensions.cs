using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SB.Infrastructure.ServiceBus.Factories;
using SB.Infrastructure.ServiceBus.Options;
using SB.Infrastructure.ServiceBus.Providers;
using SB.Infrastructure.ServiceBus.Services;
using SB.WebAPI.Persistence;
using SB.WebAPI.Persistence.Repositories;
using System;

namespace SB.WebAPI.Infrastructure.Extensions
{
    /// <summary>
    /// Extensions of IServiceCollection
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomProblemDetails(this IServiceCollection services, IHostEnvironment environment) =>
        services
        .AddProblemDetails(configure =>
        {
            configure.IncludeExceptionDetails = _ => environment.IsDevelopment();
        })
        .Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Instance = context.HttpContext.Request.Path,
                    Status = StatusCodes.Status400BadRequest,
                    Type = $"https://httpstatuses.com/400",
                    Detail = ApiConstants.Messages.ModelStateValidation
                };
                return new BadRequestObjectResult(problemDetails)
                {
                    ContentTypes =
                    {
                                    ApiConstants.ContentTypes.ProblemJson,
                                    ApiConstants.ContentTypes.ProblemXml
                    }
                };
            };
        });

        public static IServiceCollection AddServiceBus(this IServiceCollection services, IConfiguration configuration) =>
            services
                .Configure<ServiceBusOptions>(configuration.GetSection(nameof(ServiceBusOptions)))
                .AddTransient<IServiceBusClientFactory, ServiceBusClientProvider>()
                .AddTransient<IServiceBusService, ServiceBusService>()
                .AddTransient<ITopicRepository, TopicRepository>()
                .AddDbContext<ServiceBusContext>(options =>
                     options.UseSqlServer(configuration.GetConnectionString(nameof(ServiceBusContext)),
                        sqlServerOptionsAction: sqlOptions =>
                        {
                            sqlOptions.MigrationsAssembly(typeof(ServiceBusContext).Assembly.FullName);
                            sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                        }), ServiceLifetime.Scoped);
    }
}
