using System.Text.Json.Serialization;
using FluentValidation;
using Pan.Affiliation.Domain.Modules.Customers.ValueObjects;
using Pan.Affiliation.Domain.Shared;
using Pan.Affiliation.Domain.Shared.Validation;

namespace Pan.Affiliation.Domain.Modules.Customers.Entities;

public class Customer : BaseEntity
{
    private readonly Validator<Customer> _validator;

    private IList<Address> _addresses;
    public IEnumerable<Address> Addresses => _addresses;

    public string? Name { get; private set; }

    [JsonIgnore]
    public DocumentNumber DocumentNumberVo { get; }

    public string DocumentNumber => DocumentNumberVo.ToString();      

    public Customer(
        Guid? id,
        string? name,
        DocumentNumber documentNumberVo,
        IList<Address>? addresses = null)
    {
        Id = id ?? Guid.NewGuid();
        Name = name;
        DocumentNumberVo = documentNumberVo;
        _addresses = addresses ?? new List<Address>();
        _validator = Validator<Customer>.Create();
    }

    public void ChangeName(string name)
    {
        Name = name;
    }

    public void AddAddress(Address address)
    {
        _addresses.Add(address);
    }

    public void RemoveAddress(Guid addressId)
    {
        _addresses = _addresses
            .Where(a => a.Id != addressId)
            .ToList();
    }

    public void ChangeAddress(Address address)
    {
        RemoveAddress(address.Id);
        _addresses.Add(address);
    }

    public override ValidationResult Validate()
    {
        _validator.RuleFor(m => m.Name)
            .NotEmpty();

        var errors = _addresses.Select(a => a.Validate())
            .Where(result => !result.IsValid)
            .SelectMany(result => result.Errors)
            .ToList();

        var merchantValidationResult = _validator.Validate(this);

        if (!merchantValidationResult.IsValid)
        {
            ValidationResult validationResult = merchantValidationResult;

            errors.AddRange(validationResult.Errors);
        }

        return new(errors);
    }
}