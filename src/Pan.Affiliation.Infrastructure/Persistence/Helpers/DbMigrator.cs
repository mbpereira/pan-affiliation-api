namespace Pan.Affiliation.Infrastructure.Persistence.Helpers;

public class DbMigrator : IDbMigrator
{
    private readonly PanAffiliationDbContext _context;

    public DbMigrator(PanAffiliationDbContext context)
    {
        _context = context;
    }

    public async Task MigrateAsync()
    {
        await _context.ApplyMigrationsAsync();
    }
}