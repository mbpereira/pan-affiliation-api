using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pan.Affiliation.Domain.Settings;
using Pan.Affiliation.Infrastructure.Persistence;
using Pan.Affiliation.Infrastructure.Settings;
using Pan.Affiliation.Infrastructure.Settings.Sections;
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

            builder.Populate(services);

            builder.RegisterInstance(_settingsProvider).SingleInstance();
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
