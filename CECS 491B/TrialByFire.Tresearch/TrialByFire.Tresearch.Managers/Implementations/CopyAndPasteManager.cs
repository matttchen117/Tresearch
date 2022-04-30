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
    public class CopyAndPasteManager : ICopyAndPasteManager
    {
        private BuildSettingsOptions _buildSettingsOptions { get; }

        private ISqlDAO _sqlDAO { get; }


        private IAccountVerificationService _accountVerificationService { get; }

        /// <summary>
        ///     Manager to perform copy and paste feature feature
        /// </summary>
        private ICopyAndPasteService _copyAndPasteService { get; }


        /// <summary>
        ///     Model holding string responses to enumerated cases
        /// </summary>
        private IMessageBank _messageBank { get; set; }


        public CopyAndPasteManager(ISqlDAO sqlDAO, IMessageBank messageBank, IAccountVerificationService accountVerificationService, ICopyAndPasteService copyAndPasteService, IOptionsSnapshot<BuildSettingsOptions> options)
        {
            _sqlDAO = sqlDAO;
            _messageBank = messageBank;
            _accountVerificationService = accountVerificationService;
            _copyAndPasteService = copyAndPasteService;
            _buildSettingsOptions = options.Value;

        }



        //public async Task<Tuple<List<INode>, string>> CopyNodeAsync(List<INode> nodesCopy, CancellationToken cancellationToken = default(CancellationToken))
        public async Task<IResponse<IEnumerable<Node>>> CopyNodeAsync(List<long> nodesCopy, CancellationToken cancellationToken = default(CancellationToken))

        {

            try
            {

                cancellationToken.ThrowIfCancellationRequested();

                //IResponse<IEnumerable<Node>> response = await _copyAndPasteService.CopyNodeAsync(nodesCopy, cancellationToken);

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

                string resultVerifyAccount = await _accountVerificationService.VerifyAccountAsync(account, cancellationToken).ConfigureAwait(false);

                if (!resultVerifyAccount.Equals(await _messageBank.GetMessage(IMessageBank.Responses.verifySuccess).ConfigureAwait(false)))
                {

                    return new CopyResponse<IEnumerable<Node>>(resultVerifyAccount, null, 401, false);
                }

                if(nodesCopy == null || nodesCopy.Count <= 0)
                {
                    return new CopyResponse<IEnumerable<Node>>(await _messageBank.GetMessage(IMessageBank.Responses.copyNodeEmptyError).ConfigureAwait(false), null, 400, false);
                }

                IResponse<IEnumerable<Node>> response = await _copyAndPasteService.CopyNodeAsync(nodesCopy, cancellationToken).ConfigureAwait(false);


                if (!response.IsSuccess)
                {
                    //might need to return a more meaningful statuscode or message indicating what went wrong
                    return new CopyResponse<IEnumerable<Node>>(await _messageBank.GetMessage(IMessageBank.Responses.copyNodeFailure).ConfigureAwait(false), null, 400, false);
                }

                return response;

            }

            catch (OperationCanceledException)
            {
                //return code for operationCancelled is 500
                return new CopyResponse<IEnumerable<Node>>(await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested).ConfigureAwait(false), null, 500, false);
            }
            catch (Exception ex)
            {
                //return code for unhandledException is 500
                return new CopyResponse<IEnumerable<Node>>(await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message, null, 500, false);

            }
        }


        public async Task<string> PasteNodeAsync(INode nodeToPasteTo, List<INode> nodes, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();


                if (Thread.CurrentPrincipal == null || Thread.CurrentPrincipal.Identity.Name.Equals("guest"))
                {
                    return await _messageBank.GetMessage(IMessageBank.Responses.verificationFailure).ConfigureAwait(false);
                }

                // Get user's role
                string role = "";
                if (Thread.CurrentPrincipal.IsInRole(_buildSettingsOptions.User))
                {
                    role = _buildSettingsOptions.User;
                }
                else if (Thread.CurrentPrincipal.IsInRole(_buildSettingsOptions.Admin))
                {
                    role = _buildSettingsOptions.Admin;
                }
                else
                {
                    return await _messageBank.GetMessage(IMessageBank.Responses.unknownRole).ConfigureAwait(false);
                }

                //getting userhash to change the data for pasting in new nodes
                string userHash = (Thread.CurrentPrincipal.Identity as IRoleIdentity).UserHash;

                IAccount account = new UserAccount(Thread.CurrentPrincipal.Identity.Name, role);


                string resultVerifyAccount = await _accountVerificationService.VerifyAccountAsync(account, cancellationToken).ConfigureAwait(false);

                if (!resultVerifyAccount.Equals(await _messageBank.GetMessage(IMessageBank.Responses.verifySuccess).ConfigureAwait(false)))
                {
                    return resultVerifyAccount;
                }

                //need to check if node pasting to is a leaf, doing the check here








                string resultPaste;



                if (nodes == null || nodes.Count <= 0)
                {
                    return await _messageBank.GetMessage(IMessageBank.Responses.pasteNodeEmptyError).ConfigureAwait(false);
                }
                else
                {
                    resultPaste = await _copyAndPasteService.PasteNodeAsync(nodeToPasteTo, nodes, cancellationToken).ConfigureAwait(false);
                    return resultPaste;
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
