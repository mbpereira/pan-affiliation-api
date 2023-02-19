using Microsoft.Extensions.Configuration;

namespace Pan.Affiliation.UnitTests.Utils
{
    public class CustomConfigurationBuilder
    {
        private readonly ConfigurationBuilder _configuration;
        private readonly IList<KeyValuePair<string, string?>> _configCollection;

        public CustomConfigurationBuilder()
        {
            _configuration = new ConfigurationBuilder();
            _configCollection = new List<KeyValuePair<string, string?>>();
        }

        public CustomConfigurationBuilder WithEnvironmentVariable(string key, string value)
        {
            Environment.SetEnvironmentVariable(key, value);
            return this;
        }

        public CustomConfigurationBuilder WithConfiguration(IConfiguration configuration)
        {
            _configuration.AddConfiguration(configuration);
            return this;
        }

        public CustomConfigurationBuilder WithCollection(IEnumerable<KeyValuePair<string, string?>> configCollection)
        {
            _configuration.AddInMemoryCollection(configCollection);
            return this;
        }

        public CustomConfigurationBuilder AddConfig(string key, string value)
        {
            _configCollection.Add(new KeyValuePair<string, string?>(key, value));
            return this;
        }

        public IConfiguration Build()
            => _configuration
                .AddInMemoryCollection(_configCollection)
                .AddEnvironmentVariables()
                .Build();
    }
}
