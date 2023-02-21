using FluentValidation;
using Pan.Affiliation.Domain.Shared;
using Pan.Affiliation.Domain.Shared.Validation;
using Pan.Affiliation.Domain.Shared.ValueObjects;

namespace Pan.Affiliation.Domain.Modules.Merchant.Entities;

public class Address : BaseEntity
{
    private readonly Validator<Address> _validator;
    
    public PostalCode? PostalCode { get; set; }

    public string? Street { get; set; }

    public int? Number { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? Country { get; set; }

    public string? Complement { get; set; }

    public string? Neighborhood { get; set; }

    public Address()
    {
        _validator = Validator<Address>.Create();
    }
    
    public override ValidationResult Validate()
    {
        _validator.RuleFor(a => a.PostalCode)
            .Must(code => code.IsValid());
        
        _validator.RuleFor(a => a.Street)
            .Length(min: 3, max: 300);
        
        _validator.RuleFor(a => a.City)
            .Length(min: 3, max: 100);
        
        _validator.RuleFor(a => a.State)
            .Length(exactLength: 2);    
        
        _validator.RuleFor(a => a.Country)
            .Length(min: 3, max: 50);
        
        _validator.RuleFor(a => a.Neighborhood)
            .Length(min: 3, max: 150);
        
        return _validator.Validate(this);
    }
}