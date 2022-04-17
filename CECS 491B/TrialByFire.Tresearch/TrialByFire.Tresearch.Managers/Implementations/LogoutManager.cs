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
        private IMessageBank _messageBank { get; }
        private BuildSettingsOptions _options { get; }

        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource(
            TimeSpan.FromSeconds(5));

        public LogoutManager(IMessageBank messageBank, IOptionsSnapshot<BuildSettingsOptions> options)
        {
            _messageBank = messageBank;
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
            if(!Thread.CurrentPrincipal.Identity.Name.Equals("guest"))
            {
                return await _messageBank.GetMessage(IMessageBank.Responses.logoutSuccess)
                .ConfigureAwait(false);
            }
            return await _messageBank.GetMessage(IMessageBank.Responses.notAuthenticated)
                .ConfigureAwait(false);
        }
    }
}
