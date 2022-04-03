using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Exceptions;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Models;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Services.Contracts;

namespace TrialByFire.Tresearch.Managers.Implementations
{
    public class AccountDeletionManager : IAccountDeletionManager
    {
        private ISqlDAO _sqlDAO { get; }
        private ILogService _logService { get; }
        private IMessageBank _messageBank { get; }
        private IAccountDeletionService _accountDeletionService { get; }
        private BuildSettingsOptions _options { get; }

        public AccountDeletionManager(ISqlDAO sqlDAO, ILogService logService, IMessageBank messageBank, IAccountDeletionService accountDeletionService, IOptionsSnapshot<BuildSettingsOptions> options)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
            _messageBank = messageBank;
            _accountDeletionService = accountDeletionService;
            _options = options.Value;

        }


        public async Task<string> DeleteAccountAsync(CancellationToken cancellationToken = default(CancellationToken))
        {

            try
            {

                cancellationToken.ThrowIfCancellationRequested();

                string userAuthLevel = Thread.CurrentPrincipal.IsInRole("admin") ? "admin" : "user";
                string admins = "";



                if (userAuthLevel.Equals("admin"))
                {
                    admins = await _accountDeletionService.GetAmountOfAdminsAsync(cancellationToken).ConfigureAwait(false);

                    if (admins.Equals(await _messageBank.GetMessage(IMessageBank.Responses.getAdminsSuccess).ConfigureAwait(false)))
                    {
                        return await _accountDeletionService.DeleteAccountAsync(cancellationToken).ConfigureAwait(false);
                    }

                    return await _messageBank.GetMessage(IMessageBank.Responses.lastAdminFail).ConfigureAwait(false);

                }
                else
                {
                    return await _accountDeletionService.DeleteAccountAsync(cancellationToken).ConfigureAwait(false);
                }



            }

            catch (OperationCanceledException)
            {
                return await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested).ConfigureAwait(false);
            }



            //might be returning wrong thing here
            catch (Exception ex)
            {
                return _options.UncaughtExceptionMessage + ex.Message;

            }

        }

    }
}
