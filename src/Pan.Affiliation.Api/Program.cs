using Autofac;
using Autofac.Extensions.DependencyInjection;
using Pan.Affiliation.Domain.Settings;
using Pan.Affiliation.Infrastructure;
using Pan.Affiliation.Infrastructure.Data;
using Pan.Affiliation.Infrastructure.Settings.Sections;
using static Pan.Affiliation.Shared.Constants.Configuration;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

// Add services to the container.
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
    containerBuilder.RegisterModule(new InfrastructureModule(builder.Configuration)));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

{
    using var scope = app.Services.CreateScope();
    var settingsProvider = scope.ServiceProvider.GetRequiredService<ISettingsProvider>();
    var dbSettings = settingsProvider.GetSection<DbSettings>(PanAffiliationDbSettingsKey);

    if (dbSettings!.ApplyMigrationsOnStartup)
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<PanAffiliationDbContext>();
        await dbContext.ApplyMigrationsAsync();
    }
}

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();
