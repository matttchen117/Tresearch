using Dapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Managers.Implementations;
using TrialByFire.Tresearch.Models;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Services.Implementations;
using Xunit;

namespace TrialByFire.Tresearch.Tests.IntegrationTests.Tag
{
    public class TagManagerShould: TestBaseClass, IClassFixture<TagManagerDatabaseFixture>
    {
        public static TagManagerDatabaseFixture fixture { get; set; }
       
        private static List<long> nodeids = new List<long>();
        private static List<long> NodeIDs
        {
            get { return nodeids; }
            set { nodeids = value; }
        }

        public TagManagerShould(TagManagerDatabaseFixture fixture) : base(new string[] { })
        {
            TagManagerShould.fixture = fixture;
            NodeIDs = fixture.nodeIDs;
            TestServices.AddScoped<ITagService, TagService>();
            TestServices.AddScoped<IAccountVerificationService, AccountVerificationService>();
            TestServices.AddScoped<ITagManager, TagManager>();    
            TestProvider = TestServices.BuildServiceProvider();          
        }

        /// <summary>
        /// Retrieves list of nodeids matching indices.
        /// </summary>
        /// <param name="indexes">Indices of return nodeIDs</param>
        /// <returns></returns>
        public List<long> GetNodes(List<int> indexes)
        {
            List<long> ids = new List<long>();
            foreach (int index in indexes)
                ids.Add(NodeIDs[index]);
            return ids;
        }

        [Theory]
        [MemberData(nameof(AddNodeTagData))]
        public async Task AddTagToNodeAsync(IRoleIdentity roleIdentity, List<int> index, string tagName, IMessageBank.Responses response)
        {
            //Arrange
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);

            Thread.CurrentPrincipal = rolePrincipal;

            IMessageBank messageBank = TestProvider.GetService<IMessageBank>();
            ITagManager tagManager = TestProvider.GetService<ITagManager>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            string expected = await messageBank.GetMessage(response);

            //Act
            string result = await tagManager.AddTagToNodesAsync(GetNodes(index), tagName, cancellationTokenSource.Token);

            //Arrange
            Assert.Equal(expected, result);
        }

        

