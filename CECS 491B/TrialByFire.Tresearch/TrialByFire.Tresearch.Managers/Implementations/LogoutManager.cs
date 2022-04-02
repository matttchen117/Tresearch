using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Models;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Services.Contracts;

namespace TrialByFire.Tresearch.Managers.Implementations
{
    // Summary:
    //     A manager class for enforcing the business rules for logging a User out and calling the
    //     appropriate services for the operation.
    public class LogoutManager : ILogoutManager
    {
        private ISqlDAO _sqlDAO { get; }
        private ILogService _logService { get; }

        private IMessageBank _messageBank { get; }
        private ILogoutService _logoutService { get; }
        private BuildSettingsOptions _options { get; }

        public LogoutManager(ISqlDAO sqlDAO, ILogService logService, IMessageBank messageBank, 
            ILogoutService logoutService, IOptionsSnapshot<BuildSettingsOptions> options)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
            _messageBank = messageBank;
            _logoutService = logoutService;
            _options = options.Value;
        }

        //
        // Summary:
        //     Checks that the User is currently Authenticated. Calls the appropriate service for the
        //     operation.
        //
        // Returns:
        //     The result of the operation.
        public async Task<string> LogoutAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if(Thread.CurrentPrincipal != null)
            {
                try
                {
                    return await _logoutService.LogoutAsync(cancellationToken).ConfigureAwait(false);
                }catch (Exception ex)
                {
                    return _options.UncaughtExceptionMessage + ex.Message;
                }
            }
            return await _messageBank.GetMessage(IMessageBank.Responses.notAuthenticated)
                .ConfigureAwait(false);
        }
    }
}
