using Newtonsoft.Json;
using Pan.Affiliation.Domain.Modules.Localization.Entities;

namespace Pan.Affiliation.Infrastructure.Gateways.Ibge.Contracts
{
    public class StateResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("nome")]
        public string? Name { get; set; }

        [JsonProperty("sigla")]
        public string? Acronym { get; set; }

        internal State ToEntity()
            => new()
            {
                Id = Id,
                Name = Name,
                Acronym = Acronym
            };
    }
}