        public static IEnumerable<object[]> AddNodeTagData()
        {
            /**
             *  Case 0: Tag nodes. User is Authenticated and Authorized. Nodes exist and already contain tag. 
             *      NodeIDs:                    xxxx0, xxxx1, xxxx2
             *      Tag Name:                   Tresearch Manager Add Tag1 
             *      
             *      Result:                     "200: Server: Tag added to node(s)."
             */
            IRoleIdentity roleIdentity0 = new RoleIdentity(true, "tagManagerIntegration1@tresearch.system", "user", "d9e22e6b5668fe3bc85246df7aee535f65cc3fdcd95d468993136da4a35e2f4ac1052c667064368236a0f6a120771aa6f6e332d73215df7339a727e1d32cd648");
            var tagNameCase0 = "Tresearch Manager Add Tag1";
            var nodeCase0 = new List<int> { 0, 1, 2 };
            var resultCase0 = IMessageBank.Responses.tagAddSuccess;

            /**
             *  Case 1: Tag nodes. User is Authenticated and Authorized. Nodes exist and do not contain tag.
             *      NodeIDs:                    xxxx0, xxxx1, xxxx2
             *      Tag Name:                   Tresearch Manager Add Tag2 
             *      
             *      Result:                     "200: Server: Tag added to node(s)."
             */
            IRoleIdentity roleIdentity1 = new RoleIdentity(true, "tagManagerIntegration1@tresearch.system", "user", "d9e22e6b5668fe3bc85246df7aee535f65cc3fdcd95d468993136da4a35e2f4ac1052c667064368236a0f6a120771aa6f6e332d73215df7339a727e1d32cd648");
            var tagNameCase1 = "Tresearch Manager Add Tag2";
            var nodeCase1 = new List<int> { 0, 1, 2 };
            var resultCase1 = IMessageBank.Responses.tagAddSuccess;

            /**
             *  Case 2: Tag nodes. User is Authenticated and Authorized. Node exists and contains tag
             *      NodeIDs:                    xxxx0
             *      Tag Name:                   Tresearch Manager Add Tag3 
             *      
             *      Result:                     "200: Server: Tag added to node(s)."
             */
            IRoleIdentity roleIdentity2 = new RoleIdentity(true, "tagManagerIntegration1@tresearch.system", "user", "d9e22e6b5668fe3bc85246df7aee535f65cc3fdcd95d468993136da4a35e2f4ac1052c667064368236a0f6a120771aa6f6e332d73215df7339a727e1d32cd648");
            var tagNameCase2 = "Tresearch Manager Add Tag3";
            var nodeCase2 = new List<int> { 0 };
            var resultCase2 = IMessageBank.Responses.tagAddSuccess;

            /**
             *  Case 3: Tag nodes. User is Authenticated and Authorized. Node exists and does not contain tag
             *      NodeIDs:                    xxxx0
             *      Tag Name:                   Tresearch Manager Add Tag4 
             *      
             *      Result:                     "200: Server: Tag added to node(s)."
             */
            IRoleIdentity roleIdentity3 = new RoleIdentity(true, "tagManagerIntegration1@tresearch.system", "user", "d9e22e6b5668fe3bc85246df7aee535f65cc3fdcd95d468993136da4a35e2f4ac1052c667064368236a0f6a120771aa6f6e332d73215df7339a727e1d32cd648");
            var tagNameCase3 = "Tresearch Manager Add Tag4";
            var nodeCase3 = new List<int> { 0 };
            var resultCase3 = IMessageBank.Responses.tagAddSuccess;

            /**
             *  Case 4: Tag nodes. User is Authenticated. No node passed in
             *      NodeIDs:                    
             *      Tag Name:                   Tresearch Manager Add Tag4 
             *      
             *      Result:                     "404: Database: The node was not found."
             */
            IRoleIdentity roleIdentity4 = new RoleIdentity(true, "tagManagerIntegration1@tresearch.system", "user", "d9e22e6b5668fe3bc85246df7aee535f65cc3fdcd95d468993136da4a35e2f4ac1052c667064368236a0f6a120771aa6f6e332d73215df7339a727e1d32cd648");
            var tagNameCase4 = "Tresearch Manager Add Tag4";
            var nodeCase4 = new List<int> { };
            var resultCase4 = IMessageBank.Responses.nodeNotFound;

            /**
             *  Case 5: Tag nodes. User is Authenticated and Authorized. Node exists but tag doesn't exist
             *      NodeIDs:                    xxxx0
             *      Tag Name:                   Tresearch Manager Add Tag5 
             *      
             *      Result:                     "404: Database: Tag not found."
             */
            IRoleIdentity roleIdentity5 = new RoleIdentity(true, "tagManagerIntegration1@tresearch.system", "user", "d9e22e6b5668fe3bc85246df7aee535f65cc3fdcd95d468993136da4a35e2f4ac1052c667064368236a0f6a120771aa6f6e332d73215df7339a727e1d32cd648");
            var tagNameCase5 = "Tresearch Manager Add Tag5";
            var nodeCase5 = new List<int> { 0 };
            var resultCase5 = IMessageBank.Responses.tagNotFound;

            /**
             *  Case 6: Tag nodes. User is Authenticated and Authorized. Some nodes already have tag 
             *      NodeIDs:                    xxxx0, xxxx1, xxx2 (xxxx1 contains tag alredy)
             *      Tag Name:                   Tresearch Manager Add Tag2 
             *      
             *      Result:                     "200: Server: Tag added to node(s)."
             */
            IRoleIdentity roleIdentity6 = new RoleIdentity(true, "tagManagerIntegration1@tresearch.system", "user", "d9e22e6b5668fe3bc85246df7aee535f65cc3fdcd95d468993136da4a35e2f4ac1052c667064368236a0f6a120771aa6f6e332d73215df7339a727e1d32cd648");
            var tagNameCase6 = "Tresearch Manager Add Tag3";
            var nodeCase6 = new List<int> { 0, 1, 2 };
            var resultCase6 = IMessageBank.Responses.tagAddSuccess;

            /**
             *  Case 7: Tag nodes. User is authenticated but not authorized to make changes to ALL nodes.
             *      NodeIDs:                    xxxx0, xxxx1, xxx2 
             *      Tag Name:                   Tresearch Manager Add Tag3 
             *      
             *      Result:                     403: Database: You are not authorized to perform this operation.
             */
            IRoleIdentity roleIdentity7 = new RoleIdentity(true, "tagManagerIntegration2@tresearch.system", "user", "f59b47456839aadf4328940ee16e473659a48978f5bf81669dee37aac6ecd1a1e380947d68343f3c634378d7964ec573e211e8796036188b417d3265d8fd7a89");
            var tagNameCase7 = "Tresearch Manager Add Tag3";
            var nodeCase7 = new List<int> { 0, 1, 2 };
            var resultCase7 = IMessageBank.Responses.notAuthorized;

            /**
             *  Case 8: Tag nodes. User is authenticatd but not authorized to make changes to SOME nodes.
             *      NodeIDs:                    xxxx0, xxxx3, xxx4      
             *                                  xxxx0 : Not authorized
             *                                  xxxx3 : Authorized
             *                                  xxxx4 : Authorized
             *                                  
             *      Tag Name:                   Tresearch Manager Add Tag3 
             *      
             *      Result:                     403: Database: You are not authorized to perform this operation.
             */
            IRoleIdentity roleIdentity8 = new RoleIdentity(true, "tagManagerIntegration2@tresearch.system", "user", "f59b47456839aadf4328940ee16e473659a48978f5bf81669dee37aac6ecd1a1e380947d68343f3c634378d7964ec573e211e8796036188b417d3265d8fd7a89");
            var tagNameCase8 = "Tresearch Manager Add Tag3";
            var nodeCase8 = new List<int> { 0, 3, 4 };
            var resultCase8 = IMessageBank.Responses.notAuthorized;

            /**
             *  Case 9: Tag nodes. User is authenticatd but not authorized to make changes to node.
             *      NodeIDs:                    xxxx0     
             *                                  xxxx0 : Not authorized
             *                                  
             *      Tag Name:                   Tresearch Manager Add Tag3 
             *      
             *      Result:                     403: Database: You are not authorized to perform this operation.
             */
            IRoleIdentity roleIdentity9 = new RoleIdentity(true, "tagManagerIntegration2@tresearch.system", "user", "f59b47456839aadf4328940ee16e473659a48978f5bf81669dee37aac6ecd1a1e380947d68343f3c634378d7964ec573e211e8796036188b417d3265d8fd7a89");
            var tagNameCase9 = "Tresearch Manager Add Tag3";
            var nodeCase9 = new List<int> { 0};
            var resultCase9 = IMessageBank.Responses.notAuthorized;

            /**
             *  Case 10: Tag nodes. User is not authenticated
             *      NodeIDs:                    xxxx0     
             *                                  xxxx0 : Not authorized
             *                                  
             *      Tag Name:                   Tresearch Manager Add Tag3 
             *      
             *      Result:                     401: Server: No active session found. Please login and try again.
             */
            IRoleIdentity roleIdentity10 = new RoleIdentity(true, "guest", "guest", "");
            var tagNameCase10 = "Tresearch Manager Add Tag3";
            var nodeCase10 = new List<int> { 0 };
            var resultCase10 = IMessageBank.Responses.notAuthenticated;

            /**
             *  Case 11: Tag nodes. User is not enabled
             *      NodeIDs:                    xxxx0     
             *                                  xxxx0 : Not authorized
             *                                  
             *      Tag Name:                   Tresearch Manager Add Tag3 
             *      
             *      Result:                     401: Database: UserAccount disabled. Perform account recovery or contact system admin.
             */
            IRoleIdentity roleIdentity11 = new RoleIdentity(false, "tagManagerIntegrationNotEnabled@tresearch.system", "user", "820868f6b568617dca6164bd6d129fd3f0d47ee2da6785c2247f74b5fa174c8b0b4630b4c21e49f410db2293d2c89b2177a8ef5855e34a9b71402d8d57d7de8b");
            var tagNameCase11 = "Tresearch Manager Add Tag3";
            var nodeCase11 = new List<int> { 0 };
            var resultCase11 = IMessageBank.Responses.notEnabled;

            /**
             *  Case 12: Tag nodes. Account does not exist
             *      NodeIDs:                    xxxx0     
             *                                  xxxx0 : Not authorized
             *                                  
             *      Tag Name:                   Tresearch Manager Add Tag3 
             *      
             *      Result:                     404: Database: Account not found.
             */
            IRoleIdentity roleIdentity12 = new RoleIdentity(false, "tagManagerNoAccount@tresearch.system", "user", "A129733b11ce19340d78c79468ac3723632faede195ee8a78864afdd9a08cc6841feefee84b21a4f48c6e59d182c061b03439fed15f2c8ba8a5022ce1bcaffd3");
            var tagNameCase12 = "Tresearch Manager Add Tag3";
            var nodeCase12 = new List<int> { 0 };
            var resultCase12 = IMessageBank.Responses.accountNotFound;


            yield return new object[] { roleIdentity0, nodeCase0, tagNameCase0, resultCase0 };
            yield return new object[] { roleIdentity1, nodeCase1, tagNameCase1, resultCase1 };
            yield return new object[] { roleIdentity2, nodeCase2, tagNameCase2, resultCase2 };
            yield return new object[] { roleIdentity3, nodeCase3, tagNameCase3, resultCase3 };
            yield return new object[] { roleIdentity4, nodeCase4, tagNameCase4, resultCase4 };
            yield return new object[] { roleIdentity5, nodeCase5, tagNameCase5, resultCase5 };
            yield return new object[] { roleIdentity6, nodeCase6, tagNameCase6, resultCase6 };
            yield return new object[] { roleIdentity7, nodeCase7, tagNameCase7, resultCase7 };
            yield return new object[] { roleIdentity8, nodeCase8, tagNameCase8, resultCase8 };
            yield return new object[] { roleIdentity9, nodeCase9, tagNameCase9, resultCase9 };
            yield return new object[] { roleIdentity10, nodeCase10, tagNameCase10, resultCase10 };
            yield return new object[] { roleIdentity11, nodeCase11, tagNameCase11, resultCase11 };
            yield return new object[] { roleIdentity12, nodeCase12, tagNameCase12, resultCase12 };
        }

