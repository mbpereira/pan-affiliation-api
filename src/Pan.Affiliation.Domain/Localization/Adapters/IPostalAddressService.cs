using Pan.Affiliation.Domain.Localization.Entities;

namespace Pan.Affiliation.Domain.Localization.Adapters;

public interface IPostalAddressService
{
    Task<PostalAddress> GetPostalCodeInformationAsync(string postalCode);
}