using Pan.Affiliation.Domain.Modules.Localization.Entities;
using Pan.Affiliation.Domain.Shared.ValueObjects;

namespace Pan.Affiliation.Domain.Modules.Localization.Gateways;

public interface IPostalCodeInformationGatewayService
{
    Task<PostalCodeInformation?> GetPostalCodeInformationAsync(PostalCode postalCode);
}