        [Theory]
        [MemberData(nameof(RemoveNodeTagData))]
        public async Task RemoveNodeTagAsync(IRoleIdentity roleIdentity, List<int> nodeIDs, string tagName, IMessageBank.Responses response)
        {
            //Arrange
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            Thread.CurrentPrincipal = rolePrincipal;
            ITagManager tagManager = TestProvider.GetService<ITagManager>();
            IMessageBank messageBank = TestProvider.GetService<IMessageBank>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            string expected = await messageBank.GetMessage(response);

            //Act
            string result = await tagManager.RemoveTagFromNodesAsync(GetNodes(nodeIDs), tagName, cancellationTokenSource.Token);

            //Arrange
            Assert.Equal(expected, result);
        }

        public static IEnumerable<object[]> RemoveNodeTagData()
        {
            //User Authorized, Nodes already have tag
            IRoleIdentity roleIdentity0 = new RoleIdentity(true, "tagManagerIntegration1@tresearch.system", "user", "d9e22e6b5668fe3bc85246df7aee535f65cc3fdcd95d468993136da4a35e2f4ac1052c667064368236a0f6a120771aa6f6e332d73215df7339a727e1d32cd648");
            var tagNameCase0 = "Tresearch Manager Delete Tag1";
            var nodeListCase0 = new List<int> { 0, 1, 2 };
            var resultCase0 = IMessageBank.Responses.tagRemoveSuccess;

            //User Authorized, Nodes do not contain these tags
            IRoleIdentity roleIdentity1 = new RoleIdentity(true, "tagManagerIntegration1@tresearch.system", "user", "d9e22e6b5668fe3bc85246df7aee535f65cc3fdcd95d468993136da4a35e2f4ac1052c667064368236a0f6a120771aa6f6e332d73215df7339a727e1d32cd648");
            var tagNameCase1 = "Tresearch Manager Delete Tag2";
            var nodeListCase1 = new List<int> { 0, 1, 2 };
            var resultCase1 = IMessageBank.Responses.tagRemoveSuccess;

            //User Authorized, Node already has tag
            IRoleIdentity roleIdentity2 = new RoleIdentity(true, "tagManagerIntegration1@tresearch.system", "user", "d9e22e6b5668fe3bc85246df7aee535f65cc3fdcd95d468993136da4a35e2f4ac1052c667064368236a0f6a120771aa6f6e332d73215df7339a727e1d32cd648");
            var tagNameCase2 = "Tresearch Manager Delete Tag3";
            var nodeListCase2 = new List<int> { 0 };
            var resultCase2 = IMessageBank.Responses.tagRemoveSuccess;

            //User Authorized, Node does not have tag
            IRoleIdentity roleIdentity3 = new RoleIdentity(true, "tagManagerIntegration1@tresearch.system", "user", "d9e22e6b5668fe3bc85246df7aee535f65cc3fdcd95d468993136da4a35e2f4ac1052c667064368236a0f6a120771aa6f6e332d73215df7339a727e1d32cd648");
            var tagNameCase3 = "Tresearch Manager Delete Tag4";
            var nodeListCase3 = new List<int> { 0 };
            var resultCase3 = IMessageBank.Responses.tagRemoveSuccess;

            //User Authorized, No node is passed in
            IRoleIdentity roleIdentity4 = new RoleIdentity(true, "tagManagerIntegration1@tresearch.system", "user", "d9e22e6b5668fe3bc85246df7aee535f65cc3fdcd95d468993136da4a35e2f4ac1052c667064368236a0f6a120771aa6f6e332d73215df7339a727e1d32cd648");
            var tagNameCase4 = "Tresearch Manager Delete Tag4";
            var nodeListCase4 = new List<int> { };
            var resultCase4 = IMessageBank.Responses.nodeNotFound;

            //User Authorized, Tag doesn't exist
            IRoleIdentity roleIdentity5 = new RoleIdentity(true, "tagManagerIntegration1@tresearch.system", "user", "d9e22e6b5668fe3bc85246df7aee535f65cc3fdcd95d468993136da4a35e2f4ac1052c667064368236a0f6a120771aa6f6e332d73215df7339a727e1d32cd648");
            var tagNameCase5 = "Tresearch Manager Delete Tag5";
            var nodeListCase5 = new List<int> { 0 };
            var resultCase5 = IMessageBank.Responses.tagRemoveSuccess;

            //User Authorized, Some Nodes already have tag (8019303351 contains tag)
            IRoleIdentity roleIdentity6 = new RoleIdentity(true, "tagManagerIntegration1@tresearch.system", "user", "d9e22e6b5668fe3bc85246df7aee535f65cc3fdcd95d468993136da4a35e2f4ac1052c667064368236a0f6a120771aa6f6e332d73215df7339a727e1d32cd648");
            var tagNameCase6 = "Tresearch Manager Delete Tag3";
            var nodeListCase6 = new List<int> { 0, 1, 2 };
            var resultCase6 = IMessageBank.Responses.tagRemoveSuccess;

            //User not Auhorized For All Nodes
            IRoleIdentity roleIdentity7 = new RoleIdentity(true, "tagManagerIntegration2@tresearch.system", "user", "f59b47456839aadf4328940ee16e473659a48978f5bf81669dee37aac6ecd1a1e380947d68343f3c634378d7964ec573e211e8796036188b417d3265d8fd7a89");
            var tagNameCase7 = "Tresearch Manager Delete Tag3";
            var nodeListCase7 = new List<int> { 0, 1, 2 };
            var resultCase7 = IMessageBank.Responses.notAuthorized;

            //User not Authorized for some Nodes (user owns 8019303353, 8019303354 but not 8019303350)
            IRoleIdentity roleIdentity8 = new RoleIdentity(true, "tagManagerIntegration2@tresearch.system", "user", "f59b47456839aadf4328940ee16e473659a48978f5bf81669dee37aac6ecd1a1e380947d68343f3c634378d7964ec573e211e8796036188b417d3265d8fd7a89");
            var tagNameCase8 = "Tresearch Manager Delete Tag3";
            var nodeListCase8 = new List<int> { 0, 3, 4 };
            var resultCase8 = IMessageBank.Responses.notAuthorized;

            //User not Authorized for node
            IRoleIdentity roleIdentity9 = new RoleIdentity(true, "tagManagerIntegration2@tresearch.system", "user", "f59b47456839aadf4328940ee16e473659a48978f5bf81669dee37aac6ecd1a1e380947d68343f3c634378d7964ec573e211e8796036188b417d3265d8fd7a89");
            var tagNameCase9 = "Tresearch Manager Delete Tag3";
            var nodeListCase9 = new List<int> { 0 };
            var resultCase9 = IMessageBank.Responses.notAuthorized;

            //User has not been authenticated
            IRoleIdentity roleIdentity10 = new RoleIdentity(true, "guest", "guest", "");
            var tagNameCase10 = "Tresearch Manager Delete Tag3";
            var nodeListCase10 = new List<int> { 0 };
            var resultCase10 = IMessageBank.Responses.notAuthenticated;

            //User is not enabled
            IRoleIdentity roleIdentity11 = new RoleIdentity(true, "tagManagerIntegrationNotEnabled@tresearch.system", "user", "820868f6b568617dca6164bd6d129fd3f0d47ee2da6785c2247f74b5fa174c8b0b4630b4c21e49f410db2293d2c89b2177a8ef5855e34a9b71402d8d57d7de8b");
            var tagNameCase11 = "Tresearch Manager Delete Tag3";
            var nodeListCase11 = new List<int> { 5 };
            var resultCase11 = IMessageBank.Responses.notEnabled;

            //User is not confirmed
            IRoleIdentity roleIdentity12 = new RoleIdentity(true, "tagManagerIntegrationNotConfirmed@tresearch.system", "user", "e129733b11ce19340d78c79468ac3723632faede195ee8a78864afdd9a08cc6841feefee84b21a4f48c6e59d182c061b03439fed15f2c8ba8a5022ce1bcaffd3");
            var tagNameCase12 = "Tresearch Manager Delete Tag3";
            var nodeListCase12 = new List<int> { 6 };
            var resultCase12 = IMessageBank.Responses.notConfirmed;

            //UserAccount  does not exist
            IRoleIdentity roleIdentity13 = new RoleIdentity(true, "tagManagerNoAccount@tresearch.system", "user", "");
            var tagNameCase13 = "Tresearch Manager Delete Tag3";
            var nodeListCase13 = new List<int> { 0 };
            var resultCase13 = IMessageBank.Responses.accountNotFound;

            return new[]
            {
                new object[] { roleIdentity0, nodeListCase0, tagNameCase0, resultCase0 },
                new object[] { roleIdentity1, nodeListCase1, tagNameCase1, resultCase1 },
                new object[] { roleIdentity2, nodeListCase2, tagNameCase2, resultCase2 },
                new object[] { roleIdentity3, nodeListCase3, tagNameCase3, resultCase3 },
                new object[] { roleIdentity4, nodeListCase4, tagNameCase4, resultCase4 },
                new object[] { roleIdentity5, nodeListCase5, tagNameCase5, resultCase5 },
                new object[] { roleIdentity6, nodeListCase6, tagNameCase6, resultCase6 },
                new object[] { roleIdentity7, nodeListCase7, tagNameCase7, resultCase7 },
                new object[] { roleIdentity8, nodeListCase8, tagNameCase8, resultCase8 },
                new object[] { roleIdentity9, nodeListCase9, tagNameCase9, resultCase9 },
                new object[] { roleIdentity10, nodeListCase10, tagNameCase10, resultCase10 },
                new object[] { roleIdentity11, nodeListCase11, tagNameCase11, resultCase11 },
                new object[] { roleIdentity12, nodeListCase12, tagNameCase12, resultCase12 },
                new object[] { roleIdentity13, nodeListCase13, tagNameCase13, resultCase13 }
            };
        }

