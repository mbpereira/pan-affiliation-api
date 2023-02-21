using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pan.Affiliation.Domain.Settings;
using Pan.Affiliation.Infrastructure.Caching;
using Pan.Affiliation.Infrastructure.Persistence;
using Pan.Affiliation.Infrastructure.Settings;
using Pan.Affiliation.Infrastructure.Settings.Sections;
using Pan.Affiliation.Shared.Constants;
using Redbox.Serilog.Stackdriver;
using Serilog;
using Serilog.Enrichers.Span;
using Serilog.Formatting.Compact;
using StackExchange.Redis;
using static Pan.Affiliation.Shared.Constants.Configuration;

namespace Pan.Affiliation.Infrastructure
{
    public class InfrastructureModule : Module
    {
        private readonly ISettingsProvider _settingsProvider;
        private readonly IServiceCollection _services;

        public InfrastructureModule(IConfiguration configuration)
        {
            _settingsProvider = new SettingsProvider(configuration);
            _services = new ServiceCollection();
        }

        protected override void Load(ContainerBuilder builder)
        {
            _services.AddDbContext<PanAffiliationDbContext>(builder =>
                builder.UseNpgsql(GetConnectionString(),
                    b => b.MigrationsAssembly(GetMigrationsAssembly())));

            AddHttpClient(HttpClientConfiguration.IbgeClient);
            AddHttpClient(HttpClientConfiguration.ViaCepClient);
            AddSerilog();

            builder.Populate(_services);

            builder
                .RegisterInstance(_settingsProvider)
                .SingleInstance();

            RegisterRedis(builder);
            
            builder.RegisterGeneric(typeof(Logging.Logger<>))
                .As(typeof(Domain.Logging.ILogger<>))
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(InfrastructureModule).Assembly)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }

        private void RegisterRedis(ContainerBuilder builder)
        {
            var settings = _settingsProvider.GetSection<RedisSettings>(RedisCacheProvider.Constants.SettingsKey); 
            builder.Register(_ => 
                ConnectionMultiplexer.Connect(settings.Host!))
                .As<IConnectionMultiplexer>()
                .SingleInstance();
        }

        private void AddSerilog()
        {
            _services.AddLogging(loggingBuilder =>
            {
                var settings = _settingsProvider.GetSection<LogSettings>(LoggingSettingsKey);

                var logger = new LoggerConfiguration()
                    .Enrich.FromLogContext()
                    .Enrich.WithEnvironmentName()
                    .Enrich.WithMachineName()
                    .Enrich.WithClientAgent()
                    .Enrich.WithClientIp()
                    .Enrich.WithCorrelationId()
                    .Enrich.WithTraceIdentifier()
                    .Enrich.WithSpan()
                    .WriteTo.Console()
                    .WriteTo.NewRelicLogs(licenseKey: settings.NewRelicSettings?.LicenseKey,
                        applicationName: settings.NewRelicSettings?.ApplicationName)
                    .WriteTo.File(settings.LogFile!)
                    .CreateLogger();
                
                loggingBuilder.ClearProviders();
                loggingBuilder.AddSerilog(logger);
            });
        }

        private void AddHttpClient(string httpClientIdentifier)
        {
            _services.AddHttpClient(httpClientIdentifier)
                .SetHandlerLifetime(TimeSpan.FromHours(1));
        }

        private static string? GetMigrationsAssembly()
            => typeof(PanAffiliationDbContext).Assembly.FullName;

        private string GetConnectionString()
        {
            var settings = _settingsProvider.GetSection<DbSettings>(PanAffiliationDbSettingsKey);

            return string.Format(PgConnectionString,
                settings!.Host,
                settings!.Username,
                settings!.Password,
                settings!.Database);
        }
    }
}