using Pan.Affiliation.Domain.Localization.Entities;
using Pan.Affiliation.Domain.ValueObjects;

namespace Pan.Affiliation.Application.UseCases.GetPostalCodeInformation;

public interface IGetPostalCodeInformationUseCase : IUseCase<PostalCode, PostalCodeInformation>
{
}