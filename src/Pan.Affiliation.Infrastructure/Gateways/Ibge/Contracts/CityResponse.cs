using Newtonsoft.Json;
using Pan.Affiliation.Domain.Localization.Entities;

namespace Pan.Affiliation.Infrastructure.Gateways.Ibge.Contracts
{
    public class CityResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("nome")]
        public string? Name { get; set; }

        internal City ToEntity() => new()
        {
            Id = Id,
            Name = Name
        };
    }
}
