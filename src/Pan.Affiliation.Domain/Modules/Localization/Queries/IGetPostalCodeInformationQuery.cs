using Pan.Affiliation.Domain.Modules.Customers.ValueObjects;
using Pan.Affiliation.Domain.Modules.Localization.Entities;

namespace Pan.Affiliation.Domain.Modules.Localization.Queries;

public interface IGetPostalCodeInformationQuery
{
    Task<PostalCodeInformation?> GetPostalCodeInformationAsync(PostalCode postalCode);
}