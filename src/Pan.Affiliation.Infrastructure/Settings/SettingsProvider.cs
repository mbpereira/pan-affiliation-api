using Microsoft.Extensions.Configuration;
using Pan.Affiliation.Domain.Settings;

namespace Pan.Affiliation.Infrastructure.Settings
{
    public class SettingsProvider : ISettingsProvider
    {
        private readonly IConfiguration _configuration;

        public SettingsProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public T? GetSection<T>(string key) where T : class, new()
        {
            var settings = new T();
            var section = _configuration.GetSection(key);

            if (section is null || section.Value is null)
                return null;

            section.Bind(settings);

            return settings;
        }

        public T? GetValue<T>(string key)
            => _configuration.GetValue<T>(key);
    }
}
