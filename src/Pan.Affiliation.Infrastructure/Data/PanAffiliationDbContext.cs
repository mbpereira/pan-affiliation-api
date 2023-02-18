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
    }
}
