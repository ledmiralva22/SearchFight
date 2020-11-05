using Application.Contracts;
using Application.Core;
using Application.Implementations;
using Infrastructure.Contracts;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Test
{
    public class Tests
    {
        Mock<ISearchEngine> bingService;
        Mock<ISearchEngine> googleService;
        Mock<ISearchBuilder> searchBuilder;
        SearchService searchService;

        public Tests()
        {
            bingService = new Mock<ISearchEngine>();
            bingService.Setup(bs => bs.Name).Returns("Bing");

            googleService = new Mock<ISearchEngine>();
            googleService.Setup(gs => gs.Name).Returns("Google");

            searchBuilder = new Mock<ISearchBuilder>();

            searchService = new SearchService(new[] { bingService.Object, googleService.Object }, searchBuilder.Object);
        }

        [Fact]
        public async Task Should_Build_SearchFightResults()
        {
            googleService.Setup(gp => gp.Search(It.IsAny<string>())).ReturnsAsync(new Int64());
            bingService.Setup(gp => gp.Search(It.IsAny<string>())).ReturnsAsync(new Int64());

            var searchTerms = new[] { ".net", "java" };

            await searchService.RunProcess(searchTerms);

            searchBuilder.Verify(builder => builder.ConstructReport(), Times.Once);
        }
    }
}
