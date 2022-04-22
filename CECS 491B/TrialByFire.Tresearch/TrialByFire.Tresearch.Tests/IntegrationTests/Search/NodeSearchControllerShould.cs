using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Managers.Implementations;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Services.Implementations;
using TrialByFire.Tresearch.WebApi.Controllers.Contracts;
using TrialByFire.Tresearch.WebApi.Controllers.Implementations;
using Xunit;

namespace TrialByFire.Tresearch.Tests.IntegrationTests.Search
{
    public class NodeSearchControllerShould : TestBaseClass
    {
        private INodeSearchController _nodeSearchController;
        public NodeSearchControllerShould() : base(new string[] { })
        {
            TestServices.AddScoped<INodeSearchService, NodeSearchService>();
            TestServices.AddScoped<INodeSearchManager, NodeSearchManager>();
            TestServices.AddScoped<INodeSearchController, NodeSearchController>();
            TestProvider = TestServices.BuildServiceProvider();
            _nodeSearchController = TestProvider.GetService<INodeSearchController>();
        }

        [Theory]
        [MemberData(nameof(SearchInputData))]
        public async Task GetTheSearchResultsAsync(string search, List<string> tags, bool filterByRating, bool filterByTime,
            IResponse<IList<Node>> expected)
        {
            // Arrange

            // Act
            ActionResult<IEnumerable<Node>> response = await _nodeSearchController.SearchForNodeAsync(search, tags, filterByRating, filterByTime).ConfigureAwait(false);

            // Assert
            Assert.Equal(expected.Data, response.Value);
        }