        [Theory]
        [MemberData(nameof(GetNodeTagData))]
        public async Task GetNodeTagsAsync(IRoleIdentity roleIdentity, List<int> nodeIDs, List<string> expectedTags, IMessageBank.Responses response)
        {
            //Arrange
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            Thread.CurrentPrincipal = rolePrincipal;

            ITagManager tagManager = TestProvider.GetService<ITagManager>();
            IMessageBank messageBank = TestProvider.GetService<IMessageBank>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            string expected = await messageBank.GetMessage(response);

            //Act
            Tuple<List<string>, string> results = await tagManager.GetNodeTagsAsync(GetNodes(nodeIDs), cancellationTokenSource.Token);
            List<string> resultTags = results.Item1;
            string result = results.Item2;

            //Arrange
            Assert.Equal(expected, result);
            Assert.Equal(expectedTags, resultTags);
        }

        public static IEnumerable<object[]> GetNodeTagData()
        {
            /*User Authorized, Nodes contain shared tags but also have other tags
             *          xxxx6: Tresearch Manager Get Tag1, Tresearch Manager Get Tag2
             *          xxxx7: Tresearch Manager Get Tag1, Tresearch Manager Get Tag2
             *          xxxx8: Tresearch Manager Get Tag1, Tresearch Manager Get Tag2, Tresearch Manager Get Tag3
             */

            IRoleIdentity roleIdentity0 = new RoleIdentity(true, "tagManagerIntegration3@tresearch.system", "user", "571510127f69c2e3dee263541e8551d8339dc1d98c4b253b5feb5202b41d420dd55c172818feeb5fd7bf85c067c5af142cb930fac9d776b644428adb4b9c4f7b");
            var tagListCase0 = new List<string> { "Tresearch Manager Get Tag1", "Tresearch Manager Get Tag2" };
            var nodeListCase0 = new List<int> { 6, 7, 8 };
            var resultCase0 = IMessageBank.Responses.tagGetSuccess;

            /*User Authorized, Nodes do not contain shared tags
             *          xxxx8: Tresearch Manager Get Tag1, Tresearch Manager Get Tag2, Tresearch Manager Get Tag3
             *          xxxx9: Tresearch Manager Get Tag2
             *          xxx10: Tresearch Manager Get Tag1
             */
            IRoleIdentity roleIdentity1 = new RoleIdentity(true, "tagManagerIntegration3@tresearch.system", "user", "571510127f69c2e3dee263541e8551d8339dc1d98c4b253b5feb5202b41d420dd55c172818feeb5fd7bf85c067c5af142cb930fac9d776b644428adb4b9c4f7b");
            var tagListCase1 = new List<string> { };
            var nodeListCase1 = new List<int> { 8, 9, 10 };
            var resultCase1 = IMessageBank.Responses.tagGetSuccess;

            //User Authorized, Grab Single Node's Tags (8019303356: Tresearch Manager Get Tag1, Tresearch Manager Get Tag2)
            IRoleIdentity roleIdentity2 = new RoleIdentity(true, "tagManagerIntegration3@tresearch.system", "user", "571510127f69c2e3dee263541e8551d8339dc1d98c4b253b5feb5202b41d420dd55c172818feeb5fd7bf85c067c5af142cb930fac9d776b644428adb4b9c4f7b");
            var tagListCase2 = new List<string> { "Tresearch Manager Get Tag1", "Tresearch Manager Get Tag2" };
            var nodeListCase2 = new List<int> { 6 };
            var resultCase2 = IMessageBank.Responses.tagGetSuccess;

            //User Authorized, Node does not have tag
            IRoleIdentity roleIdentity3 = new RoleIdentity(true, "tagManagerIntegration3@tresearch.system", "user", "571510127f69c2e3dee263541e8551d8339dc1d98c4b253b5feb5202b41d420dd55c172818feeb5fd7bf85c067c5af142cb930fac9d776b644428adb4b9c4f7b");
            var tagListCase3 = new List<string> { };
            var nodeListCase3 = new List<int> { 11 };
            var resultCase3 = IMessageBank.Responses.tagGetSuccess;

            //User Authorized, No node is passed in
            IRoleIdentity roleIdentity4 = new RoleIdentity(true, "tagManagerIntegration3@tresearch.system", "user", "571510127f69c2e3dee263541e8551d8339dc1d98c4b253b5feb5202b41d420dd55c172818feeb5fd7bf85c067c5af142cb930fac9d776b644428adb4b9c4f7b");
            var tagListCase4 = new List<string> { };
            var nodeListCase4 = new List<int> { };
            var resultCase4 = IMessageBank.Responses.nodeNotFound;


            //User not Auhorized For All Nodes
            IRoleIdentity roleIdentity5 = new RoleIdentity(true, "tagManagerIntegration2@tresearch.system", "user", "f59b47456839aadf4328940ee16e473659a48978f5bf81669dee37aac6ecd1a1e380947d68343f3c634378d7964ec573e211e8796036188b417d3265d8fd7a89");
            var tagListCase5 = new List<string> { };
            var nodeListCase5 = new List<int> { 0, 1, 2 };
            var resultCase5 = IMessageBank.Responses.notAuthorized; ;

            //User not Authorized for some Nodes (user owns xxxx3, xxxx4 but not xxxx0)
            IRoleIdentity roleIdentity6 = new RoleIdentity(true, "tagManagerIntegration2@tresearch.system", "user", "f59b47456839aadf4328940ee16e473659a48978f5bf81669dee37aac6ecd1a1e380947d68343f3c634378d7964ec573e211e8796036188b417d3265d8fd7a89");
            var tagListCase6 = new List<string> { };
            var nodeListCase6 = new List<int> { 0, 3, 4 };
            var resultCase6 = IMessageBank.Responses.notAuthorized; ;

            //User not Authorized for node
            IRoleIdentity roleIdentity7 = new RoleIdentity(true, "tagManagerIntegration2@tresearch.system", "user", "f59b47456839aadf4328940ee16e473659a48978f5bf81669dee37aac6ecd1a1e380947d68343f3c634378d7964ec573e211e8796036188b417d3265d8fd7a89");
            var tagListCase7 = new List<string> { };
            var nodeListCase7 = new List<int> { 0 };
            var resultCase7 = IMessageBank.Responses.notAuthorized;

            //User has not been authenticated
            IRoleIdentity roleIdentity8 = new RoleIdentity(true, "guest", "guest", "");
            var tagListCase8 = new List<string> { };
            var nodeListCase8 = new List<int> { 0 };
            var resultCase8 = IMessageBank.Responses.notAuthenticated;

            //User is not enabled
            IRoleIdentity roleIdentity9 = new RoleIdentity(true, "tagManagerIntegrationNotEnabled@tresearch.system", "user", "820868f6b568617dca6164bd6d129fd3f0d47ee2da6785c2247f74b5fa174c8b0b4630b4c21e49f410db2293d2c89b2177a8ef5855e34a9b71402d8d57d7de8b");
            var tagListCase9 = new List<string> { };
            var nodeListCase9 = new List<int> { 0 };
            var resultCase9 = IMessageBank.Responses.notEnabled;

            //User is not confirmed
            IRoleIdentity roleIdentity10 = new RoleIdentity(true, "tagManagerIntegrationNotConfirmed@tresearch.system", "user", "e129733b11ce19340d78c79468ac3723632faede195ee8a78864afdd9a08cc6841feefee84b21a4f48c6e59d182c061b03439fed15f2c8ba8a5022ce1bcaffd3");
            var tagListCase10 = new List<string> { };
            var nodeListCase10 = new List<int> { 6 };
            var resultCase10 = IMessageBank.Responses.notConfirmed;

            //UserAccount  does not exist
            IRoleIdentity roleIdentity11 = new RoleIdentity(true, "tagManagerNoAccount@tresearch.system", "user", "");
            var tagListCase11 = new List<string> { };
            var nodeListCase11 = new List<int> { 0 };
            var resultCase11 = IMessageBank.Responses.accountNotFound;

            return new[]
            {
                new object[] { roleIdentity0, nodeListCase0, tagListCase0, resultCase0 },
                new object[] { roleIdentity1, nodeListCase1, tagListCase1, resultCase1 },
                new object[] { roleIdentity2, nodeListCase2, tagListCase2, resultCase2 },
                new object[] { roleIdentity3, nodeListCase3, tagListCase3, resultCase3 },
                new object[] { roleIdentity4, nodeListCase4, tagListCase4, resultCase4 },
                new object[] { roleIdentity5, nodeListCase5, tagListCase5, resultCase5 },
                new object[] { roleIdentity6, nodeListCase6, tagListCase6, resultCase6 },
                new object[] { roleIdentity7, nodeListCase7, tagListCase7, resultCase7 },
                new object[] { roleIdentity8, nodeListCase8, tagListCase8, resultCase8 },
                new object[] { roleIdentity9, nodeListCase9, tagListCase9, resultCase9 },
                new object[] { roleIdentity10, nodeListCase10, tagListCase10, resultCase10 },
                new object[] { roleIdentity11, nodeListCase11, tagListCase11, resultCase11 }
            };
        }

