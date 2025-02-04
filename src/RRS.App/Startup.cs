using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RRS.App.Extensions;
using RRS.Infrastructure.Extensions;

namespace RRS.App;

public sealed class Startup
{
    private readonly IServiceCollection _services;
    private readonly IConfiguration _configuration;

    public Startup()
    {
        _services = new ServiceCollection();

        _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();
    }

    public Startup ConfigureServices()
    {
        // Configure logging
        _services.AddLogging(builder =>
        {
            builder.ClearProviders();

            builder.AddConfiguration(_configuration.GetSection("Logging"))
                .AddConsole()
                .AddDebug();
        });

        // Configure services
        _services.AddApplicationServices()
            .AddInfrastructureServices();

        // Configure options
        _services.AddSingleton(_configuration);
        _services.AddApplicationOptions();

        // Validation
        _services.AddValidatorsFromAssemblyContaining<Startup>(includeInternalTypes: true);


        return this;
    }

    public IServiceProvider Build() => _services.BuildServiceProvider();
}
