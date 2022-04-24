
using Microsoft.Extensions.DependencyInjection;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Managers.Implementations;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Services.Implementations;
using Xunit;

namespace TrialByFire.Tresearch.Tests.UnitTests.Tag
{
    public class InMemoryTagManagerShould: TestBaseClass
    {
        public InMemoryTagManagerShould() : base(new string[] { })
        {
            TestServices.AddScoped<ISqlDAO, InMemorySqlDAO>();
            TestServices.AddScoped<ITagService, TagService>();
            TestServices.AddScoped<ITagManager, TagManager>();
            TestProvider = TestServices.BuildServiceProvider();
        }

        [Theory]
        [MemberData(nameof(AddNodeTagData))]
        public async Task AddTagToNodeAsync(IRoleIdentity roleIdentity, List<long> nodeIDs, string tagName, IMessageBank.Responses response)
        {
            //Arrange
            ITagManager tagManager = TestProvider.GetService<ITagManager>();

            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            Thread.CurrentPrincipal = rolePrincipal;
            
            IMessageBank messageBank = TestProvider.GetService<IMessageBank>();
            string expected = await messageBank.GetMessage(response).ConfigureAwait(false);

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            //Act
            string result = await tagManager.AddTagToNodesAsync(nodeIDs, tagName, cancellationTokenSource.Token);

            //Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [MemberData(nameof(RemoveNodeTagData))]
        public async Task RemoveTagFromNodeAsync(IRoleIdentity roleIdentity, List<long> nodeIDs, string tagName, IMessageBank.Responses response)
        {
            //Arrange
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            Thread.CurrentPrincipal = rolePrincipal;

            IMessageBank messageBank = TestProvider.GetService<IMessageBank>();
            string expected = await messageBank.GetMessage(response);
            ITagManager tagManager = TestProvider.GetService<ITagManager>();

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            //Act
            string result = await tagManager.RemoveTagFromNodesAsync(nodeIDs, tagName, cancellationTokenSource.Token);

            //Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [MemberData(nameof(GetNodeTagData))]
        public async Task GetNodeTagsAsync(IRoleIdentity roleIdentity, List<long> nodeIDs, List<string> tagsExpected, IMessageBank.Responses response)
        {
            //Arrange
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            Thread.CurrentPrincipal = rolePrincipal;

            IMessageBank messageBank = TestProvider.GetService<IMessageBank>();
            string expected = await messageBank.GetMessage(response);
            ITagManager tagManager = TestProvider.GetService<ITagManager>();

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            //Act
            Tuple<List<string>, string> results = await tagManager.GetNodeTagsAsync(nodeIDs, cancellationTokenSource.Token);
            List<string> tagsResults = results.Item1;
            string result = results.Item2;

            //Assert
            Assert.Equal(expected, result);
            Assert.Equal(tagsExpected, tagsResults);
        }

        [Theory]
        [MemberData(nameof(CreateTagData))]
        public async Task CreateTagAsync(IRoleIdentity roleIdentity, string tagName, IMessageBank.Responses response)
        {
            //Arrange
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            Thread.CurrentPrincipal = rolePrincipal;

            IMessageBank messageBank = TestProvider.GetService<IMessageBank>();
            string expected = await messageBank.GetMessage(response);
            ITagManager tagManager = TestProvider.GetService<ITagManager>();

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            //Act
            string result = await tagManager.CreateTagAsync(tagName, cancellationTokenSource.Token);

            //Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [MemberData(nameof(DeleteTagData))]
        public async Task DeleteTagAsync(IRoleIdentity roleIdentity, string tagName, IMessageBank.Responses response)
        {
            //Arrange
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            Thread.CurrentPrincipal = rolePrincipal;

            IMessageBank messageBank = TestProvider.GetService<IMessageBank>();
            string expected = await messageBank.GetMessage(response);
            ITagManager tagManager = TestProvider.GetService<ITagManager>();

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            //Act
            string result = await tagManager.RemoveTagAsync(tagName, cancellationTokenSource.Token);

            //Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [MemberData(nameof(GetTagData))]
        public async Task GetTagsAsync(IRoleIdentity roleIdentity, IMessageBank.Responses response)
        {
            //Arrange
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            Thread.CurrentPrincipal = rolePrincipal;

            IMessageBank messageBank = TestProvider.GetService<IMessageBank>();
            string expected = await messageBank.GetMessage(response);
            ITagManager tagManager = TestProvider.GetService<ITagManager>();

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            //Act
            Tuple<List<ITag>, string> results = await tagManager.GetTagsAsync(cancellationTokenSource.Token);
            string result = results.Item2;

            //Assert
            Assert.Equal(expected, result);
        }

        /// <summary>
        ///     Test data to add tag to list of node(s)
        ///     <br>Case 0: Tag nodes. User is Authenticated and Authorized. Nodes exist and already contain tag.</br>
        ///     <br>Case 1: Tag nodes. User is Authenticated and Authorized. Nodes exist and do not contain tag.</br>
        ///     <br>Case 2: Tag nodes. User is Authenticated and Authorized. Nodes already has tag.</br>
        ///     <br>Case 3: Tag nodes. User is Authenticated and Authorized. Nodes does not have tag.</br>
        ///     <br>Case 4: Tag nodes. User is Authenticated and Authorized. Node node is passed in.</br>
        ///     <br>Case 5: Tag nodes. User is Authenticated and Authorized. Tag does not exist.</br>
        ///     <br>Case 6: Tag nodes. User is Authenticated and Authorized. Some nodes already have tag.</br>
        ///     <br>Case 7: Tag nodes. User is Authenticated but user is not authorized for ALL nodes.</br>
        ///     <br>Case 8: Tag nodes. User is Authenticated but user is not authorized for some nodes.</br>
        ///     <br>Case 9: Tag nodes. User is Authenticated but user is not authorized for node.</br>
        ///     <br>Case 10: Tag nodes. User is Authenticated but user is not authenticated.</br>
        ///     <br>Case 11: Tag nodes. User is Authenticated but user is not enabled.</br>
        ///     <br>Case 12: Tag nodes. User is Authenticated but user is not confirmed.</br>
        ///     <br>Case 13: Tag nodes. User is authenticated but account does not exist.</br>
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<object[]> AddNodeTagData()
        {
            /**
             *  Case 0: Tag nodes. User is Authenticated and Authorized. Nodes exist and already contain tag.
             *      Account:                    tagManagerIntegration1@tresearch.system
             *      NodeIDs:                    8019303350, 8019303351, 8019303352
             *      Tag Name:                   Tresearch Manager Add Tag1 
             */
            IRoleIdentity roleIdentity0 = new RoleIdentity(true, "tagManagerIntegration1@tresearch.system", "user", "d9e22e6b5668fe3bc85246df7aee535f65cc3fdcd95d468993136da4a35e2f4ac1052c667064368236a0f6a120771aa6f6e332d73215df7339a727e1d32cd648");
            var tagNameCase0 = "Tresearch Manager Add Tag1";
            var nodeListCase0 = new List<long> { 8019303350, 8019303351, 8019303352 };
            var resultCase0 = IMessageBank.Responses.tagAddSuccess;

            /**
             *  Case 1: Tag nodes. User is Authenticated and Authorized. Nodes exist and do not contain tag.
             *      Account:                    tagManagerIntegration1@tresearch.system
             *      NodeIDs:                    8019303350, 8019303351, 8019303352
             *      Tag Name:                   Tresearch Manager Add Tag2 
             */
            IRoleIdentity roleIdentity1 = new RoleIdentity(true, "tagManagerIntegration1@tresearch.system", "user", "d9e22e6b5668fe3bc85246df7aee535f65cc3fdcd95d468993136da4a35e2f4ac1052c667064368236a0f6a120771aa6f6e332d73215df7339a727e1d32cd648");
            var tagNameCase1 = "Tresearch Manager Add Tag2";
            var nodeListCase1 = new List<long> { 8019303350, 8019303351, 8019303352 };
            var resultCase1 = IMessageBank.Responses.tagAddSuccess;

            /**
             *  Case 2: Tag nodes. User is Authenticated and Authorized. Nodes already has tag.
             *      Account:                    tagManagerIntegration1@tresearch.system
             *      NodeIDs:                    8019303350
             *      Tag Name:                   Tresearch Manager Add Tag3 
             */
            IRoleIdentity roleIdentity2 = new RoleIdentity(true, "tagManagerIntegration1@tresearch.system", "user", "d9e22e6b5668fe3bc85246df7aee535f65cc3fdcd95d468993136da4a35e2f4ac1052c667064368236a0f6a120771aa6f6e332d73215df7339a727e1d32cd648");
            var tagNameCase2 = "Tresearch Manager Add Tag3";
            var nodeListCase2 = new List<long> { 8019303350 };
            var resultCase2 = IMessageBank.Responses.tagAddSuccess;

            /**
             *  Case 3: Tag nodes. User is Authenticated and Authorized. Nodes does not have tag.
             *      Account:                    tagManagerIntegration1@tresearch.system
             *      NodeIDs:                    8019303350
             *      Tag Name:                   Tresearch Manager Add Tag4
             */
            IRoleIdentity roleIdentity3 = new RoleIdentity(true, "tagManagerIntegration1@tresearch.system", "user", "d9e22e6b5668fe3bc85246df7aee535f65cc3fdcd95d468993136da4a35e2f4ac1052c667064368236a0f6a120771aa6f6e332d73215df7339a727e1d32cd648");
            var tagNameCase3 = "Tresearch Manager Add Tag4";
            var nodeListCase3 = new List<long> { 8019303350 };
            var resultCase3 = IMessageBank.Responses.tagAddSuccess;

            /**
             *  Case 4: Tag nodes. User is Authenticated and Authorized. Node node is passed in.
             *      Account:                    tagManagerIntegration1@tresearch.system
             *      NodeIDs:                    8019303350
             *      Tag Name:                   Tresearch Manager Add Tag4
             */
            IRoleIdentity roleIdentity4 = new RoleIdentity(true, "tagManagerIntegration1@tresearch.system", "user", "d9e22e6b5668fe3bc85246df7aee535f65cc3fdcd95d468993136da4a35e2f4ac1052c667064368236a0f6a120771aa6f6e332d73215df7339a727e1d32cd648");
            var tagNameCase4 = "Tresearch Manager Add Tag4";
            var nodeListCase4 = new List<long> { };
            var resultCase4 = IMessageBank.Responses.nodeNotFound;

            /**
             *  Case 5: Tag nodes. User is Authenticated and Authorized. Tag does not exist.
             *      Account:                    tagManagerIntegration1@tresearch.system
             *      NodeIDs:                    8019303350
             *      Tag Name:                   Tresearch Manager Add Tag5
             */
            IRoleIdentity roleIdentity5 = new RoleIdentity(true, "tagManagerIntegration1@tresearch.system", "user", "d9e22e6b5668fe3bc85246df7aee535f65cc3fdcd95d468993136da4a35e2f4ac1052c667064368236a0f6a120771aa6f6e332d73215df7339a727e1d32cd648");
            var tagNameCase5 = "Tresearch Manager Add Tag5";
            var nodeListCase5 = new List<long> { 8019303350 };
            var resultCase5 = IMessageBank.Responses.tagNotFound;

            /**
             *  Case 6: Tag nodes. User is Authenticated and Authorized. Some nodes already have tag.
             *      Account:                    tagManagerIntegration1@tresearch.system
             *      NodeIDs:                    8019303350, 8019303351, 8019303352
             *      Tag Name:                   Tresearch Manager Add Tag3
             */
            IRoleIdentity roleIdentity6 = new RoleIdentity(true, "tagManagerIntegration1@tresearch.system", "user", "d9e22e6b5668fe3bc85246df7aee535f65cc3fdcd95d468993136da4a35e2f4ac1052c667064368236a0f6a120771aa6f6e332d73215df7339a727e1d32cd648");
            var tagNameCase6 = "Tresearch Manager Add Tag3";
            var nodeListCase6 = new List<long> { 8019303350, 8019303351, 8019303352 };
            var resultCase6 = IMessageBank.Responses.tagAddSuccess;

            /**
             *  Case 7: Tag nodes. User is Authenticated but user is not authorized for ALL nodes.
             *      Account:                    tagManagerIntegration2@tresearch.system
             *      NodeIDs:                    8019303350, 8019303351, 8019303352
             *      Tag Name:                   Tresearch Manager Add Tag3
             */
            IRoleIdentity roleIdentity7 = new RoleIdentity(true, "tagManagerIntegration2@tresearch.system", "user", "f59b47456839aadf4328940ee16e473659a48978f5bf81669dee37aac6ecd1a1e380947d68343f3c634378d7964ec573e211e8796036188b417d3265d8fd7a89");
            var tagNameCase7 = "Tresearch Manager Add Tag3";
            var nodeListCase7 = new List<long> { 8019303350, 8019303351, 8019303352 };
            var resultCase7 = IMessageBank.Responses.notAuthorized;

            /**
            *  Case 8: Tag nodes. User is Authenticated but user is not authorized for some nodes.
            *      Account:                    tagManagerIntegration2@tresearch.system
            *      NodeIDs:                    8019303350, 8019303353, 8019303354
            *                                       * User owns 8019303353, 8019303354
            *      Tag Name:                   Tresearch Manager Add Tag3
            */
            IRoleIdentity roleIdentity8 = new RoleIdentity(true, "tagManagerIntegration2@tresearch.system", "user", "f59b47456839aadf4328940ee16e473659a48978f5bf81669dee37aac6ecd1a1e380947d68343f3c634378d7964ec573e211e8796036188b417d3265d8fd7a89");
            var tagNameCase8 = "Tresearch Manager Add Tag3";
            var nodeListCase8 = new List<long> { 8019303350, 8019303353, 8019303354 };
            var resultCase8 = IMessageBank.Responses.notAuthorized;

            /**
            *  Case 9: Tag nodes. User is Authenticated but user is not authorized for node.
            *      Account:                    tagManagerIntegration2@tresearch.system
            *      NodeIDs:                    8019303350
            *      Tag Name:                   Tresearch Manager Add Tag3
            */
            IRoleIdentity roleIdentity9 = new RoleIdentity(true, "tagManagerIntegration2@tresearch.system", "user", "f59b47456839aadf4328940ee16e473659a48978f5bf81669dee37aac6ecd1a1e380947d68343f3c634378d7964ec573e211e8796036188b417d3265d8fd7a89");
            var tagNameCase9 = "Tresearch Manager Add Tag3";
            var nodeListCase9 = new List<long> { 8019303350 };
            var resultCase9 = IMessageBank.Responses.notAuthorized;

            /**
           *  Case 10: Tag nodes. User is Authenticated but user is not authenticated
           *      Account:                    
           *      NodeIDs:                    8019303350
           *      Tag Name:                   Tresearch Manager Add Tag3
           */
            IRoleIdentity roleIdentity10 = new RoleIdentity(true, "guest", "guest", "");
            var tagNameCase10 = "Tresearch Manager Add Tag3";
            var nodeListCase10 = new List<long> { 8019303350 };
            var resultCase10 = IMessageBank.Responses.notAuthenticated;

            /**
           *  Case 11: Tag nodes. User is Authenticated but user is not enabled.
           *      Account:                    tagManagerIntegrationNotEnabled@tresearch.system
           *      NodeIDs:                    8019303350
           *      Tag Name:                   Tresearch Manager Add Tag3
           */
            IRoleIdentity roleIdentity11 = new RoleIdentity(false, "tagManagerIntegrationNotEnabled@tresearch.system", "user", "820868f6b568617dca6164bd6d129fd3f0d47ee2da6785c2247f74b5fa174c8b0b4630b4c21e49f410db2293d2c89b2177a8ef5855e34a9b71402d8d57d7de8b");
            var tagNameCase11 = "Tresearch Manager Add Tag3";
            var nodeListCase11 = new List<long> { 8019303365 };
            var resultCase11 = IMessageBank.Responses.notEnabled;

            /**
           *  Case 12: Tag nodes. User is Authenticated but user is not confirmed.
           *      Account:                    tagManagerIntegrationNotConfirmed@tresearch.system
           *      NodeIDs:                    8019303366
           *      Tag Name:                   Tresearch Manager Add Tag3
           */
            IRoleIdentity roleIdentity12 = new RoleIdentity(false, "tagManagerIntegrationNotConfirmed@tresearch.system", "user", "e129733b11ce19340d78c79468ac3723632faede195ee8a78864afdd9a08cc6841feefee84b21a4f48c6e59d182c061b03439fed15f2c8ba8a5022ce1bcaffd3");
            var tagNameCase12 = "Tresearch Manager Add Tag3";
            var nodeListCase12 = new List<long> { 8019303366 };
            var resultCase12 = IMessageBank.Responses.notConfirmed;

            /**
           *  Case 13: Tag nodes. User is authenticated but account does not exist.
           *      Account:                    tagManagerNoAccount@tresearch.system
           *      NodeIDs:                    8019303366
           *      Tag Name:                   Tresearch Manager Add Tag3
           */
            IRoleIdentity roleIdentity13 = new RoleIdentity(false, "tagManagerNoAccount@tresearch.system", "user");
            var tagNameCase13 = "Tresearch Manager Add Tag3";
            var nodeListCase13 = new List<long> { 8019303366 };
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

        public static IEnumerable<object[]> RemoveNodeTagData()
        {
            //User Authorized, Nodes already have tag
            IRoleIdentity roleIdentity0 = new RoleIdentity(true, "tagManagerIntegration1@tresearch.system", "user", "d9e22e6b5668fe3bc85246df7aee535f65cc3fdcd95d468993136da4a35e2f4ac1052c667064368236a0f6a120771aa6f6e332d73215df7339a727e1d32cd648");
            var tagNameCase0 = "Tresearch Manager Delete Tag1";
            var nodeListCase0 = new List<long> { 8019303350, 8019303351, 8019303352 };
            var resultCase0 = IMessageBank.Responses.tagRemoveSuccess;

            //User Authorized, Nodes do not contain these tags
            IRoleIdentity roleIdentity1 = new RoleIdentity(true, "tagManagerIntegration1@tresearch.system", "user", "d9e22e6b5668fe3bc85246df7aee535f65cc3fdcd95d468993136da4a35e2f4ac1052c667064368236a0f6a120771aa6f6e332d73215df7339a727e1d32cd648");
            var tagNameCase1 = "Tresearch Manager Delete Tag2";
            var nodeListCase1 = new List<long> { 8019303350, 8019303351, 8019303352 };
            var resultCase1 = IMessageBank.Responses.tagRemoveSuccess;

            //User Authorized, Node already has tag
            IRoleIdentity roleIdentity2 = new RoleIdentity(true, "tagManagerIntegration1@tresearch.system", "user", "d9e22e6b5668fe3bc85246df7aee535f65cc3fdcd95d468993136da4a35e2f4ac1052c667064368236a0f6a120771aa6f6e332d73215df7339a727e1d32cd648");
            var tagNameCase2 = "Tresearch Manager Delete Tag3";
            var nodeListCase2 = new List<long> { 8019303350 };
            var resultCase2 = IMessageBank.Responses.tagRemoveSuccess;

            //User Authorized, Node does not have tag
            IRoleIdentity roleIdentity3 = new RoleIdentity(true, "tagManagerIntegration1@tresearch.system", "user", "d9e22e6b5668fe3bc85246df7aee535f65cc3fdcd95d468993136da4a35e2f4ac1052c667064368236a0f6a120771aa6f6e332d73215df7339a727e1d32cd648");
            var tagNameCase3 = "Tresearch Manager Delete Tag4";
            var nodeListCase3 = new List<long> { 8019303350 };
            var resultCase3 = IMessageBank.Responses.tagRemoveSuccess;

            //User Authorized, No node is passed in
            IRoleIdentity roleIdentity4 = new RoleIdentity(true, "tagManagerIntegration1@tresearch.system", "user", "d9e22e6b5668fe3bc85246df7aee535f65cc3fdcd95d468993136da4a35e2f4ac1052c667064368236a0f6a120771aa6f6e332d73215df7339a727e1d32cd648");
            var tagNameCase4 = "Tresearch Manager Delete Tag4";
            var nodeListCase4 = new List<long> { };
            var resultCase4 = IMessageBank.Responses.nodeNotFound;

            //User Authorized, Tag doesn't exist
            IRoleIdentity roleIdentity5 = new RoleIdentity(true, "tagManagerIntegration1@tresearch.system", "user", "d9e22e6b5668fe3bc85246df7aee535f65cc3fdcd95d468993136da4a35e2f4ac1052c667064368236a0f6a120771aa6f6e332d73215df7339a727e1d32cd648");
            var tagNameCase5 = "Tresearch Manager Delete Tag5";
            var nodeListCase5 = new List<long> { 8019303350 };
            var resultCase5 = IMessageBank.Responses.tagRemoveSuccess;

            //User Authorized, Some Nodes already have tag (8019303351 contains tag)
            IRoleIdentity roleIdentity6 = new RoleIdentity(true, "tagManagerIntegration1@tresearch.system", "user", "d9e22e6b5668fe3bc85246df7aee535f65cc3fdcd95d468993136da4a35e2f4ac1052c667064368236a0f6a120771aa6f6e332d73215df7339a727e1d32cd648");
            var tagNameCase6 = "Tresearch Manager Delete Tag3";
            var nodeListCase6 = new List<long> { 8019303350, 8019303351, 8019303352 };
            var resultCase6 = IMessageBank.Responses.tagRemoveSuccess;

            //User not Auhorized For All Nodes
            IRoleIdentity roleIdentity7 = new RoleIdentity(true, "tagManagerIntegration2@tresearch.system", "user", "f59b47456839aadf4328940ee16e473659a48978f5bf81669dee37aac6ecd1a1e380947d68343f3c634378d7964ec573e211e8796036188b417d3265d8fd7a89");
            var tagNameCase7 = "Tresearch Manager Delete Tag3";
            var nodeListCase7 = new List<long> { 8019303350, 8019303351, 8019303352 };
            var resultCase7 = IMessageBank.Responses.notAuthorized;

            //User not Authorized for some Nodes (user owns 8019303353, 8019303354 but not 8019303350)
            IRoleIdentity roleIdentity8 = new RoleIdentity(true, "tagManagerIntegration2@tresearch.system", "user", "f59b47456839aadf4328940ee16e473659a48978f5bf81669dee37aac6ecd1a1e380947d68343f3c634378d7964ec573e211e8796036188b417d3265d8fd7a89");
            var tagNameCase8 = "Tresearch Manager Delete Tag3";
            var nodeListCase8 = new List<long> { 8019303350, 8019303353, 8019303354 };
            var resultCase8 = IMessageBank.Responses.notAuthorized;

            //User not Authorized for node
            IRoleIdentity roleIdentity9 = new RoleIdentity(true, "tagManagerIntegration2@tresearch.system", "user", "f59b47456839aadf4328940ee16e473659a48978f5bf81669dee37aac6ecd1a1e380947d68343f3c634378d7964ec573e211e8796036188b417d3265d8fd7a89");
            var tagNameCase9 = "Tresearch Manager Delete Tag3";
            var nodeListCase9 = new List<long> { 8019303350 };
            var resultCase9 = IMessageBank.Responses.notAuthorized; ;

            //User has not been authenticated
            IRoleIdentity roleIdentity10 = new RoleIdentity(true, "guest", "guest", "");
            var tagNameCase10 = "Tresearch Manager Delete Tag3";
            var nodeListCase10 = new List<long> { 8019303350 };
            var resultCase10 = IMessageBank.Responses.notAuthenticated;

            //User is not enabled
            IRoleIdentity roleIdentity11 = new RoleIdentity(true, "tagManagerIntegrationNotEnabled@tresearch.system", "user", "820868f6b568617dca6164bd6d129fd3f0d47ee2da6785c2247f74b5fa174c8b0b4630b4c21e49f410db2293d2c89b2177a8ef5855e34a9b71402d8d57d7de8b");
            var tagNameCase11 = "Tresearch Manager Delete Tag3";
            var nodeListCase11 = new List<long> { 8019303365 };
            var resultCase11 = IMessageBank.Responses.notEnabled;

            //User is not confirmed
            IRoleIdentity roleIdentity12 = new RoleIdentity(true, "tagManagerIntegrationNotConfirmed@tresearch.system", "user", "e129733b11ce19340d78c79468ac3723632faede195ee8a78864afdd9a08cc6841feefee84b21a4f48c6e59d182c061b03439fed15f2c8ba8a5022ce1bcaffd3");
            var tagNameCase12 = "Tresearch Manager Delete Tag3";
            var nodeListCase12 = new List<long> { 8019303366 };
            var resultCase12 = IMessageBank.Responses.notConfirmed;

            //UserAccount  does not exist
            IRoleIdentity roleIdentity13 = new RoleIdentity(true, "tagManagerNoAccount@tresearch.system", "user", "");
            var tagNameCase13 = "Tresearch Manager Delete Tag3";
            var nodeListCase13 = new List<long> { 8019303366 };
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

        public static IEnumerable<object[]> GetNodeTagData()
        {
            /*User Authorized, Nodes contain shared tags but also have other tags
             *          8019303356: Tresearch Manager Get Tag1, Tresearch Manager Get Tag2
             *          8019303357: Tresearch Manager Get Tag1, Tresearch Manager Get Tag2
             *          8019303358: Tresearch Manager Get Tag1, Tresearch Manager Get Tag2, Tresearch Manager Get Tag3
             */

            IRoleIdentity roleIdentity0 = new RoleIdentity(true, "tagManagerIntegration3@tresearch.system", "user", "571510127f69c2e3dee263541e8551d8339dc1d98c4b253b5feb5202b41d420dd55c172818feeb5fd7bf85c067c5af142cb930fac9d776b644428adb4b9c4f7b");
            var tagListCase0 = new List<string> { "Tresearch Manager Get Tag1", "Tresearch Manager Get Tag2" };
            var nodeListCase0 = new List<long> { 8019303356, 8019303357, 8019303358 };
            var resultCase0 = IMessageBank.Responses.tagGetSuccess;

            /*User Authorized, Nodes do not contain shared tags
             *          8019303358: Tresearch Manager Get Tag1, Tresearch Manager Get Tag2, Tresearch Manager Get Tag3
             *          8019303359: Tresearch Manager Get Tag2
             *          8019303360: Tresearch Manager Get Tag1
             */
            IRoleIdentity roleIdentity1 = new RoleIdentity(true, "tagManagerIntegration3@tresearch.system", "user", "571510127f69c2e3dee263541e8551d8339dc1d98c4b253b5feb5202b41d420dd55c172818feeb5fd7bf85c067c5af142cb930fac9d776b644428adb4b9c4f7b");
            var tagListCase1 = new List<string> { };
            var nodeListCase1 = new List<long> { 8019303358, 8019303359, 8019303360 };
            var resultCase1 = IMessageBank.Responses.tagGetSuccess;

            //User Authorized, Grab Single Node's Tags (8019303356: Tresearch Manager Get Tag1, Tresearch Manager Get Tag2)
            IRoleIdentity roleIdentity2 = new RoleIdentity(true, "tagManagerIntegration3@tresearch.system", "user", "571510127f69c2e3dee263541e8551d8339dc1d98c4b253b5feb5202b41d420dd55c172818feeb5fd7bf85c067c5af142cb930fac9d776b644428adb4b9c4f7b");
            var tagListCase2 = new List<string> { "Tresearch Manager Get Tag1", "Tresearch Manager Get Tag2" };
            var nodeListCase2 = new List<long> { 8019303356 };
            var resultCase2 = IMessageBank.Responses.tagGetSuccess;

            //User Authorized, Node does not have tag
            IRoleIdentity roleIdentity3 = new RoleIdentity(true, "tagManagerIntegration3@tresearch.system", "user", "571510127f69c2e3dee263541e8551d8339dc1d98c4b253b5feb5202b41d420dd55c172818feeb5fd7bf85c067c5af142cb930fac9d776b644428adb4b9c4f7b");
            var tagListCase3 = new List<string> { };
            var nodeListCase3 = new List<long> { 8019303361 };
            var resultCase3 = IMessageBank.Responses.tagGetSuccess;

            //User Authorized, No node is passed in
            IRoleIdentity roleIdentity4 = new RoleIdentity(true, "tagManagerIntegration3@tresearch.system", "user", "571510127f69c2e3dee263541e8551d8339dc1d98c4b253b5feb5202b41d420dd55c172818feeb5fd7bf85c067c5af142cb930fac9d776b644428adb4b9c4f7b");
            var tagListCase4 = new List<string> { };
            var nodeListCase4 = new List<long> { };
            var resultCase4 = IMessageBank.Responses.nodeNotFound;


            //User not Auhorized For All Nodes
            IRoleIdentity roleIdentity5 = new RoleIdentity(true, "tagManagerIntegration2@tresearch.system", "user", "f59b47456839aadf4328940ee16e473659a48978f5bf81669dee37aac6ecd1a1e380947d68343f3c634378d7964ec573e211e8796036188b417d3265d8fd7a89");
            var tagListCase5 = new List<string> { };
            var nodeListCase5 = new List<long> { 8019303350, 8019303351, 8019303352 };
            var resultCase5 = IMessageBank.Responses.notAuthorized;

            //User not Authorized for some Nodes (user owns 8019303353, 8019303354 but not 8019303350)
            IRoleIdentity roleIdentity6 = new RoleIdentity(true, "tagManagerIntegration2@tresearch.system", "user", "f59b47456839aadf4328940ee16e473659a48978f5bf81669dee37aac6ecd1a1e380947d68343f3c634378d7964ec573e211e8796036188b417d3265d8fd7a89");
            var tagListCase6 = new List<string> { };
            var nodeListCase6 = new List<long> { 8019303350, 8019303353, 8019303354 };
            var resultCase6 = IMessageBank.Responses.notAuthorized;

            //User not Authorized for node
            IRoleIdentity roleIdentity7 = new RoleIdentity(true, "tagManagerIntegration2@tresearch.system", "user", "f59b47456839aadf4328940ee16e473659a48978f5bf81669dee37aac6ecd1a1e380947d68343f3c634378d7964ec573e211e8796036188b417d3265d8fd7a89");
            var tagListCase7 = new List<string> { };
            var nodeListCase7 = new List<long> { 8019303350 };
            var resultCase7 = IMessageBank.Responses.notAuthorized;

            //User has not been authenticated
            IRoleIdentity roleIdentity8 = new RoleIdentity(true, "guest", "guest", "");
            var tagListCase8 = new List<string> { };
            var nodeListCase8 = new List<long> { 8019303350 };
            var resultCase8 = IMessageBank.Responses.notAuthenticated;

            //User is not enabled
            IRoleIdentity roleIdentity9 = new RoleIdentity(true, "tagManagerIntegrationNotEnabled@tresearch.system", "user", "820868f6b568617dca6164bd6d129fd3f0d47ee2da6785c2247f74b5fa174c8b0b4630b4c21e49f410db2293d2c89b2177a8ef5855e34a9b71402d8d57d7de8b");
            var tagListCase9 = new List<string> { };
            var nodeListCase9 = new List<long> { 8019303365 };
            var resultCase9 = IMessageBank.Responses.notEnabled;

            //User is not confirmed
            IRoleIdentity roleIdentity10 = new RoleIdentity(true, "tagManagerIntegrationNotConfirmed@tresearch.system", "user", "e129733b11ce19340d78c79468ac3723632faede195ee8a78864afdd9a08cc6841feefee84b21a4f48c6e59d182c061b03439fed15f2c8ba8a5022ce1bcaffd3");
            var tagListCase10 = new List<string> { };
            var nodeListCase10 = new List<long> { 8019303366 };
            var resultCase10 = IMessageBank.Responses.notConfirmed;

            //UserAccount  does not exist
            IRoleIdentity roleIdentity11 = new RoleIdentity(true, "tagManagerNoAccount@tresearch.system", "user", "");
            var tagListCase11 = new List<string> { };
            var nodeListCase11 = new List<long> { 8019303366 };
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

        public static IEnumerable<object[]> CreateTagData()
        {
            //User is Authenticated and Authorized as admin, tag doesn't exist
            var roleIdentity0 = new RoleIdentity(false, "tagManagerIntegrationAdmin1@tresearch.system", "admin");
            var tagName0 = "Tresearch Manager Doesnt Exist";
            var resultCase0 = IMessageBank.Responses.tagCreateSuccess;

            //User is Authenticated and Authorized as admin, tag exists
            var roleIdentity1 = new RoleIdentity(false, "tagManagerIntegrationAdmin1@tresearch.system", "admin");
            var tagName1 = "Tresearch Manager Tag Exist";
            var resultCase1 = IMessageBank.Responses.tagDuplicate;

            //User is Authenticated but not authorized (user)
            var roleIdentity2 = new RoleIdentity(false, "tagManagerIntegration1@gmail.com", "user");
            var tagName2 = "Tresearch Manager Doesnt Managaer";
            var resultCase2 = IMessageBank.Responses.notAuthorized;

            //User is not Authenticated
            var roleIdentity3 = new RoleIdentity(false, "guest", "guest");
            var tagName3 = "Tresearch Manager Doesnt Managaer";
            var resultCase3 = IMessageBank.Responses.notAuthenticated;


            //UserAccount  does not exist
            var roleIdentity4 = new RoleIdentity(false, "tagManagerNoAccount@tresearch.system", "admin");
            var tagName4 = "Tresearch Manager Doesnt Managaer";
            var resultCase4 = IMessageBank.Responses.accountNotFound;

            return new[]
           {
                new object[] { roleIdentity0, tagName0, resultCase0 },
                new object[] { roleIdentity1, tagName1, resultCase1 },
                new object[] { roleIdentity2, tagName2, resultCase2 },
                new object[] { roleIdentity3, tagName3, resultCase3 },
                new object[] { roleIdentity4, tagName4, resultCase4 }
            };
        }

        public static IEnumerable<object[]> GetTagData()
        {
            //User is Authenticated and Authorized 
            var roleIdentity0 = new RoleIdentity(false, "tagManagerIntegration1@tresearch.system", "user");
            var resultCase0 = IMessageBank.Responses.tagGetSuccess;

            //User is Authenticated and Authorized and Admin
            var roleIdentity1 = new RoleIdentity(false, "tagManagerIntegrationAdmin1@tresearch.system", "admin");
            var resultCase1 = IMessageBank.Responses.tagGetSuccess;

            //User has not been authenticated
            var roleIdentity2 = new RoleIdentity(false, "guest", "guest");
            var resultCase2 = IMessageBank.Responses.tagGetSuccess;

            //User is not enabled
            var roleIdentity3 = new RoleIdentity(false, "tagManagerIntegrationNotEnabled@tresearch.system", "user");
            var resultCase3 = IMessageBank.Responses.tagGetSuccess;

            //User is not confirmed
            var roleIdentity4 = new RoleIdentity(false, "tagManagerIntegrationNotConfirmed@tresearch.system", "user");
            var resultCase4 = IMessageBank.Responses.tagGetSuccess;

            //User is has unknown role
            var roleIdentity5 = new RoleIdentity(false, "tagManagerIntegrationAdmin1@tresearch.system", "wrong");
            var resultCase5 = IMessageBank.Responses.tagGetSuccess;

            //UserAccount not found
            var roleIdentity6 = new RoleIdentity(false, "tagManagerNoAccount@tresearch.system", "user");
            var resultCase6 = IMessageBank.Responses.tagGetSuccess;

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

        public static IEnumerable<object[]> DeleteTagData()
        {
            //User is Authenticated and Authorized as admin, tag doesn't exist
            var roleIdentity0 = new RoleIdentity(false, "tagManagerIntegrationAdmin1@tresearch.system", "admin");
            var tagName0 = "Tresearch Manager REMOVE Doesnt Exist";
            var resultCase0 = IMessageBank.Responses.tagDeleteSuccess;

            //User is Authenticated and Authorized as admin, tag exists
            var roleIdentity1 = new RoleIdentity(false, "tagManagerIntegrationAdmin1@tresearch.system", "admin");
            var tagName1 = "Tresearch Manager REMOVE Tag Exist";
            var resultCase1 = IMessageBank.Responses.tagDeleteSuccess;

            //User is Authenticated but not authorized (user)
            var roleIdentity2 = new RoleIdentity(false, "tagManagerIntegration1@gmail.com", "user");
            var tagName2 = "Tresearch Manager Doesnt Matter";
            var resultCase2 = IMessageBank.Responses.notAuthorized;

            //User is not Authenticated
            var roleIdentity3 = new RoleIdentity(false, "guest", "guest");
            var tagName3 = "Tresearch Manager Doesnt Matter";
            var resultCase3 = IMessageBank.Responses.notAuthenticated;

            //UserAccount  does not exist
            var roleIdentity4 = new RoleIdentity(false, "tagManagerNoAccount@tresearch.system", "admin");
            var tagName4 = "Tresearch Manager Doesnt Managaer";
            var resultCase4 = IMessageBank.Responses.accountNotFound;

            //User is Authenticated and Authorized as admin, tag exists and other nodes have this tag
            var roleIdentity5 = new RoleIdentity(false, "tagManagerIntegrationAdmin1@tresearch.system", "admin");
            var tagName5 = "Tresearch Manager REMOVE Exist and Tagged";
            var resultCase5 = IMessageBank.Responses.tagDeleteSuccess;

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

    }
}
