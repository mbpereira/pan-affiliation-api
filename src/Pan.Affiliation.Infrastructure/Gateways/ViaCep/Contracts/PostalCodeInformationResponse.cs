using Newtonsoft.Json;
using Pan.Affiliation.Domain.Localization.Entities;

namespace Pan.Affiliation.Infrastructure.Gateways.ViaCep.Contracts;

public class PostalCodeInformationResponse
{
    [JsonProperty("cep")]
    public string PostalCode { get; set; }
    
    [JsonProperty("logradouro")]
    public string Street { get; set; }
    
    [JsonProperty("bairro")]
    public string Neighborhood { get; set; }
    
    [JsonProperty("uf")]
    public string State { get; set; }
    
    [JsonProperty("localidade")]
    public string City { get; set; }

    internal PostalCodeInformation ToEntity() => new()
    {
        PostalCode = PostalCode,
        Street = Street,
        Neighborhood = Neighborhood,
        State = State,
        City = City,
    };
}