using Newtonsoft.Json;
using Pan.Affiliation.Domain.Shared.Logging;

namespace Pan.Affiliation.Infrastructure.Gateways
{
    public abstract class HttpService
    {
        private readonly ILogger _logger;
        
        protected HttpService(ILogger logger)
        {
            _logger = logger;
        }
        
        protected async Task<T?> DeserializeResponseAsync<T>(HttpResponseMessage response)
        {
            _logger.LogInformation("Request finished with {HttpStatusCode} status code", response.StatusCode);
            
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
