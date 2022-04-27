using Microsoft.AspNetCore.Mvc;
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
using TrialByFire.Tresearch.WebApi.Controllers.Contracts;
using TrialByFire.Tresearch.WebApi.Controllers.Implementations;
using Xunit;

namespace TrialByFire.Tresearch.Tests.UnitTests.CopyAndPaste
{
    public class InMemoryCopyAndPasteControllerShould : TestBaseClass
    {
        public InMemoryCopyAndPasteControllerShould() : base(new string[] { })
        {
            TestServices.AddScoped<ISqlDAO, InMemorySqlDAO>();
            TestServices.AddScoped<ICopyAndPasteService, CopyAndPasteService>();
            TestServices.AddScoped<ICopyAndPasteManager, CopyAndPasteManager>();
            TestServices.AddScoped<ICopyAndPasteController, CopyAndPasteController>();
            TestProvider = TestServices.BuildServiceProvider();
        }

        /*

        [Theory]
        [InlineData()]

        public async Task CopyNodeAsync(List<long> nodeIDs, IMessageBank.Responses response)
        {
            ICopyAndPasteController copyAndPasteController = TestProvider.GetService<ICopyAndPasteController>();
            IMessageBank messageBank = TestProvider.GetService<IMessageBank>();
            IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
            Thread.CurrentPrincipal rolePrincipal;
        }

        */


    }
}
