namespace Pan.Affiliation.Domain.Modules.Customers.UseCases.ChangeAddress;

public record ChangeAddressInput(Guid CustomerId, Guid addressId, AddressInput Address)
{
    public Domain.Modules.Customers.Entities.Address ToDomainAddress() => new()
    {
        Id = addressId,
        City = Address.City,
        Complement = Address.Complement,
        Country = Address.Country,
        Neighborhood = Address.Neighborhood,
        Number = Address.Number,
        State = Address.State,
        Street = Address.Street,
        PostalCodeVo = Address.PostalCode
    };
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
}