
using Bogus;
using FluentAssertions;
using Pan.Affiliation.Infrastructure.Settings.Sections;
using Pan.Affiliation.UnitTests.Utils;

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
            var provider = new SettingsProviderBuilder()
                .WithEnvironmentVariable(envName, envValue.ToString())
                .Build();

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
            var provider = new SettingsProviderBuilder()
                .WithEnvironmentVariable(envName, envValue)
                .Build();

            // Act
            var result = provider.GetSection<DbSettings>("PanAffiliationDatabaseSettings");

            // Assert
            result.Should().BeEquivalentTo(new DbSettings
            {
                Username = envValue
            });
        }

        [Fact]
        public void When_GetSection_is_called_should_return_default_object_props_if_configuration_was_not_found()
        {
            // Arrange
            var provider = new SettingsProviderBuilder()
                .Build();

            // Act
            var result = provider.GetSection<DbSettings>("PanAffiliationDatabaseSettings");

            // Assert
            result.Should().BeEquivalentTo(new DbSettings());
        }
    }
}
