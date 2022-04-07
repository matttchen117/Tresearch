using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace TrialByFire.Tresearch.Tests.IntegrationTests.Tag
{
    public class SqlDAOShould : TestBaseClass
    {
        public SqlDAOShould() : base(new string[] { })
        {
            TestProvider = TestServices.BuildServiceProvider();
        }

        public async Task AddTagToNodes(List<long> nodeIDs, string tagName)
        {

        }

        public static IEnumerable<object[]> AddNodeTagData()
        {
            //User owns all tags, nodes already have tag
            var userCase0 = "82336d2e39f058bbc65703caf7247c47a8362279f88f39f5e60ed125485adcf0ad6f6ced311e432f7a10491717f74101d6281540ab6073977853263035f0c62b";
            var roleCase0 = "user";
            var tagNameCase0 = "music";
            var nodeListCase0 = new List<long> { 67890, 67891, 67892 };
            var resultCase0 = "200: Server: success";

            //User owns all tags, nodes do not contain these tags already
            var userCase1 = "82336d2e39f058bbc65703caf7247c47a8362279f88f39f5e60ed125485adcf0ad6f6ced311e432f7a10491717f74101d6281540ab6073977853263035f0c62b";
            var roleCase1 = "user";
            var tagNameCase1 = "art";
            var nodeListCase1 = new List<long> { 67890, 67891, 67892 };
            var resultCase1 = "200: Server: success";

            // User owns tag, node doesn't already contain tag
            var userCase2 = "82336d2e39f058bbc65703caf7247c47a8362279f88f39f5e60ed125485adcf0ad6f6ced311e432f7a10491717f74101d6281540ab6073977853263035f0c62b";
            var roleCase2 = "user";
            var tagNameCase2 = "cooking";
            var nodeListCase2 = new List<long> { 67890 };
            var resultCase2 = "200: Server: success";

            //No node is passed in
            var userCase3 = "82336d2e39f058bbc65703caf7247c47a8362279f88f39f5e60ed125485adcf0ad6f6ced311e432f7a10491717f74101d6281540ab6073977853263035f0c62b";
            var roleCase3 = "user";
            var tagNameCase3 = "baking";
            var nodeListCase3 = new List<long> { };
            var resultCase3 = "204: No nodes passed in.";

            //User does not own node and is trying to make changes
            var userCase4 = "92336d2e39f058bbc65703caf7247c47a8362279f88f39f5e60ed125485adcf0ad6f6ced311e432f7a10491717f74101d6281540ab6073977853263035f0c62b";
            var roleCase4 = "user";
            var tagNameCase4 = "art";
            var nodeListCase4 = new List<long> { 67890 };
            var resultCase4 = "403: Database: You are not authorized to perform this operation.";

            //User does not own all nodes and is trying to make changes (they only own first 2)
            var userCase5 = "82336d2e39f058bbc65703caf7247c47a8362279f88f39f5e60ed125485adcf0ad6f6ced311e432f7a10491717f74101d6281540ab6073977853263035f0c62b";
            var roleCase5 = "user";
            var tagNameCase5 = "baking";
            var nodeListCase5 = new List<long> { 67890, 67891, 67897 };     //user does not own 67897
            var resultCase5 = "403: Database: You are not authorized to perform this operation.";

            //Users role is not valid
            var userCase6 = "82336d2e39f058bbc65703caf7247c47a8362279f88f39f5e60ed125485adcf0ad6f6ced311e432f7a10491717f74101d6281540ab6073977853263035f0c62b";
            var roleCase6 = "role";         // This role is invalid
            var tagNameCase6 = "baking";
            var nodeListCase6 = new List<long> { 67890 };
            var resultCase6 = "400: Server: Unknown role used.";

            return new[]
            {
                new object[] { userCase0, roleCase0, tagNameCase0, resultCase0, nodeListCase0 },
                new object[] { userCase1, roleCase1, tagNameCase1, resultCase1, nodeListCase1 },
                new object[] { userCase2, roleCase2, tagNameCase2, resultCase2, nodeListCase2 },
                new object[] { userCase3, roleCase3, tagNameCase3, resultCase3, nodeListCase3 },
                new object[] { userCase4, roleCase4, tagNameCase4, resultCase4, nodeListCase4 },
                new object[] { userCase5, roleCase5, tagNameCase5, resultCase5, nodeListCase5 },
                new object[] { userCase6, roleCase6, tagNameCase6, resultCase6, nodeListCase6 }
            };
        }
    }
}
