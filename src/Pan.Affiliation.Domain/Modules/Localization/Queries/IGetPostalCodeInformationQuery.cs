using Pan.Affiliation.Domain.Modules.Customers.ValueObjects;
using Pan.Affiliation.Domain.Modules.Localization.Entities;
using Pan.Affiliation.Domain.Shared.ValueObjects;

namespace Pan.Affiliation.Domain.Modules.Localization.Queries;

public interface IGetPostalCodeInformationQuery
{
    Task<PostalCodeInformation?> GetPostalCodeInformationAsync(PostalCode postalCode);
}