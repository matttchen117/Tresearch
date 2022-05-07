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

namespace TrialByFire.Tresearch.Tests.UnitTests.DeleteNode
{
    public class InMemoryDeleteNodeManagerShould : TestBaseClass
    {
        public InMemoryDeleteNodeManagerShould() : base(new string[] { })
        {
            TestServices.AddScoped<ISqlDAO, InMemorySqlDAO>();
            TestServices.AddScoped<IDeleteNodeService, DeleteNodeService>();
            TestServices.AddScoped<IDeleteNodeManager, DeleteNodeManager>();
            TestProvider = TestServices.BuildServiceProvider();
        }

        [Theory]
        [InlineData("51549CF94E96FED6DB3B43BD4B3A989B77CC44E481D40BF86A262D081B029C9CEBE4E4D228A288301408797DD30CC094B7814ACB87695D0ACCE0A28C5FA9B126",
            14, 12, "jelazo@live.com", "user", "200: Server: Delete Node Success")]
        public async Task DeleteTheNode(string userhash, long nodeID, long parentID, string currentIdentity, 
            string currentRole, string expected)
        {
            //Arrange
            IRoleIdentity roleIdentity = new RoleIdentity(true, currentIdentity, currentRole, userhash);
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            Thread.CurrentPrincipal = rolePrincipal;
            IDeleteNodeManager deleteNodeManager = TestProvider.GetService<IDeleteNodeManager>();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            //Act
            IResponse<string> result = await deleteNodeManager.DeleteNodeAsync(userhash, nodeID, parentID, cancellationTokenSource.Token);

            //Assert
            //Assert.Equal(expected, result);
        }
    }
}
