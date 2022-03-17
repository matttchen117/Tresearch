using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Models;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Services.Implementations;

namespace TrialByFire.Tresearch.Tests
{
    public class IntegrationTestDependencies
    {
        private BuildSettingsOptions _buildSettingsOptions { get; }
        public IOptions<BuildSettingsOptions> BuildSettingsOptions { get; }
        public ISqlDAO SqlDAO { get; }
        public ILogService SqlLogService { get; }
        public IMessageBank MessageBank { get; }
        public IAuthenticationService AuthenticationService { get; }
        public IAuthorizationService AuthorizationService { get; }
        public IValidationService ValidationService { get; }
        public IAccountDeletionService AccountDeletionService { get; }

        public IntegrationTestDependencies()
        {
            _buildSettingsOptions = new BuildSettingsOptions()
            {
                Environment = "Test",
                SqlConnectionString = "Server=MATTS-PC;Initial Catalog=TrialByFire.Tresearch.IntegrationTestDB; Integrated Security=true",
                SendGridAPIKey = ""
            };
            BuildSettingsOptions = Options.Create(_buildSettingsOptions);


            MessageBank = new MessageBank();
            SqlDAO = new SqlDAO(MessageBank, BuildSettingsOptions);
            SqlLogService = new SqlLogService(SqlDAO);
            AuthenticationService = new AuthenticationService(SqlDAO, SqlLogService, MessageBank);
            AuthorizationService = new AuthorizationService(SqlDAO, SqlLogService);
            ValidationService = new ValidationService(MessageBank);
            AccountDeletionService = new AccountDeletionService(SqlDAO, SqlLogService);
        }
    }
}
