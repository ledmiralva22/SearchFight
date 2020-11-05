using Domain.Models;

namespace Application.Contracts
{
    public interface ISearchBuilder
    {
        void AddResultByWord(string word, string searchEngine, long numberOfResults);
        void SetWinnerBySearchEngine();
        void SetProviderWinner();
        SearchResults ConstructReport();
    }
}
