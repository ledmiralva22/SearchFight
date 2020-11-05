using Infrastructure.Contracts;
using Infrastructure.Models;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Implementations
{
    public class BingService: ISearchEngine
    {
        private readonly HttpClient httpClient;
        private readonly SearchProviderConfig searchProviderConfig;

        public string Name => "Bing";

        public BingService(HttpClient httpClient, IOptions<SearchProvidersConfig> searchProvidersConfig)
        {
            this.httpClient = httpClient;
            searchProviderConfig = searchProvidersConfig.Value.SearchProvidersConfigurations[Name];
        }

        public async Task<long> Search(string searchRequest)
        {
            httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", searchProviderConfig.ApiKey);
            var requestUrl = new Uri($"{searchProviderConfig.Url}?q={searchRequest}");
            using(var response = await httpClient.GetAsync(requestUrl))
            {
                if(!response.IsSuccessStatusCode)
                {
                    throw new Exception($"An error produced when trying to obtain the results from {Name} Search Engine");
                }

                var stringResponse = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                var deserializedResult = JsonSerializer.Deserialize<BingResponse>(stringResponse, options);

                return deserializedResult.WebPages.TotalEstimatedMatches;
            }
        }
    }
}
