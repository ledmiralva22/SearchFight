using System.Collections.Generic;

namespace Domain.Models
{
    public class SearchResults
    {
        public Dictionary<string, Dictionary<string, long>> ResultsByWord { get; set; }
        public Dictionary<string, string> WinnerByProvider { get; set; }
        public string ProviderWinner { get; set; }
    }
}