        [Theory]
        [MemberData(nameof(CreateTagData))]
        public async Task CreateTag(IRoleIdentity roleIdentity, string tagName, string expected)
        {
            //Arrange
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            Thread.CurrentPrincipal = rolePrincipal;
            ITagManager tagManager = TestProvider.GetService<ITagManager>();
            IMessageBank messageBank = TestProvider.GetService<IMessageBank>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            //Act
            string result = await tagManager.CreateTagAsync(tagName, cancellationTokenSource.Token);

            //Arrange
            Assert.Equal(expected, result);
        }

        public static IEnumerable<object[]> CreateTagData()
        {
            //User is Authenticated and Authorized as admin, tag doesn't exist
            IRoleIdentity roleIdentity0 = new RoleIdentity(false, "tagManagerIntegrationAdmin1@tresearch.system", "admin");
            string tagName0 = "Tresearch Manager Doesnt Exist";
            string resultCase0 = "200: Server: Tag created in tag bank.";

            //User is Authenticated and Authorized as admin, tag exists
            IRoleIdentity roleIdentity1 = new RoleIdentity(false, "tagManagerIntegrationAdmin1@tresearch.system", "admin");
            string tagName1 = "Tresearch Manager Tag Exist";
            string resultCase1 = "409: Database: The tag already exists.";

            //User is Authenticated but not authorized (user)
            IRoleIdentity roleIdentity2 = new RoleIdentity(false, "tagManagerIntegration1@gmail.com", "user");
            string tagName2 = "Tresearch Manager Doesnt Managaer";
            string resultCase2 = "403: Database: You are not authorized to perform this operation.";

            //User is not Authenticated
            IRoleIdentity roleIdentity3 = new RoleIdentity(false, "guest", "guest");
            string tagName3 = "Tresearch Manager Doesnt Managaer";
            string resultCase3 = "401: Server: No active session found. Please login and try again.";

            
            //UserAccount  does not exist
            IRoleIdentity roleIdentity4 = new RoleIdentity(false, "tagManagerNoAccount@tresearch.system", "admin");
            string tagName4 = "Tresearch Manager Doesnt Managaer";
            var resultCase4 = "500: Database: The UserAccount was not found.";

            return new[]
           {
                new object[] { roleIdentity0, tagName0, resultCase0 },
                new object[] { roleIdentity1, tagName1, resultCase1 },
                new object[] { roleIdentity2, tagName2, resultCase2 },
                new object[] { roleIdentity3, tagName3, resultCase3 },
                new object[] { roleIdentity4, tagName4, resultCase4 }
            };
        }