        public static IEnumerable<object[]> SearchInputData()
        {
            Node node1 = new Node("0B1CC9CFB7380E8E7A80726D12CB997C936D95B514E7F921187119FD80996BBACA103C08EFCC39553EFF5DFC368D4D8D197C9080C7015AE4DA2E87884E7DE9A6", 1, 1, "Cooking", "This is a test node.", new DateTime(2022, 4, 17), true, false);
            Node node2 = new Node("0B1CC9CFB7380E8E7A80726D12CB997C936D95B514E7F921187119FD80996BBACA103C08EFCC39553EFF5DFC368D4D8D197C9080C7015AE4DA2E87884E7DE9A6", 2, 1, "Cooking Pasta", "This is a test node.", new DateTime(2022, 4, 18), true, false);
            Node node3 = new Node("0B1CC9CFB7380E8E7A80726D12CB997C936D95B514E7F921187119FD80996BBACA103C08EFCC39553EFF5DFC368D4D8D197C9080C7015AE4DA2E87884E7DE9A6", 3, 1, "Cooking Rice", "This is a test node.", new DateTime(2022, 4, 19), true, false);
            Node node4 = new Node("D8FC97AC79D370FC43BE4528C72B02AD7B560DC707956B77D5892504754E6C2484C07BF28243FF3CD1A2EA6F778BBBF924B384A34975D6A7D590A40CEE455A32", 4, 1, "Cooking", "This is a test node.", new DateTime(2022, 4, 17), true, false);
            Node node5 = new Node("D8FC97AC79D370FC43BE4528C72B02AD7B560DC707956B77D5892504754E6C2484C07BF28243FF3CD1A2EA6F778BBBF924B384A34975D6A7D590A40CEE455A32", 5, 1, "Cooking Pasta", "This is a test node.", new DateTime(2022, 4, 18), true, false);
            Node node6 = new Node("D8FC97AC79D370FC43BE4528C72B02AD7B560DC707956B77D5892504754E6C2484C07BF28243FF3CD1A2EA6F778BBBF924B384A34975D6A7D590A40CEE455A32", 6, 1, "Cooking Rice", "This is a test node.", new DateTime(2022, 4, 19), true, false);
            Node node7 = new Node("AE57D4CD0E7DC14F7C8C7EEF4DC8C8B833567A71021C1D123328D9B85C3825D8B72376D162C7F03C78D3CE048104A6BB0047979544F4852679D937048258558D", 7, 1, "Cooking", "This is a test node.", new DateTime(2022, 4, 19), true, false);
            Node node8 = new Node("E5D6801551E6079FCAF2B10403FA86F9B9EC40B0D7A70256EDA0A9988ABAB4CC250681D5054D18E224DCF0CADB730BCF6E07546F2B775A0E31D64C3DC41BC159", 8, 1, "Cooking", "This is a test node.", new DateTime(2022, 4, 19), true, false);
            Node node9 = new Node("AE57D4CD0E7DC14F7C8C7EEF4DC8C8B833567A71021C1D123328D9B85C3825D8B72376D162C7F03C78D3CE048104A6BB0047979544F4852679D937048258558D", 9, 1, "Cooking Pasta", "This is a test node.", new DateTime(2022, 4, 19), true, false);

            node1.Tags = new List<INodeTag> { new NodeTag(1, "cooking"), new NodeTag(1, "food") };
            node2.Tags = new List<INodeTag> { new NodeTag(2, "cooking"), new NodeTag(2, "food") };
            node3.Tags = new List<INodeTag> { };
            node4.Tags = new List<INodeTag> { };
            node5.Tags = new List<INodeTag> { new NodeTag(5, "cooking") };
            node6.Tags = new List<INodeTag> { new NodeTag(6, "cooking") };
            node7.Tags = new List<INodeTag> { new NodeTag(7, "cooking") };
            node8.Tags = new List<INodeTag> { new NodeTag(8, "cooking"), new NodeTag(8, "food") };
            node9.Tags = new List<INodeTag> { new NodeTag(9, "cooking"), new NodeTag(9, "food") };

            node1.RatingScore = 15;
            node2.RatingScore = 15;
            node3.RatingScore = 5;
            node4.RatingScore = 5;
            node7.RatingScore = 15;
            node8.RatingScore = 15;
            node9.RatingScore = 15;

            return new[]
            {
                //1
                //exact match, no tags
                new object[] {"cooking", new List<string>{}, false, false,
                    new SearchResponse<IList<Node>>("", new List<Node>
                    {
                        //exact match
                        node1,
                        node4,
                        node7,
                        node8,
                        //not exact match
                        node2,
                        node3,
                        node5,
                        node6,
                        node9,
                    }, 200, true) },
                //2
                //rating, exact match, no tags
                new object[] {"cooking", new List<string>{}, true, false,
                    new SearchResponse<IList<Node>>("", new List<Node>
                    {
                        //highest rating, exact match
                        node1,
                        node7,
                        node8,
                        //highest rating, not exact match
                        node2,
                        node9,
                        //medium rating, exact match
                        node4,
                        //medium rating, not exact match
                        node3,
                        //no rating, not exact match
                        node5,
                        node6,
                    }, 200, true) },
                //3
                //time, exact match, no tags
                new object[] {"cooking", new List<string>{}, false, true,
                    new SearchResponse<IList<Node>>("", new List<Node>
                    {
                        //newest, exact match
                        node7,
                        node8,
                        //newest, not exact match
                        node3,
                        node6,
                        node9,
                        //medium newest, not exact match
                        node2,
                        node5,
                        //oldest, exact match
                        node1,
                        node4,
                    }, 200, true) },
                //4
                //time, rating, exact match, no tags
                new object[] {"cooking", new List<string>{}, true, true,
                    new SearchResponse<IList<Node>>("", new List<Node>
                    {
                        //newest, highest rating, exact match 
                        node7,
                        node8,
                        //newest, highest rating, not exact match
                        node9,
                        //newest, medium rating, not exact match
                        node3,
                        //newest, lowest rating, not exact match
                        node6,
                        //medium newest, medium rating, not exact match
                        node2,
                        //medium newest, lowest rating, not exact match
                        node5,
                        //oldest, highest rating, exact match
                        node1,
                        //oldest, medium rating, exact match
                        node4
                    }, 200, true) },
                //5
                //exact match, tag score
                new object[] {"cooking", new List<string>{ "cooking", }, false, false,
                    new SearchResponse<IList<Node>>("", new List<Node>
                        {
                            //exact match, highest tag score
                            node7,
                            //exact match, medium tag score
                            node1,
                            node8,
                            //exact match, lowest tag score
                            node4,
                            //not exact match, highest tag score
                            node5,
                            node6,
                            //not exact match, medium tag score
                            node2,
                            node9,
                            //not exact match, lowest tag score
                            node3
                        }, 200, true) },
                //6
                //rating, exact match, tag score
                new object[] {"cooking", new List<string>{ "cooking", }, true, false,
                    new SearchResponse<IList<Node>>("", new List<Node>
                        {
                            //highest rating, exact match, highest tag score
                            node7,
                            //highest rating, exact match, medium tag score
                            node1,
                            node8,
                            //highest rating, not exact match, medium tag score
                            node2,
                            node9,
                            //medium rating, exact match, lowest tag score
                            node4,
                            //medium rating, not exact match, lowest tag score
                            node3,
                            //no rating, not exact match, highest tag score
                            node5,
                            node6,
                        }, 200, true) },
                //7
                //time, exact match, tag score
                new object[] {"cooking", new List<string>{ "cooking", }, false, true,
                    new SearchResponse<IList<Node>>("", new List<Node>
                        {
                            //newest, exact match, highest tag score
                            node7,
                            //newest, exact match, medium tag score
                            node8,
                            //newest, not exact match, highest tag score
                            node6,
                            //newest, not exact match, medium tag score
                            node9,
                            //newest, not exact match, lowest tag score
                            node3,
                            //medium newest, not exact match, highest tag score
                            node5,
                            //medium newest, not exact match, medium tag score
                            node2,
                            //oldest, exact match, medium tag score
                            node1,
                            //oldest, exact match, lowest tag score
                            node4,
                        }, 200, true) },
                //8
                //time, rating, exact match, tag score
                new object[] {"cooking", new List<string>{ "cooking", }, true, true,
                    new SearchResponse<IList<Node>>("", new List<Node>
                    {
                        //newest, highest rating, exact match, highest tag score 
                        node7,
                        //newest, highest rating, exact match, medium tag score 
                        node8,
                        //newest, highest rating, not exact match, medium tag score
                        node9,
                        //newest, medium rating, not exact match, lowest tag score
                        node3,
                        //newest, lowest rating, not exact match, highest tag score
                        node6,
                        //medium newest, medium rating, not exact match, medium tag score
                        node2,
                        //medium newest, lowest rating, not exact match, highest tag score
                        node5,
                        //oldest, highest rating, exact match, medium tag score
                        node1,
                        //oldest, medium rating, exact match, lowest tag score
                        node4
                    }, 200, true) },
                //9
                //exact match, tag score
                new object[] {"cooking", new List<string>{ "cooking", "food", }, false, false,
                    new SearchResponse<IList<Node>>("", new List<Node>
                        {
                            //exact match, highest tag score
                            node1,
                            node8,
                            //exact match, medium tag score
                            node7,
                            //exact match, lowest tag score
                            node4,
                            //not exact match, highest tag score
                            node2,
                            node9,
                            //not exact match, medium tag score
                            node5,
                            node6,
                            //not exact match, lowest tag score
                            node3

                        }, 200, true) },
                //10
                //rating, exact match, tag score
                new object[] {"cooking", new List<string>{ "cooking", "food", }, true, false,
                    new SearchResponse<IList<Node>>("", new List<Node>
                        {
                            //highest rating, exact match, highest tag score
                            node1,
                            node8,
                            //highest rating, exact match, medium tag score
                            node7,
                            //highest rating, not exact match, highest tag score
                            node2,
                            node9,
                            //medium rating, exact match, lowest tag score
                            node4,
                            //medium rating, not exact match, lowest tag score
                            node3,
                            //no rating, not exact match, medium tag score
                            node5,
                            node6,
                        }, 200, true) },
                //11
                //time, exact match, tag score
                new object[] {"cooking", new List<string>{ "cooking", "food", }, false, true,
                    new SearchResponse<IList<Node>>("", new List<Node>
                        {
                            //newest, exact match, highest tag score
                            node8,
                            //newest, exact match, medium tag score
                            node7,
                            //newest, not exact match, highest tag score
                            node9,
                            //newest, not exact match, medium tag score
                            node6,
                            //newest, not exact match, lowest tag score
                            node3,
                            //medium newest, not exact match, highest tag score
                            node2,
                            //medium newest, not exact match, medium tag score
                            node5,
                            //oldest, exact match, highest tag score
                            node1,
                            //oldest, exact match, lowest tag score
                            node4,
                        }, 200, true) },
                //12
                //time, rating, exact match, tag score
                new object[] {"cooking", new List<string>{ "cooking", "food", }, true, true,
                    new SearchResponse<IList<Node>>("", new List<Node>
                    {
                        //newest, highest rating, exact match, highest tag score 
                        node8,
                        //newest, highest rating, exact match, medium tag score 
                        node7,
                        //newest, highest rating, not exact match, highest tag score
                        node9,
                        //newest, medium rating, not exact match, lowest tag score
                        node3,
                        //newest, lowest rating, not exact match, medium tag score
                        node6,
                        //medium newest, medium rating, not exact match, highest tag score
                        node2,
                        //medium newest, lowest rating, not exact match, medium tag score
                        node5,
                        //oldest, highest rating, exact match, highest tag score
                        node1,
                        //oldest, medium rating, exact match, lowest tag score
                        node4
                    }, 200, true) },
            };
        }
    }
}