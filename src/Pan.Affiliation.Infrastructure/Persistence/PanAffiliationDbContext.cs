using Microsoft.EntityFrameworkCore;
using Pan.Affiliation.Infrastructure.Persistence.Entities;

namespace Pan.Affiliation.Infrastructure.Persistence;

public static class Constants
{
    public const string PanAffiliationDbSettingsKey = "PanAffiliationDbSettings";
    public const string PgConnectionString = "Host={0};Username={1};Password={2};Database={3}";
}
    
public class PanAffiliationDbContext : DbContext
{
    public PanAffiliationDbContext()
    {
    }

    public PanAffiliationDbContext(DbContextOptions<PanAffiliationDbContext> opt)
        : base(opt) { }

    public async Task ApplyMigrationsAsync()
    {
        var pendingMigrations = await Database.GetPendingMigrationsAsync();

        if (pendingMigrations.Any())
        {
            await Database.MigrateAsync();
        }
    }

    public DbSet<Customer>? Customers { get; set; }
    public DbSet<Address>? Addresses { get; set; }
}