        [Theory]
        [MemberData(nameof(DeleteTagData))]
        public async Task DeleteTagAsync(IRoleIdentity roleIdentity, string tagName, string expected)
        {
            //Arrange
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            Thread.CurrentPrincipal = rolePrincipal;
            ITagManager tagManager = TestProvider.GetService<ITagManager>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            //Act
            string result = await tagManager.RemoveTagAsync(tagName, cancellationTokenSource.Token);

            //Arrange
            Assert.Equal(expected, result);
        }

        public static IEnumerable<object[]> DeleteTagData()
        {
            //User is Authenticated and Authorized as admin, tag doesn't exist
            IRoleIdentity roleIdentity0 = new RoleIdentity(false, "tagManagerIntegrationAdmin1@tresearch.system", "admin");
            string tagName0 = "Tresearch Manager REMOVE Doesnt Exist";
            string resultCase0 = "200: Server: Tag removed from tag bank.";

            //User is Authenticated and Authorized as admin, tag exists
            IRoleIdentity roleIdentity1 = new RoleIdentity(false, "tagManagerIntegrationAdmin1@tresearch.system", "admin");
            string tagName1 = "Tresearch Manager REMOVE Tag Exist";
            string resultCase1 = "200: Server: Tag removed from tag bank.";

            //User is Authenticated but not authorized (user)
            IRoleIdentity roleIdentity2 = new RoleIdentity(false, "tagManagerIntegration1@gmail.com", "user");
            string tagName2 = "Tresearch Manager Doesnt Managaer";
            string resultCase2 = "403: Database: You are not authorized to perform this operation.";

            //User is not Authenticated
            IRoleIdentity roleIdentity3 = new RoleIdentity(false, "guest", "guest");
            string tagName3 = "Tresearch Manager Doesnt Managaer";
            string resultCase3 = "401: Server: No active session found. Please login and try again.";


            //UserAccount  does not exist
            IRoleIdentity roleIdentity4 = new RoleIdentity(false, "tagManagerNoAccount@tresearch.system", "admin");
            string tagName4 = "Tresearch Manager Doesnt Managaer";
            var resultCase4 = "500: Database: The UserAccount was not found.";

            //User is Authenticated and Authorized as admin, tag exists and other nodes have this tag
            IRoleIdentity roleIdentity5 = new RoleIdentity(false, "tagManagerIntegrationAdmin1@tresearch.system", "admin");
            string tagName5 = "Tresearch Manager REMOVE Exist and Tagged";
            string resultCase5 = "200: Server: Tag removed from tag bank.";

            return new[]
           {
                new object[] { roleIdentity0, tagName0, resultCase0 },
                new object[] { roleIdentity1, tagName1, resultCase1 },
                new object[] { roleIdentity2, tagName2, resultCase2 },
                new object[] { roleIdentity3, tagName3, resultCase3 },
                new object[] { roleIdentity4, tagName4, resultCase4 },
                new object[] { roleIdentity5, tagName5, resultCase5 }
            };
        }

