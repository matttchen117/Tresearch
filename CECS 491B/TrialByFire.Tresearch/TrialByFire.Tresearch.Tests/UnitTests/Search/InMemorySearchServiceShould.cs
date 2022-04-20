using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using Xunit;

namespace TrialByFire.Tresearch.Tests.UnitTests.Search
{
    public class InMemorySearchServiceShould : TestBaseClass
    {
        private ISearchService _searchService { get; }
        public InMemorySearchServiceShould() : base(new string[] { })
        {
            TestServices.AddScoped<ISqlDAO, InMemorySqlDAO>();
            TestServices.AddScoped<ISearchService, SearchService>();
            TestProvider = TestServices.BuildServiceProvider();
            _searchService = TestProvider.GetService<ISearchService>();
        }

        [Theory]
        public async Task GetSearchResults(string search)
        {
            // Arrange

            // Act
            SearchResponse response = _searchService.SearchAsync(search);

            // Assert
            foreach (Node n in response.Nodes)
            {
                Assert.True(n.NodeTitle.contains(search));
            }
        }
    }
}
