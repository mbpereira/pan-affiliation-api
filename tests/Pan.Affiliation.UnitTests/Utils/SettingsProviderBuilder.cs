using Microsoft.Extensions.Configuration;
using Pan.Affiliation.Domain.Shared.Settings;
using Pan.Affiliation.Infrastructure.Settings;

namespace Pan.Affiliation.UnitTests.Utils
{
    public class SettingsProviderBuilder
    {
        private readonly ConfigurationBuilder _configuration;
        private readonly IList<KeyValuePair<string, string?>> _configCollection;

        public SettingsProviderBuilder()
        {
            _configuration = new ConfigurationBuilder();
            _configCollection = new List<KeyValuePair<string, string?>>();
        }

        public SettingsProviderBuilder WithEnvironmentVariable(string key, string value)
        {
            Environment.SetEnvironmentVariable(key, value);
            return this;
        }

        public SettingsProviderBuilder WithConfiguration(IConfiguration configuration)
        {
            _configuration.AddConfiguration(configuration);
            return this;
        }

        public SettingsProviderBuilder WithCollection(IEnumerable<KeyValuePair<string, string?>> configCollection)
        {
            _configuration.AddInMemoryCollection(configCollection);
            return this;
        }

        public SettingsProviderBuilder AddConfig(string key, string value)
        {
            _configCollection.Add(new KeyValuePair<string, string?>(key, value));
            return this;
        }

        public ISettingsProvider Build()
            => new SettingsProvider(_configuration
                .AddInMemoryCollection(_configCollection)
                .AddEnvironmentVariables()
                .Build());
    }
}
