using Pan.Affiliation.Domain.Shared.Validation;

namespace Pan.Affiliation.Domain.Shared;

public abstract class BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public abstract ValidationResult Validate();
}