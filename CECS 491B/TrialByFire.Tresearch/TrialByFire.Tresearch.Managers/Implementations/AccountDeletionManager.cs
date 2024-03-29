﻿using Microsoft.AspNetCore.Mvc;
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
using TrialByFire.Tresearch.Models.Implementations;
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
        private IAccountVerificationService _accountVerificationService { get; }

        public AccountDeletionManager(ISqlDAO sqlDAO, ILogService logService, IMessageBank messageBank, IAccountDeletionService accountDeletionService, IOptionsSnapshot<BuildSettingsOptions> options, IAccountVerificationService accountVerificationService)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
            _messageBank = messageBank;
            _accountDeletionService = accountDeletionService;
            _options = options.Value;
            _accountVerificationService = accountVerificationService;
        }

        /// <summary>
        /// Manager layer for account deletion, contains business rules for deletion. Verifies by creating account based on current thread using username and role
        /// and verifies using accountVerificationService
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>Message indicating if there are enough admins left, or message</returns>
       
        public async Task<string> DeleteAccountAsync(CancellationToken cancellationToken = default(CancellationToken))
        {

            try
            {
                if (Thread.CurrentPrincipal.Equals(null))
                {
                    return await _messageBank.GetMessage(IMessageBank.Responses.notAuthorized).ConfigureAwait(false);
                }

                cancellationToken.ThrowIfCancellationRequested();
                string userName = Thread.CurrentPrincipal.Identity.Name;
                string userAuthLevel = Thread.CurrentPrincipal.IsInRole("admin") ? "admin" : "user";
                string admins = "";
                string confirmed = "";

                //verifying account here
                IAccount account = new UserAccount(userName, userAuthLevel);
                confirmed = await _accountVerificationService.VerifyAccountAsync(account, cancellationToken).ConfigureAwait(false);


                if (confirmed.Equals(await _messageBank.GetMessage(IMessageBank.Responses.verifySuccess).ConfigureAwait(false)))
                {
                    if (userAuthLevel.Equals("admin"))
                    {
                        admins = await _accountDeletionService.GetAmountOfAdminsAsync(cancellationToken).ConfigureAwait(false);

                        if (admins.Equals(await _messageBank.GetMessage(IMessageBank.Responses.getAdminsSuccess).ConfigureAwait(false)))
                        {
                            return await _accountDeletionService.DeleteAccountAsync(cancellationToken).ConfigureAwait(false);
                        }
                        return await _messageBank.GetMessage(IMessageBank.Responses.getAdminsSuccess).ConfigureAwait(false);

                    }
                    else
                    {
                        return await _accountDeletionService.DeleteAccountAsync(cancellationToken).ConfigureAwait(false);
                    }
                }
                else
                {
                    return await _messageBank.GetMessage(IMessageBank.Responses.accountNotFound);
                }
            }
            catch (OperationCanceledException)
            {
                return await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message;

            }

        }

    }
}
