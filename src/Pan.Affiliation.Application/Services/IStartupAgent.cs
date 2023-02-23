namespace Pan.Affiliation.Application.Services;

public interface IStartupAgent
{
    Task SetupAsync(StartupSettings settings);
}