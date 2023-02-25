namespace Todos.IntegrationTests;

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Todos.Application.Interfaces;
using Todos.Infrastructure.Repositories;
using Todos.Infrastructure.Settings;
using Todos.WebUI;

[ExcludeFromCodeCoverage]
public class IntegrationTestWebApplicationFactory : WebApplicationFactory<Program>
{
    public ITodoListRepository TodoListRepository { get; private set; }
    public ITodoItemRepository TodoItemRepository { get; private set; }
    
    /// <summary>
    /// Gets or sets the application settings override.
    /// </summary>
    public string AppSettingsOverride { get; set; }
    
    /// <summary>
    /// Creates the host with the bootstrapped application in the builder.
    /// </summary>
    /// <param name="builder">The host builder used to create the host.</param>
    /// <returns>The host with the bootstrapped application.</returns>
    protected override IHost CreateHost(IHostBuilder builder)
    {
        ConfigureHostConfiguration(builder);
        return base.CreateHost(builder);
    }
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.AddSingleton<ITodoListRepository, TodoListRepository>();
            services.AddSingleton<ITodoItemRepository, TodoItemRepository>();
                
            ConfigureSettings(services);

            var serviceProvider = services.BuildServiceProvider();
                
            TodoListRepository = serviceProvider.GetService<ITodoListRepository>();
            TodoItemRepository = serviceProvider.GetService<ITodoItemRepository>();
        });
    }

    private static void ConfigureSettings(IServiceCollection services)
    {
        var config = GetIConfigurationRoot();
            
        services.Configure<DatabaseSettings>(opts =>
        {
            opts.ConnectionString = config["DatabaseSettings:ConnectionString"];
            opts.DatabaseName = config["DatabaseSettings:DatabaseName"];
        });
    }
    
    private static IConfigurationRoot GetIConfigurationRoot()
    {            
        return new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.IntegrationTest.json", optional: true)
            .AddEnvironmentVariables()
            .Build();
    }
    
    private void ConfigureHostConfiguration(IHostBuilder builder)
    {
        if (string.IsNullOrEmpty(AppSettingsOverride))
            return;

        builder.ConfigureHostConfiguration(config =>
        {
            var bytes = Encoding.UTF8.GetBytes(AppSettingsOverride);
            var stream = new MemoryStream(bytes);
            config.AddJsonStream(stream);
        });
    }
}