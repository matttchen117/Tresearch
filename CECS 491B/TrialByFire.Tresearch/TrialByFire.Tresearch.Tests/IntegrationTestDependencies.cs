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

namespace TrialByFire.Tresearch.Tests
{
    public class IntegrationTestDependencies
    {
        public ISqlDAO sqlDAO { get; }
        public ILogService logService { get; }
        public IMessageBank messageBank { get; }
        public IAuthenticationService authenticationService { get; }
        public IAuthorizationService authorizationService { get; }
        public IValidationService validationService { get; }
        public IAccountDeletionService accountDeletionService { get; }
        public IRolePrincipal rolePrincipal { get; }


        private string _connectionString = "Server=MATTS-PC;Initial Catalog=TrialByFire.Tresearch.IntegrationTestDB; Integrated Security=true";


        public IntegrationTestDependencies()
        {
            messageBank = new MessageBank();
            sqlDAO = new SqlDAO(_connectionString, messageBank);
            logService = new SqlLogService(sqlDAO);
            authenticationService = new AuthenticationService(sqlDAO, logService, messageBank);
            authorizationService = new AuthorizationService(sqlDAO, logService);
            validationService = new ValidationService(messageBank);
            accountDeletionService = new AccountDeletionService(sqlDAO, logService, rolePrincipal);
        }
    }
}
