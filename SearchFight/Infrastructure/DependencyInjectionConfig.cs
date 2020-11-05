using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using Infrastructure;
using Infrastructure.Contracts;
using Infrastructure.Implementations;
using Application.Contracts;
using Application.Core;
using Application.Implementations;
using System.Net.Http;

namespace SearchFight.Infrastructure
{
    public static class DependencyInjectionConfig
    {
        public static IConfiguration Configuration { get; set; }

        public static IServiceProvider ServiceProvider { get; set; }

        public static void SetUpApp()
        {
            BuildConfigurationRoot();
            var serviceCollection = BuildServiceCollection();
            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        private static IServiceCollection BuildServiceCollection()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.Configure<SearchProvidersConfig>(Configuration);

            serviceCollection.AddTransient<ISearchEngine, BingService>();
            serviceCollection.AddTransient<ISearchEngine, GoogleService>();

            serviceCollection.AddSingleton<ISearchBuilder, SearchBuilder>();

            serviceCollection.AddTransient<ISearchService, SearchService>();
            serviceCollection.AddTransient<HttpClient>();

            return serviceCollection;
        }

        private static void BuildConfigurationRoot()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .AddEnvironmentVariables()
                .Build();
        }
    }
}
