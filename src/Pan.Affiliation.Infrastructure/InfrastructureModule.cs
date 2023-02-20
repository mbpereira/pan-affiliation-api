using System.Net;
using System.Security.Authentication;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pan.Affiliation.Domain.Logging;
using Pan.Affiliation.Domain.Settings;
using Pan.Affiliation.Infrastructure.Logging;
using Pan.Affiliation.Infrastructure.Persistence;
using Pan.Affiliation.Infrastructure.Settings;
using Pan.Affiliation.Infrastructure.Settings.Sections;
using Pan.Affiliation.Shared.Constants;
using static Pan.Affiliation.Shared.Constants.Configuration;

namespace Pan.Affiliation.Infrastructure
{
    public class InfrastructureModule : Module
    {
        private readonly ISettingsProvider _settingsProvider;

        public InfrastructureModule(IConfiguration configuration)
        {
            _settingsProvider = new SettingsProvider(configuration);
        }

        protected override void Load(ContainerBuilder builder)
        {
            var services = new ServiceCollection();
            services.AddDbContext<PanAffiliationDbContext>(builder =>
                builder.UseNpgsql(GetConnectionString(),
                    b => b.MigrationsAssembly(GetMigrationsAssembly())));

            AddHttpClient(services, HttpClientConfiguration.IbgeClient);
            AddHttpClient(services, HttpClientConfiguration.ViaCepClient);

            services.AddHttpClient(HttpClientConfiguration.ViaCepClient);

            builder.Populate(services);

            builder
                .RegisterInstance(_settingsProvider)
                .SingleInstance();

            builder.RegisterGeneric(typeof(Logger<>))
                .As(typeof(ILogger<>))
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(InfrastructureModule).Assembly)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }

        private static void AddHttpClient(ServiceCollection services, string httpClientIdentifier)
        {
            services.AddHttpClient(httpClientIdentifier)
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