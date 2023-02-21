using Pan.Affiliation.Domain.Logging;
using Pan.Affiliation.Infrastructure.Persistence;

namespace Pan.Affiliation.Application.Services;

public class ApplicationUpdater : IApplicationUpdater
{
    private readonly IDbMigrator _migrator;
    private readonly ILogger<ApplicationUpdater> _logger;
    
    public ApplicationUpdater(IDbMigrator migrator, ILogger<ApplicationUpdater> logger)
    {
        _migrator = migrator;
        _logger = logger;
    }

    public async Task UpdateAsync()
    {
        _logger.LogInformation("Applying migrations");

        await _migrator.MigrateAsync();
        
        _logger.LogInformation("Migrations applyied");
    }
}