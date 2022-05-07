using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
using Xunit;

namespace TrialByFire.Tresearch.Tests.IntegrationTests.CreateNode
{
    public class CreateNodeManagerShould : TestBaseClass
    {
        public CreateNodeManagerShould() : base(new string[] { })
        {
            //TestServices.AddScoped<ISqlDAO, InMemorySqlDAO>();
            TestServices.AddScoped<ICreateNodeService, CreateNodeService>();
            TestServices.AddScoped<ICreateNodeManager, CreateNodeManager>();
            TestProvider = TestServices.BuildServiceProvider();
        }

        [Theory]
        [InlineData("75250943621632BA2A2B7BF4FAC0C05F2AC9D5FB5109A6B3E242177B6DE1B23571B134A3DEAD2C45C00D997862A206650A2ADC01881E2E03D80942EF5D6608F6", 
            1702, "Barbell Bench", "Bench for ORM", true, false, "jelazo@live.com", "user", "200")]
        public async Task CreateTheNode(string userhash, long parentID, string nodeTitle, string summary, bool visibility, bool deleted,
            string accountOwner, string currentRole, string expected)
        {
            // Arrange
            IRoleIdentity roleIdentity = new RoleIdentity(true, accountOwner, currentRole, userhash);
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            Thread.CurrentPrincipal = rolePrincipal;

            ICreateNodeManager createNodeManager = TestProvider.GetService<ICreateNodeManager>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            Node node = new Node(userhash, 0, parentID, nodeTitle, summary, DateTime.UtcNow, visibility, deleted);

            // Act
            IResponse<string> result = await createNodeManager.CreateNodeAsync(userhash, node, cancellationTokenSource.Token).ConfigureAwait(false);

            // Assert
            Assert.Equal(expected, result.Data);
        }

    }
}
