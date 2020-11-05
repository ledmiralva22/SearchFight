using Application.Contracts;
using Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace Application.Core
{
    public class SearchBuilder: ISearchBuilder
    {
        Dictionary<string, string> winnersByProvider = new Dictionary<string, string>();
        List<ResultsByWord> resultsByWords = new List<ResultsByWord>();
        string providerWinner;

        public void AddResultByWord(string word, string searchEngine, long numberOfResults)
        {
            ResultsByWord resultsByWord = new ResultsByWord 
            {
                Word = word,
                SearchEngine = searchEngine,
                NumberOfResults = numberOfResults
            };
            resultsByWords.Remove(resultsByWord);
            resultsByWords.Add(resultsByWord);
        }

        public void SetWinnerBySearchEngine()
        {
            var resultsBySearchEngine = resultsByWords
                .GroupBy(result => result.SearchEngine)
                .SelectMany(r => r.Where(w => w.NumberOfResults == r.Max(n => n.NumberOfResults)).Take(1));
            foreach(var result in resultsBySearchEngine)
            {
                AddWinnerByProvider(result.Word, result.SearchEngine);
            }
        }

        public void SetProviderWinner()
        {
            providerWinner = resultsByWords
                .GroupBy(result => result.Word)
                .Select(g => new 
                { 
                    Word = g.Key, 
                    Total = g.Sum(st => st.NumberOfResults) 
                })
                .Aggregate((max, next) => max.Total > max.Total ? max: next)
                .Word;
        }

        public SearchResults ConstructReport()
        {
            return new SearchResults
            {
                ResultsByWord = SetResultsByWord(),
                WinnerByProvider = winnersByProvider,
                ProviderWinner = providerWinner
            };
        }

        private void AddWinnerByProvider(string word, string searchEngine)
        {
            winnersByProvider.Remove(searchEngine);
            winnersByProvider.Add(searchEngine, word);
        }

        private Dictionary<string, Dictionary<string, long>> SetResultsByWord()
        {
            return resultsByWords
                .GroupBy(result => result.Word)
                .ToDictionary(
                    k => k.Key,
                    v => v.GroupBy(se => se.SearchEngine)
                          .ToDictionary(dd => dd.Key, vv => vv.First().NumberOfResults));
        }
    }
}
