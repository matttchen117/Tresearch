using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Models;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Services.Contracts;

namespace TrialByFire.Tresearch.Managers.Implementations
{
    public class AccountDeletionManager : IAccountDeletionManager
    {
        private ISqlDAO SqlDAO { get; }
        private ILogService LogService { get; }
        private IAccountDeletionService AccountDeletionService { get; }

        private IMessageBank _messageBank { get; }

        private BuildSettingsOptions BuildSettingOptions { get; }

        public AccountDeletionManager(ISqlDAO sqlDAO, ILogService logService, IMessageBank messageBank, IAccountDeletionService accountDeletionService)
        {
            SqlDAO = sqlDAO;
            LogService = logService;
            _messageBank = messageBank;
            AccountDeletionService = accountDeletionService;
        }


        //here i should be checking for last admin or something useful
        public async Task<string> DeleteAccountAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {


                cancellationToken.ThrowIfCancellationRequested();


                string userAuthLevel = Thread.CurrentPrincipal.IsInRole("admin") ? "admin" : "user";
                string admins;

                admins = await AccountDeletionService.GetAmountOfAdmins(cancellationToken).ConfigureAwait(false);


                if (userAuthLevel.Equals("admin"))
                {
                    if (admins.Equals(_messageBank.GetMessage(IMessageBank.Responses.generic)))
                    {
                        return await AccountDeletionService.DeleteAccountAsync(cancellationToken).ConfigureAwait(false);
                    }

                    return await _messageBank.GetMessage(IMessageBank.Responses.lastAdminFail).ConfigureAwait(false);
           
                }
                else
                {
                    return await AccountDeletionService.DeleteAccountAsync(cancellationToken).ConfigureAwait(false);
                }









            }

            catch (OperationCanceledException)
            {
                return await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return ("500: Database " + ex.Message);

            }

        }

    }
}
