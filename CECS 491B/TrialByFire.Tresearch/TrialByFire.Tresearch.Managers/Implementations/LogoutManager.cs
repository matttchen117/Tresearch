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
    /// <summary>
    ///     LogoutManager: Class that is part of the Manager abstraction layer that handles business rules related to Logout
    /// </summary>
    public class LogoutManager : ILogoutManager
    {
        private IMessageBank _messageBank { get; }
        private BuildSettingsOptions _options { get; }

        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource(
            TimeSpan.FromSeconds(5));

        /// <summary>
        ///     public LogoutManager():
        ///         Constructor for LogoutManager class
        /// </summary>
        /// <param name="messageBank">Object that contains error and success messages</param>
        /// <param name="options">Snapshot object that represents the setings/configurations of the application</param>
        public LogoutManager(IMessageBank messageBank, IOptionsSnapshot<BuildSettingsOptions> options)
        {
            _messageBank = messageBank;
            _options = options.Value;
        }

        /// <summary>
        ///     LogoutAsync:
        ///         Async method that checks business rules related to Logout
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>The result of the operation</returns>
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
