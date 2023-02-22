using Pan.Affiliation.Domain.Shared.UseCases;

namespace Pan.Affiliation.Domain.Modules.Customers.UseCases.ChangeAddress;

public interface IChangeAddressUseCase : IUseCase<ChangeAddressInput, Domain.Modules.Customers.Entities.Address>
{
}