using Newtonsoft.Json;

namespace Pan.Affiliation.Infrastructure.Gateways
{
    public abstract class HttpService
    {
        public async Task<T> DeserializeResponseAsync<T>(HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
