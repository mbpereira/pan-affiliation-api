using FluentValidation;

namespace Pan.Affiliation.Domain.Shared.Validation;

public class Validator<T> : AbstractValidator<T>
{
    private Validator()
    {
    }

    public static Validator<T> Create()
        => new();
}