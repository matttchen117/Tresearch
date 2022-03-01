using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrialByFire.Tresearch.Tests.Contracts
{
    // Base class for DI for tests (work on later)
    public class BaseTest
    {
        public BaseTest()
        {
            var services = new ServiceCollection();

            services.AddTransient<>();

        }
    }
}
