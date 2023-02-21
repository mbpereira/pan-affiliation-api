namespace Pan.Affiliation.Domain.Shared.Logging;

public interface ILogger
{
    IDisposable? BeginScope(IDictionary<string, object> scope);
    void LogInformation(string message, params object[] args);
    void LogWarning(string message, params object[] args);
    void LogError(string message, params object[] args);
    void LogDebug(string message, params object[] args);
    void LogCritical(string message, params object[] args);
    void LogInformation(Exception ex, string message, params object[] args);
    void LogWarning(Exception ex, string message, params object[] args);
    void LogError(Exception ex, string message, params object[] args);
    void LogDebug(Exception ex, string message, params object[] args);
    void LogCritical(Exception ex, string message, params object[] args);
}