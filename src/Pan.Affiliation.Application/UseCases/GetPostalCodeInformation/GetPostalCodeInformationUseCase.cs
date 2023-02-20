using Pan.Affiliation.Domain.Localization.Adapters;
using Pan.Affiliation.Domain.Localization.Entities;
using Pan.Affiliation.Domain.ValueObjects;

namespace Pan.Affiliation.Application.UseCases.GetPostalCodeInformation;

public class GetPostalCodeInformationUseCase : IGetPostalCodeInformationUseCase
{
    private readonly IPostalCodeInformationService _service;

    public GetPostalCodeInformationUseCase(IPostalCodeInformationService service)
    {
        _service = service;
    }

    public Task<PostalCodeInformation> ExecuteAsync(PostalCode param)
        =>
            _service.GetPostalCodeInformationAsync(param);
}