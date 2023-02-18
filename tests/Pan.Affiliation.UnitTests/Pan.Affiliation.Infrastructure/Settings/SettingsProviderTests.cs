
using Bogus;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Pan.Affiliation.Infrastructure.Settings;
using Pan.Affiliation.Infrastructure.Settings.Sections;

namespace Pan.Affiliation.UnitTests.Pan.Affiliation.Infrastructure.Settings
{
    public class SettingsProviderTests
    {
        private readonly Faker _faker = new();

        [Fact]
        public void When_GetValue_is_called_should_return_parsed_value()
        {
            // Arrange
            var envName = "LOG";
            var envValue = true;
            Environment.SetEnvironmentVariable(envName, envValue.ToString());
            var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();
            var provider = new SettingsProvider(configuration);

            // Act
            var result = provider.GetValue<bool>(envName);

            // Assert
            result.Should().Be(envValue);
        }

        [Fact]
        public void When_GetSection_is_called_should_return_parsed_value()
        {
            // Arrange
            var envName = "PanAffiliationDatabaseSettings__Username";
            var envValue = _faker.Random.Word();
            Environment.SetEnvironmentVariable(envName, envValue.ToString());
            var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();
            var provider = new SettingsProvider(configuration);

            // Act
            var result = provider.GetSection<DbSettings>("PanAffiliationDatabaseSettings");

            // Assert
            result.Should().BeEquivalentTo(new DbSettings
            {
                Username = envValue
            });
        }

        [Fact]
        public void When_GetSection_is_called_should_return_null_if_configuration_was_not_found()
        {
            // Arrange
            var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();
            var provider = new SettingsProvider(configuration);

            // Act
            var result = provider.GetSection<DbSettings>("PanAffiliationDatabaseSettings");

            // Assert
            result.Should().BeNull();
        }
    }
}
