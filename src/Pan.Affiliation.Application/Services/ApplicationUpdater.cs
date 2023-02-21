using Pan.Affiliation.Domain.Shared.Logging;
using Pan.Affiliation.Domain.Shared.Settings;
using Pan.Affiliation.Infrastructure.Persistence;
using Pan.Affiliation.Infrastructure.Persistence.Helpers;
using Pan.Affiliation.Infrastructure.Settings.Sections;

namespace Pan.Affiliation.Application.Services;

public class ApplicationUpdater : IApplicationUpdater
{
    private readonly IDbSeeder _seeder;
    private readonly IDbMigrator _migrator;
    private readonly ILogger<ApplicationUpdater> _logger;
    private readonly ISettingsProvider _settingsProvider;
    
    public ApplicationUpdater(
        IDbMigrator migrator, 
        ILogger<ApplicationUpdater> logger, 
        IDbSeeder seeder, ISettingsProvider settingsProvider)
    {
        _migrator = migrator;
        _logger = logger;
        _seeder = seeder;
        _settingsProvider = settingsProvider;
    }

    public async Task SeedAsync()
    {
        _logger.LogInformation("Seeding fake data");

        await MigrateAsync();

        await _seeder.SeedAsync();
        
        _logger.LogInformation("Finished seeding process");
    }
    
    public async Task UpdateAsync()
    {
        var dbSettings = _settingsProvider
            .GetSection<DbSettings>(Constants.PanAffiliationDbSettingsKey);

        if (!dbSettings.ApplyMigrationsOnStartup)
            return;
        
        await MigrateAsync();
    }

    private async Task MigrateAsync()
    {
        _logger.LogInformation("Applying migrations");

        await _migrator.MigrateAsync();

        _logger.LogInformation("Migrations applyied");
    }
}