namespace Pan.Affiliation.Infrastructure.Persistence;

public interface IDbMigrator
{
    Task MigrateAsync();
}