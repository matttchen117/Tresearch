using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Services.Contracts;

namespace TrialByFire.Tresearch.Services.Implementations
{
    public class CrossCuttingDependencies : UniversalDependencies
    {
        public IAuthenticationService authenticationService { get; set; }
        public IAuthorizationService authorizationService { get; set; }

        public IValidationService validationService { get; set; }

        public CrossCuttingDependencies() : base()
        {
            authenticationService = new AuthenticationService();
            authorizationService = new AuthorizationService();
            validationService = new ValidationService();
        }
    }
}
