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

        private IMessageBank MessageBank { get; }

        private BuildSettingsOptions BuildSettingOptions { get; }

        public AccountDeletionManager(ISqlDAO sqlDAO, ILogService logService, IMessageBank messageBank, IAccountDeletionService accountDeletionService)
        {
            SqlDAO = sqlDAO;
            LogService = logService;
            MessageBank = messageBank;
            AccountDeletionService = accountDeletionService;
        }


        //here i should be checking for last admin or something useful
        public async Task<IActionResult> DeleteAccountAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {


                cancellationToken.ThrowIfCancellationRequested();

                //placeholder variable for if person trying to delete is last admin in database.
                //still creating stored procedure for this value

                List<string> results;

                int fakeAdminCount = 1;
                if (fakeAdminCount > 1 && Thread.CurrentPrincipal.Equals("admin"))
                {
                    results = await AccountDeletionService.DeleteAccountAsync(cancellationToken);
                }



                string split;
                List<string> results = await connection.QueryAsync
                String result;
                Thread.CurrentPrincipal.IsInRole
                if (Thread.CurrentPrincipal.Equals(null))
                {
                    return
                }
                else
                {

                }


                string results = this.AccountDeletionService.DeleteAccount();
                return results;

            }
            catch (Exception ex)
            {

            }

        }

    }
}
