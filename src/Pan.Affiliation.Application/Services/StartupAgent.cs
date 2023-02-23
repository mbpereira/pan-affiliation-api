using Pan.Affiliation.Domain.Shared.Logging;
using Pan.Affiliation.Domain.Shared.Settings;
using Pan.Affiliation.Infrastructure.Persistence;
using Pan.Affiliation.Infrastructure.Persistence.Helpers;
using Pan.Affiliation.Infrastructure.Settings.Sections;

namespace Pan.Affiliation.Application.Services;

public record StartupSettings(bool SeedData);

public class StartupAgent : IStartupAgent
{
    private readonly IDbSeeder _seeder;
    private readonly IDbMigrator _migrator;
    private readonly ILogger<StartupAgent> _logger;
    private readonly ISettingsProvider _settingsProvider;

    public StartupAgent(
        IDbMigrator migrator,
        ILogger<StartupAgent> logger,
        IDbSeeder seeder, ISettingsProvider settingsProvider)
    {
        _migrator = migrator;
        _logger = logger;
        _seeder = seeder;
        _settingsProvider = settingsProvider;
    }

    public async Task SetupAsync(StartupSettings settings)
    {
        if (settings.SeedData)
        {
            await SeedAsync();
            return;
        }

        await MigrateAsync(forceMigration: false);
    }

    private async Task SeedAsync()
    {
        _logger.LogInformation("Seeding fake data");

        await MigrateAsync(forceMigration: true);

        await _seeder.SeedAsync();

        _logger.LogInformation("Finished seeding process");
    }

    private async Task MigrateAsync(bool forceMigration)
    {
        var dbSettings = _settingsProvider
            .GetSection<DbSettings>(Constants.PanAffiliationDbSettingsKey);

        if (!dbSettings.ApplyMigrationsOnStartup && !forceMigration)
            return;

        _logger.LogInformation("Applying migrations");

        await _migrator.MigrateAsync();

        _logger.LogInformation("Migrations applyied");
    }
}