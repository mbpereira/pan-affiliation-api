using Pan.Affiliation.Domain.Settings;
using Pan.Affiliation.Infrastructure.Settings.Sections;

namespace Pan.Affiliation.Infrastructure.Persistence;

public class DatabaseMigrator : IDatabaseMigrator
{
    private readonly PanAffiliationDbContext _context;
    private readonly ISettingsProvider _settingsProvider;

    public DatabaseMigrator(PanAffiliationDbContext context, ISettingsProvider settingsProvider)
    {
        _context = context;
        _settingsProvider = settingsProvider;
    }

    public async Task MigrateAsync()
    {
        var dbSettings = _settingsProvider
            .GetSection<DbSettings>(Constants.PanAffiliationDbSettingsKey);

        if (dbSettings.ApplyMigrationsOnStartup)
            await _context.ApplyMigrationsAsync();
    }
}