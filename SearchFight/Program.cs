using Application.Contracts;
using SearchFight.Infrastructure;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace SearchFight
{
    class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("No words were entered");
                return;
            }

            DependencyInjectionConfig.SetUpApp();

            var searchFightService = DependencyInjectionConfig.ServiceProvider.GetService<ISearchService>();

            var searchFightResults = await searchFightService.RunProcess(args);

            foreach (var resultByWord in searchFightResults.ResultsByWord)
            {
                var sb = new StringBuilder();
                sb.Append($"{resultByWord.Key}: ");
                foreach (var searchEngineResult in resultByWord.Value)
                {
                    sb.Append($"{searchEngineResult.Key}: {searchEngineResult.Value} ");
                }
                Console.WriteLine(sb.ToString());
            }

            Console.WriteLine();

            foreach(var providerWinner in searchFightResults.WinnerByProvider)
            {
                Console.WriteLine($"{providerWinner.Key} Winner: {providerWinner.Value}");
            }

            Console.WriteLine($"\nTotal Winner: {searchFightResults.ProviderWinner}");
            Console.ReadLine();
        }
    }
}
