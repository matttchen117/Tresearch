using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.BaseClasses.Contracts;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Services.Contracts;

namespace TrialByFire.Tresearch.BaseClasses.Implementations
{
    public class CrossCuttingDependencies : ICrossCuttingDependencies
    {
        public ISqlDAO sqlDAO { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ILogService logService { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IAuthenticationService authenticationService { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
