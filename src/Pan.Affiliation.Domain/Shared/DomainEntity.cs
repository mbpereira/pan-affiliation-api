using System.ComponentModel.DataAnnotations;

namespace Pan.Affiliation.Domain.Shared;

public abstract class DomainEntity
{
    public abstract ValidationResult Validate();
}