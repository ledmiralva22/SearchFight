using Infrastructure.Contracts;
using System.Net.Http;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System;
using System.Text.Json;
using Infrastructure.Models;

namespace Infrastructure.Implementations
{
    public class GoogleService : ISearchEngine
    {
        private readonly HttpClient httpClient;
        private readonly SearchProviderConfig searchProviderConfig;

        public string Name => "Google";

        public GoogleService(HttpClient httpClient, IOptions<SearchProvidersConfig> searchProvidersConfig)
        {
            this.httpClient = httpClient;
            searchProviderConfig = searchProvidersConfig.Value.SearchProvidersConfigurations[Name];
        }

        public async Task<long> Search(string searchRequest)
        {
            var requestUrl = new Uri($"{searchProviderConfig.Url}&q={searchRequest}&key={searchProviderConfig.ApiKey}");
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

                var deserializedResult = JsonSerializer.Deserialize<GoogleResponse>(stringResponse, options);

                return Convert.ToInt64(deserializedResult.SearchInformation.TotalResults);
            }
        }
    }
}
