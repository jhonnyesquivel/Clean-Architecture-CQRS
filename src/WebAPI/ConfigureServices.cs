using Zemoga_Test.Application.Common.Interfaces;
using Zemoga_Test.Infrastructure.Persistence;
using Zemoga_Test.WebAPI.Filters;
using Zemoga_Test.WebAPI.Services;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using NSwag;
using NSwag.Generation.Processors.Security;
using System.Text.Json.Serialization;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddWebAPIServices(this IServiceCollection services)
    {
        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddScoped<ICurrentUserService, CurrentUserService>();

        services.AddHttpContextAccessor();

        services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>();

        services.AddControllersWithViews(options =>
            options.Filters.Add<ApiExceptionFilterAttribute>())
                .AddFluentValidation(x => x.AutomaticValidationEnabled = false);

        services.AddMvcCore().AddJsonOptions(opts =>
        {
            opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        // Customise default API behaviour
        services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);

        services.AddOpenApiDocument(configure =>
        {
            configure.Title = "Zemoga_Test API";
            configure.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
            {
                Type = OpenApiSecuritySchemeType.ApiKey,
                Name = "Authorization",
                In = OpenApiSecurityApiKeyLocation.Header,
                Description = "Type into the textbox: Bearer {your JWT token}."
            });

            configure.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
        });
        

        return services;
    }
}
