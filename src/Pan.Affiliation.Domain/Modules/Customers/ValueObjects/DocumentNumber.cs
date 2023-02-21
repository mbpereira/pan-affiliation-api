using FluentValidation;
using Pan.Affiliation.Domain.Modules.Customers.Enums;
using Pan.Affiliation.Domain.Shared.Validation;
using Pan.Affiliation.Domain.Shared.ValueObjects;
using Pan.Affiliation.Shared.Extensions;

namespace Pan.Affiliation.Domain.Modules.Customers.ValueObjects;

public record DocumentNumber : ValueObject
{
    private bool _isValid;
    public string? OriginalValue { get; }
    public string? Value { get; }
    public DocumentType? DocumentType { get; private set; }

    public DocumentNumber(string? documentNumber)
    {
        OriginalValue = documentNumber;
        Value = documentNumber?.OnlyNumbers();
        Validate();
    }

    private void Validate()
    {
        if (string.IsNullOrEmpty(Value))
        {
            _isValid = false;
            return;
        }

        var validator = Validator<string>.Create();

        var documentType = Value.Length > 11
            ? Enums.DocumentType.Cnpj
            : Enums.DocumentType.Cpf;

        validator
            .When(
                predicate: _ => documentType == Enums.DocumentType.Cnpj,
                action: () => validator.RuleFor(document => document).IsValidCNPJ())
            .Otherwise(
                action: () => validator.RuleFor(document => document).IsValidCPF());

        var result = validator.Validate(Value);

        if (result.IsValid)
        {
            DocumentType = documentType;
            _isValid = true;
        }
    }

    public override bool IsValid()
        => _isValid;

    public static implicit operator DocumentNumber(string? documentNumber) =>
        new(documentNumber);
}