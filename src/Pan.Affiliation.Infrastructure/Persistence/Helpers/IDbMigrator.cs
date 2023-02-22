namespace Pan.Affiliation.Infrastructure.Persistence.Helpers;

public interface IDbMigrator
{
    Task MigrateAsync();
}