using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Services.Contracts;

namespace TrialByFire.Tresearch.Services.Implementations
{
    // Summary:
    //     A service class for Logging the User Out
    public class LogoutService : ILogoutService
    {
        private ISqlDAO _sqlDAO { get; }
        private ILogService _logService { get; }

        private IMessageBank _messageBank { get; }

        public LogoutService(ISqlDAO sqlDAO, ILogService logService, IMessageBank messageBank)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
            _messageBank = messageBank;
        }

        //
        // Summary:
        //     Logs the User out
        //
        // Returns:
        //     The result of the logout process.
        public async Task<string> Logout(CancellationToken cancellationToken)
        {
            Thread.CurrentPrincipal = null;
            // this actually seems unnecessary, so not sure what
            // this service should actually do
            if (Thread.CurrentPrincipal == null)
            {
                return _messageBank.SuccessMessages["generic"];
            }
            return _messageBank.ErrorMessages["logoutFail"];
        }
    }
}
