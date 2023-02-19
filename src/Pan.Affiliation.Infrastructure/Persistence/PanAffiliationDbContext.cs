using Microsoft.EntityFrameworkCore;
using Pan.Affiliation.Infrastructure.Persistence.Entities;

namespace Pan.Affiliation.Infrastructure.Persistence
{
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

            if (pendingMigrations?.Any() is true)
            {
                await Database.MigrateAsync();
            }
        }

        public DbSet<CustomerInformation> Customers { get; set; }
        public DbSet<Address> Addresses { get; set; }
    }
}
