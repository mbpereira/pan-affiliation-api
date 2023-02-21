namespace Pan.Affiliation.Infrastructure.Settings.Sections;

public class LogSettings
{
    public string? LogFile { get; set; }
    public NewRelicSettings? NewRelicSettings { get; set; }
}

public class NewRelicSettings
{
    public string? ApplicationName { get; set; } = string.Empty;
    public string? LicenseKey { get; set; } = string.Empty;
}