using System.Collections.Generic;

namespace Infrastructure
{
    public class SearchProvidersConfig
    {
        public Dictionary<string, SearchProviderConfig> SearchProvidersConfigurations { get; set; }
    }

    public class SearchProviderConfig
    {
        public string Url { get; set; }
        public string ApiKey { get; set; }
    }
}
