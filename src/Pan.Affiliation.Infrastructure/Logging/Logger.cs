using Microsoft.Extensions.Logging;

namespace Pan.Affiliation.Infrastructure.Logging;

public static class Constants
{
    public const string LoggingSettingsKey = "Logging";
}

public class Logger<T> : Domain.Logging.ILogger<T>
{
    private readonly ILogger<T> _logger;

    public Logger(ILogger<T> logger)
    {
        _logger = logger;
    }

    public IDisposable? BeginScope(IDictionary<string, object> scope)
        => _logger.BeginScope(scope);

    public void LogInformation(string message, params object[] args)
        => _logger.LogInformation(message, args);

    public void LogWarning(string message, params object[] args)
        => _logger.LogWarning(message, args);

    public void LogError(string message, params object[] args)
        => _logger.LogError(message, args);

    public void LogDebug(string message, params object[] args)
        => _logger.LogDebug(message, args);

    public void LogCritical(string message, params object[] args)
        => _logger.LogCritical(message, args);

    public void LogInformation(Exception ex, string message, params object[] args)
        => _logger.LogInformation(ex, message, args);

    public void LogWarning(Exception ex, string message, params object[] args)
        => _logger.LogWarning(ex, message, args);

    public void LogError(Exception ex, string message, params object[] args)
        => _logger.LogError(ex, message, args);

    public void LogDebug(Exception ex, string message, params object[] args)
        => _logger.LogDebug(ex, message, args);

    public void LogCritical(Exception ex, string message, params object[] args)
        => _logger.LogCritical(ex, message, args);
}