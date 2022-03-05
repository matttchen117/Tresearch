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

namespace TrialByFire.Tresearch.Services.Implementations
{
    public class InMemoryTestDependencies
    {
        public ISqlDAO sqlDAO { get; set; }
        public ILogService logService { get; set; }
        public IAuthenticationService authenticationService { get; set; }
        public IAuthorizationService authorizationService { get; set; }

        public IValidationService validationService { get; set; }

        public InMemoryTestDependencies()
        {
            sqlDAO = new InMemorySqlDAO();
            logService = new InMemoryLogService(sqlDAO);
            authenticationService = new AuthenticationService(sqlDAO, logService);
            authorizationService = new AuthorizationService(sqlDAO, logService);
            validationService = new ValidationService();
        }
    }
}
