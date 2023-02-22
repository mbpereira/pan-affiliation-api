using Pan.Affiliation.Domain.Modules.Customers.ValueObjects;
using Pan.Affiliation.Domain.Modules.Localization.Entities;
using Pan.Affiliation.Domain.Shared.UseCases;

namespace Pan.Affiliation.Domain.Modules.Localization.UseCases;

public interface IGetPostalCodeInformationUseCase : IUseCase<PostalCode, PostalCodeInformation?>
{
}