        [Theory]
        [MemberData(nameof(GetTagData))]
        public async Task GetTagsAsync(IRoleIdentity roleIdentity, string expected)
        {
            //Arrange
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            Thread.CurrentPrincipal = rolePrincipal;
            ITagManager tagManager = TestProvider.GetService<ITagManager>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            //Act
            Tuple<List<ITag>, string> resultTags = await tagManager.GetTagsAsync(cancellationTokenSource.Token);
            string result = resultTags.Item2;

            //Arrange
            Assert.Equal(expected, result);
        }

        public static IEnumerable<object[]> GetTagData()
        {
            //User is Authenticated and Authorized 
            IRoleIdentity roleIdentity0 = new RoleIdentity(false, "tagManagerIntegration1@tresearch.system", "user");
            string resultCase0 = "200: Server: Tag(s) retrieved.";

            //User is Authenticated and Authorized and Admin
            IRoleIdentity roleIdentity1 = new RoleIdentity(false, "tagManagerIntegrationAdmin1@tresearch.system", "admin");
            string resultCase1 = "200: Server: Tag(s) retrieved.";

            //User has not been authenticated
            IRoleIdentity roleIdentity2 = new RoleIdentity(false, "guest", "guest");
            string resultCase2 = "401: Server: No active session found. Please login and try again.";

            //User is not enabled
            IRoleIdentity roleIdentity3 = new RoleIdentity(false, "tagManagerIntegrationNotEnabled@tresearch.system", "user");
            string resultCase3 = "401: Database: UserAccount disabled. Perform account recovery or contact system admin.";

            //User is not confirmed
            IRoleIdentity roleIdentity4 = new RoleIdentity(false, "tagManagerIntegrationNotConfirmed@tresearch.system", "user");
            string resultCase4 = "401: Database: Please confirm your account before attempting to login.";

            //User is has unknown role
            IRoleIdentity roleIdentity5 = new RoleIdentity(false, "tagManagerIntegrationAdmin1@tresearch.system", "wrong");
            string resultCase5 = "400: Server: Unknown role used.";

            //UserAccount not found
            IRoleIdentity roleIdentity6 = new RoleIdentity(false, "tagManagerNoAccount@tresearch.system", "user");
            var resultCase6 = "500: Database: The UserAccount was not found.";

            return new[]
            {
                new object[] { roleIdentity0, resultCase0 },
                new object[] { roleIdentity1, resultCase1 },
                new object[] { roleIdentity2, resultCase2 },
                new object[] { roleIdentity3, resultCase3 },
                new object[] { roleIdentity4, resultCase4 },
                new object[] { roleIdentity5, resultCase5 },
                new object[] { roleIdentity6, resultCase6 }
            };
        }

    }

    public class TagManagerDatabaseFixture : TestBaseClass, IDisposable
    {
        public List<long> nodeIDs { get; set; }
        public TagManagerDatabaseFixture() : base(new string[] { })
        {
            TestProvider = TestServices.BuildServiceProvider();
            IOptionsSnapshot<BuildSettingsOptions> options = TestProvider.GetService<IOptionsSnapshot<BuildSettingsOptions>>();
            BuildSettingsOptions optionsValue = options.Value;

            string script = File.ReadAllText("../../../IntegrationTests/Tag/SetupAndCleanup/ManagerIntegrationSetup.sql");

            IEnumerable<string> commands = Regex.Split(script, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);

            using (var connection = new SqlConnection(optionsValue.SqlConnectionString))
            {
                connection.Open();
                foreach (string command in commands)
                    if (!string.IsNullOrWhiteSpace(command.Trim()))
                        using (var com = new SqlCommand(command, connection))
                            com.ExecuteNonQuery();
            }

            // Initialize list of nodes. 
            using (var connection = new SqlConnection(optionsValue.SqlConnectionString))
            {
                connection.Open();
                var procedure = "dbo.[ManagerIntegrationTagInitializeProcedure]";                    // Name of store procedure
                var value = new { };                                                                 // Parameters of stored procedure
                nodeIDs = connection.Query<long>(new CommandDefinition(procedure, value, commandType: CommandType.StoredProcedure)).ToList();
            }
        }

        public void Dispose()
        {
            IOptionsSnapshot<BuildSettingsOptions> options = TestProvider.GetService<IOptionsSnapshot<BuildSettingsOptions>>();
            BuildSettingsOptions optionsValue = options.Value;

            string script = File.ReadAllText("../../../IntegrationTests/Tag/SetupAndCleanup/ManagerIntegrationCleanup.sql");

            IEnumerable<string> commands = Regex.Split(script, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);

            using (var connection = new SqlConnection(optionsValue.SqlConnectionString))
            {
                connection.Open();
                foreach (string command in commands)
                    if (!string.IsNullOrWhiteSpace(command.Trim()))
                        using (var com = new SqlCommand(command, connection))
                            com.ExecuteNonQuery();
            }
        }
    }
}
