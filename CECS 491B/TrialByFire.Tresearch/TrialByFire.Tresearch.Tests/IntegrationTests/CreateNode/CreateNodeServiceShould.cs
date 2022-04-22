using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Services.Implementations;
using TrialByFire.Tresearch.WebApi;
using Xunit;

namespace TrialByFire.Tresearch.Tests.IntegrationTests.CreateNode
{
    public class CreateNodeServiceShould : TestBaseClass
    {
        public CreateNodeServiceShould() : base(new string[] { })
        {
            //TestServices.AddScoped<ISqlDAO, InMemorySqlDAO>();
            TestServices.AddScoped<ICreateNodeService, CreateNodeService>();
            TestProvider = TestServices.BuildServiceProvider();
        }

        [Theory]
        /*[InlineData("jessie@gmail.com", 69420, 3, "Cooking", "Concepts of Preparing Food", true, false, "jessie@gmail.com", "user", "200: Server: Create Node Success")]
        [InlineData("jessie@gmail.com", 69420, 2, "Cooking", "Concepts of Preparing Food", true, false, "jessie@gmail.com", "user", "200: Server: Create Node Success")]*/
        [InlineData("jelazo@live.com", 6969, null, "Root", "Root Node", true, false, false, "jelazo@live.com", "user", "200: Server: Create Node Success")]
        public async Task CreateTheNode(string username, long nodeID, long parentID, string nodeTitle, string summary, bool visibility, bool mode, bool deleted,
            string accountOwner, string currentRole, string expected)
        {
            //Arrange
            //Arrange
            IRoleIdentity roleIdentity = new RoleIdentity(false, username, currentRole);
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            if (!username.Equals("guest"))
            {
                Thread.CurrentPrincipal = rolePrincipal;
            }
            ICreateNodeService createNodeService = TestProvider.GetRequiredService<ICreateNodeService>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            Node node = new Node("AD89551B3BF5021B53AC0C9878DE96EAB72816241C417DDF2FB421BD78B7B7477372245C5EF36FEEE1A5DB096596D170309A904D9D0FDA6FAD4071148AD67C75",
                nodeID, parentID, nodeTitle, summary, mode, deleted);
            Account account = new Account(username, "jessie123", "user");

            //Act
            string result = await createNodeService.CreateNodeAsync(account, node, cancellationTokenSource.Token).ConfigureAwait(false);

            //Assert
            Assert.Equal(expected, result);
        }
    }
}
