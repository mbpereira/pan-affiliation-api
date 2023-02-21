using FluentValidation;
using Pan.Affiliation.Domain.Shared;
using Pan.Affiliation.Domain.Shared.Validation;

namespace Pan.Affiliation.Domain.Modules.Merchant.Entities;

public class CustomerInformation : BaseEntity
{
    private readonly Validator<CustomerInformation> _validator;

    private IList<Address> _addresses;
    public IEnumerable<Address> Addresses => _addresses;

    public string Name { get; private set;  }

    public CustomerInformation(Guid? id, string name, IList<Address>? addresses = null)
    {
        Id = id ?? Guid.NewGuid();
        Name = name;
        _addresses = addresses ?? new List<Address>();
        _validator = Validator<CustomerInformation>.Create();
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