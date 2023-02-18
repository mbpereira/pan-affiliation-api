using Microsoft.EntityFrameworkCore;

namespace Pan.Affiliation.Infrastructure.Data
{
    public class PanAffiliationDbContext : DbContext
    {
        public PanAffiliationDbContext()
        {
        }

        public PanAffiliationDbContext(DbContextOptions<DbContext> opt)
            : base(opt) { }

        public async Task ApplyMigrationsAsync()
        {
            var pendingMigrations = await Database.GetPendingMigrationsAsync();

            if (pendingMigrations?.Any() is true)
            {
                await Database.MigrateAsync();
            }
        }
    }
}
