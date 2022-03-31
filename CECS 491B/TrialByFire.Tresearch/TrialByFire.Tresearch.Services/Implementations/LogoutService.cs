using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Models;
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
        private BuildSettingsOptions _options { get; }

        public LogoutService(ISqlDAO sqlDAO, ILogService logService, IMessageBank messageBank,
            IOptionsSnapshot<BuildSettingsOptions> options)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
            _messageBank = messageBank;
            _options = options.Value;
        }

        //
        // Summary:
        //     Logs the User out
        //
        // Returns:
        //     The result of the logout process.
        public async Task<string> LogoutAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                string role = "";
                if (Thread.CurrentPrincipal.IsInRole(_options.User))
                {
                    role = _options.User;
                }
                else if (Thread.CurrentPrincipal.IsInRole(_options.Admin))
                {
                    role = _options.Admin;
                }
                else
                {
                    return await _messageBank.GetMessage(IMessageBank.Responses.unknownRole)
                        .ConfigureAwait(false);
                }
                IAccount account = new Account(Thread.CurrentPrincipal.Identity.Name, role);
                int result = await _sqlDAO.LogoutAsync(account, cancellationToken).ConfigureAwait(false);
                switch (result)
                {
                    case 0:
                        return await _messageBank.GetMessage(IMessageBank.Responses.accountNotFound)
                            .ConfigureAwait(false);
                    case 1:
                        return await _messageBank.GetMessage(IMessageBank.Responses.logoutSuccess)
                            .ConfigureAwait(false);
                    case 2:
                        return await _messageBank.GetMessage(IMessageBank.Responses.duplicateAccountData)
                            .ConfigureAwait(false);
                    default:
                        throw new NotImplementedException();
                };
            }catch(Exception ex)
            {
                return _options.UncaughtExceptionMessage + ex.Message;
            }
        }
    }
}
