using System.Net;
using System.Net.Security;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Pan.Affiliation.Application;
using Pan.Affiliation.Domain.Settings;
using Pan.Affiliation.Infrastructure;
using Pan.Affiliation.Infrastructure.Persistence;
using Pan.Affiliation.Infrastructure.Settings.Sections;
using Serilog;
using Serilog.Events;
using static Pan.Affiliation.Shared.Constants.Configuration;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

// Add services to the container.
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
    containerBuilder
        .RegisterModule(new InfrastructureModule(builder.Configuration))
        .RegisterModule(new ApplicationModule()));

builder.Host.UseSerilog(((_, provider, loggerConfig) =>
{
    var settingsProvider = provider.GetRequiredService<ISettingsProvider>();
    var settings = settingsProvider.GetSection<LogSettings>(LoggingSettingsKey);

    loggerConfig
        .WriteTo.NewRelicLogs(licenseKey: settings.NewRelicSettings?.LicenseKey,
            applicationName: settings.NewRelicSettings?.ApplicationName)
        .WriteTo.Console()
        .WriteTo.File(settings.LogFile!)
        .Enrich.FromLogContext()
        .Enrich.WithEnvironmentName()
        .Enrich.WithMachineName()
        .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning);
}));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


{
    using var scope = app.Services.CreateScope();

    var settingsProvider = scope
        .ServiceProvider
        .GetRequiredService<ISettingsProvider>();

    var dbSettings = settingsProvider
        .GetSection<DbSettings>(PanAffiliationDbSettingsKey);

    if (dbSettings!.ApplyMigrationsOnStartup)
    {
        var dbContext = scope
            .ServiceProvider
            .GetRequiredService<PanAffiliationDbContext>();

        await dbContext.ApplyMigrationsAsync();
    }
}

app.UseSerilogRequestLogging();
// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();