using Bogus;
using FluentAssertions;
using NSubstitute;
using Pan.Affiliation.Infrastructure.Caching;
using StackExchange.Redis;

namespace Pan.Affiliation.UnitTests.Pan.Affiliation.Infrastructure.Caching;

public class RedisCacheProviderTests
{
    private readonly IConnectionMultiplexer _multiplexer;
    private readonly IDatabase _database;
    private readonly Faker _faker = new();

    public RedisCacheProviderTests()
    {
        _multiplexer = Substitute.For<IConnectionMultiplexer>();
        _database = Substitute.For<IDatabase>();

        _multiplexer.GetDatabase()
            .Returns(_database);
    }

    [Fact]
    public async Task When_GetManyAsync_called_should_return_null_if_key_does_not_exists()
    {
        _database.SetMembersAsync(Arg.Any<RedisKey>())
            .Returns(Task.FromResult<RedisValue[]>(null));
        var provider = new RedisCacheProvider(_multiplexer);

        var result = await provider.GetManyAsync<object>(_faker.Random.Word());

        await _database.Received().SetMembersAsync(Arg.Any<RedisKey>());
        result.Should().BeNull();
    }

    [Fact]
    public async Task When_GetAsync_called_should_return_default_generic_type_value_if_key_does_not_exists()
    {
        _database.StringGetAsync(Arg.Any<RedisKey>())
            .Returns(Task.FromResult<RedisValue>(string.Empty));
        var provider = new RedisCacheProvider(_multiplexer);

        var result = await provider.GetAsync<object>(_faker.Random.Word());

        await _database.Received().StringGetAsync(Arg.Any<RedisKey>());
        result.Should().BeNull();
    }

    [Fact]
    public async Task When_RemoveAsync_called_should_delete_and_return_true_if_key_was_removed()
    {
        _database.KeyDeleteAsync(Arg.Any<RedisKey>())
            .Returns(Task.FromResult(true));
        var provider = new RedisCacheProvider(_multiplexer);

        var result = await provider.RemoveAsync(_faker.Random.Word());

        await _database.Received().KeyDeleteAsync(Arg.Any<RedisKey>());
        result.Should().BeTrue();
    }

    [Fact]
    public async Task When_SaveManyAsync_called_should_add_to_cache_and_return_saved_record_number()
    {
        var persons = new List<object>
        {
            new
            {
                Name = _faker.Person.FirstName
            }
        };
        _database.SetAddAsync(Arg.Any<RedisKey>(), Arg.Any<RedisValue[]>())
            .Returns(Task.FromResult<long>(1));
        var provider = new RedisCacheProvider(_multiplexer);

        var result = await provider.SaveManyAsync(_faker.Random.Word(), persons);

        await _database
            .Received()
            .SetAddAsync(Arg.Any<RedisKey>(), Arg.Any<RedisValue[]>());
        result.Should().Be(persons.Count);
    }

    [Fact]
    public async Task When_SaveAsync_called_should_add_to_cache_and_return_true()
    {
        var person = new
        {
            Name = _faker.Person.FirstName
        };
        _database.StringSetAsync(
                Arg.Any<RedisKey>(),
                Arg.Any<RedisValue>(),
                Arg.Any<TimeSpan>())
            .Returns(Task.FromResult(true));
        var provider = new RedisCacheProvider(_multiplexer);

        var result = await provider.SaveAsync(_faker.Random.Word(), person, TimeSpan.Zero);

        await _database.Received().StringSetAsync(
            Arg.Any<RedisKey>(),
            Arg.Any<RedisValue>(),
            Arg.Any<TimeSpan>());
        result.Should().BeTrue();
    }
}