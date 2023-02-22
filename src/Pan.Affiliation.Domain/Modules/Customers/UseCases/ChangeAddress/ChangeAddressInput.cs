using Pan.Affiliation.Domain.Modules.Customers.Entities;

namespace Pan.Affiliation.Domain.Modules.Customers.UseCases.ChangeAddress;

public record ChangeAddressInput(Guid CustomerId, Guid AddressId, AddressInput Address)
{
    public Address ToDomainAddress()
        => Address.ToDomainEntity(AddressId);
}

public record AddressInput
{
    public string? PostalCode { get; set; }

    public string? Street { get; set; }

    public int? Number { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? Country { get; set; }

    public string? Complement { get; set; }

    public string? Neighborhood { get; set; }

    public Address ToDomainEntity(Guid? id = null) =>
        new()
        {
            Id = id ?? Guid.NewGuid(),
            City = City,
            Complement = Complement,
            Country = Country,
            Neighborhood = Neighborhood,
            Number = Number,
            State = State,
            Street = Street,
            PostalCodeVo = PostalCode
        };
}