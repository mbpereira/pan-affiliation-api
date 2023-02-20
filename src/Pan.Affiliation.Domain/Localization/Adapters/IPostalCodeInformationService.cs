using Pan.Affiliation.Domain.Localization.Entities;
using Pan.Affiliation.Domain.ValueObjects;

namespace Pan.Affiliation.Domain.Localization.Adapters;

public interface IPostalCodeInformationService
{
    Task<PostalCodeInformation?> GetPostalCodeInformationAsync(PostalCode postalCode);
}