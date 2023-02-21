using Pan.Affiliation.Domain.Shared.Settings;
using Pan.Affiliation.Infrastructure.Settings.Sections;

namespace Pan.Affiliation.Infrastructure.Persistence;

public class DbMigrator : IDbMigrator
{
    private readonly PanAffiliationDbContext _context;
    private readonly ISettingsProvider _settingsProvider;

    public DbMigrator(PanAffiliationDbContext context, ISettingsProvider settingsProvider)
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