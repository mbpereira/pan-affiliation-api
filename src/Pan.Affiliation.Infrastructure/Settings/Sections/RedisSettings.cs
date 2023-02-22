namespace Pan.Affiliation.Infrastructure.Settings.Sections;

public class RedisSettings
{
    public string? Host { get; set; }
    public int Port { get; set; }
    public int DefaultDatabase { get; set; }
}