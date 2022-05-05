using Microsoft.AspNetCore.Mvc;
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
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Services.Contracts;

namespace TrialByFire.Tresearch.Managers.Implementations
{
    public class PrivateAndPublicManager : IPrivateAndPublicManager
    {
        private BuildSettingsOptions _buildSettingsOptions { get; }

        private ISqlDAO _sqlDAO { get; }


        private IAccountVerificationService _accountVerificationService { get; }

        /// <summary>
        ///     Manager to perform copy and paste feature feature
        /// </summary>
        private IPrivateAndPublicService _privateAndPublicService { get; }


        /// <summary>
        ///     Model holding string responses to enumerated cases
        /// </summary>
        private IMessageBank _messageBank { get; set; }


        public PrivateAndPublicManager(ISqlDAO sqlDAO, IMessageBank messageBank, IAccountVerificationService accountVerificationService, IPrivateAndPublicService privateAndPublicservice, IOptionsSnapshot<BuildSettingsOptions> buildSettingsOptions)
        {
            _sqlDAO = sqlDAO;
            _messageBank = messageBank;
            _accountVerificationService = accountVerificationService;
            _privateAndPublicService = privateAndPublicservice;
            _buildSettingsOptions = buildSettingsOptions.Value;
        }


        public async Task<IResponse<string>> PrivateNodeAsync(List<long> nodes, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                // Get user's role
                string role = "";
                if (Thread.CurrentPrincipal.IsInRole(_buildSettingsOptions.User))
                {
                    role = _buildSettingsOptions.User;
                }
                else
                {
                    role = _buildSettingsOptions.Admin;
                }

                IAccount account = new UserAccount(Thread.CurrentPrincipal.Identity.Name, role);

                // Calling verifyAccountAsync to authenticate account
                string resultVerifyAccount = await _accountVerificationService.VerifyAccountAsync(account, cancellationToken).ConfigureAwait(false);

                if (!resultVerifyAccount.Equals(await _messageBank.GetMessage(IMessageBank.Responses.verifySuccess).ConfigureAwait(false)))
                {

                    return new PrivateResponse<string>(resultVerifyAccount, null, 401, false);
                }

                IResponse<string> response = await _privateAndPublicService.PrivateNodeAsync(nodes, cancellationToken).ConfigureAwait(false);

                if (!response.IsSuccess)
                {
                    //might need to return a more meaningful statuscode or message indicating what went wrong
                    return new PrivateResponse<string>(await _messageBank.GetMessage(IMessageBank.Responses.copyNodeFailure).ConfigureAwait(false), null, 400, false);
                }

                return response;


            }

            catch (OperationCanceledException)
            {
                //return code for operationCancelled is 500
                return new PrivateResponse<string>(await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested).ConfigureAwait(false), null, 500, false);
            }
            catch (Exception ex)
            {
                //return code for unhandledException is 500
                return new PrivateResponse<string>(await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message, null, 500, false);

            }

        }


        /*


        public async Task<IResponse<string>> PublicNodeAsync(List<long> nodes, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                // Get user's role
                string role = "";
                if (Thread.CurrentPrincipal.IsInRole(_buildSettingsOptions.User))
                {
                    role = _buildSettingsOptions.User;
                }
                else
                {
                    role = _buildSettingsOptions.Admin;
                }

                IAccount account = new UserAccount(Thread.CurrentPrincipal.Identity.Name, role);

                // Calling verifyAccountAsync to authenticate account
                string resultVerifyAccount = await _accountVerificationService.VerifyAccountAsync(account, cancellationToken).ConfigureAwait(false);

                if (!resultVerifyAccount.Equals(await _messageBank.GetMessage(IMessageBank.Responses.verifySuccess).ConfigureAwait(false)))
                {

                    return new PublicResponse<string>(resultVerifyAccount, null, 401, false);
                }




            }

            catch (OperationCanceledException)
            {
                //return code for operationCancelled is 500
                return new PublicResponse<string>(await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested).ConfigureAwait(false), null, 500, false);
            }
            catch (Exception ex)
            {
                //return code for unhandledException is 500
                return new PublicResponse<string>(await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message, null, 500, false);

            }

        }

        */


    }
}
