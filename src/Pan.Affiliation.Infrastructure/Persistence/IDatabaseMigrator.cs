namespace Pan.Affiliation.Infrastructure.Persistence;

public interface IDatabaseMigrator
{
    Task MigrateAsync();
}