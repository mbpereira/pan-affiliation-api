namespace Pan.Affiliation.Domain.Shared.ValueObjects;

public abstract record ValueObject
{
    public abstract bool IsValid();
}