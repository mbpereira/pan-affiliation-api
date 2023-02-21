using Pan.Affiliation.Domain.Shared.Settings;
using Pan.Affiliation.Infrastructure.Settings.Sections;

namespace Pan.Affiliation.Infrastructure.Persistence.Helpers;

public class DbMigrator : IDbMigrator
{
    private readonly PanAffiliationDbContext _context;

    public DbMigrator(PanAffiliationDbContext context, ISettingsProvider settingsProvider)
    {
        _context = context;
    }

    public async Task MigrateAsync()
    {
        await _context.ApplyMigrationsAsync();
    }
}