namespace Todos.WebUI;

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Todos.Infrastructure;
using Todos.Infrastructure.Settings;

[ExcludeFromCodeCoverage]
public class Program
{
    /// <summary>
    /// Defines the entry point of the application.
    /// </summary>
    /// <param name="args">The arguments.</param>
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        ConfigureServices(builder);
        ConfigureSettings(builder);
        
        var app = builder.Build();
        ConfigureApplication(app);
    }

    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers()
            .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            );
        
        builder.Services.AddHttpClient();
        
        builder.Services.AddInternalServices();
    }
    
    
    private static void ConfigureSettings(WebApplicationBuilder builder)
    {
        builder.Services.AddOptions<DatabaseSettings>().Configure<IConfiguration>((settings, configuration) =>
            configuration.GetSection(nameof(DatabaseSettings)).Bind(settings));
    }
    
    /// <summary>
    /// Configures the application.
    /// </summary>
    /// <param name="app">The application.</param>
    private static void ConfigureApplication(WebApplication app)
    {
        if (app.Environment.IsDevelopment()) app.UseDeveloperExceptionPage();
        
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}