namespace Pan.Affiliation.Infrastructure.Settings.Sections
{
    public class DbSettings
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Port { get; set; }
        public string? Host { get; set; }
        public string? Database { get; set; }
        public bool ApplyMigrationsOnStartup { get; set; }
    }
}
