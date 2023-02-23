using Autofac;
using Autofac.Extensions.DependencyInjection;
using Pan.Affiliation.Application;
using Pan.Affiliation.Application.Services;
using Pan.Affiliation.Domain;
using Pan.Affiliation.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.UseSerilog();
builder.Logging.ClearProviders();

// Add services to the container.
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
    containerBuilder
        .RegisterModule(new InfrastructureModule(builder.Configuration))
        .RegisterModule(new DomainModule())
        .RegisterModule(new ApplicationModule()));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

{
    var scope = app.Services.CreateScope();
    var agent = scope.ServiceProvider.GetRequiredService<IStartupAgent>();
    
    await  agent.SetupAsync(new(SeedData: args.Contains("--seed")));
}

app.UseSerilogRequestLogging();
// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();