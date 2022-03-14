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


        private string _sqlConnectionString = "Data Source=tresearchstudentserver.database.windows.net;Initial Catalog=tresearchStudentServer;User ID=tresearchadmin;Password=CECS491B!;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";


        public IntegrationTestDependencies()
        {
            messageBank = new MessageBank();
            sqlDAO = new SqlDAO(_sqlConnectionString, messageBank);
            logService = new SqlLogService(sqlDAO);
            authenticationService = new AuthenticationService(sqlDAO, logService, messageBank);
            authorizationService = new AuthorizationService(sqlDAO, logService);
            validationService = new ValidationService(messageBank);
            accountDeletionService = new AccountDeletionService(sqlDAO, logService, rolePrincipal);
        }
    }
}
