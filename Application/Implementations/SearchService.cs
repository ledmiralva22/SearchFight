using Application.Contracts;
using Application.Core;
using Domain.Models;
using Infrastructure.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Implementations
{
    public class SearchService: ISearchService
    {
        private readonly IEnumerable<ISearchEngine> searchProviders;
        private readonly ISearchBuilder searchBuilder;

        public SearchService(IEnumerable<ISearchEngine> searchProviders, ISearchBuilder searchBuilder)
        {
            this.searchProviders = searchProviders;
            this.searchBuilder = searchBuilder;
        }
        
        public async Task<SearchResults> RunProcess(IEnumerable<string> searchWords)
        {
            searchWords = searchWords.Distinct();
            
            foreach(var searchProvider in searchProviders)
            {
                foreach(var searchWord in searchWords)
                {
                    var searchResult = await searchProvider.Search(searchWord);
                    searchBuilder.AddResultByWord(searchWord, searchProvider.Name, searchResult);
                }
                searchBuilder.SetWinnerBySearchEngine();
            }
            searchBuilder.SetProviderWinner();
            return searchBuilder.ConstructReport();
        }
    }
}
