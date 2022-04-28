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


        public CopyAndPasteManager(ISqlDAO sqlDAO, IMessageBank messageBank, IAccountVerificationService accountVerificationService, IOptionsSnapshot<BuildSettingsOptions> options)
        {
            _sqlDAO = sqlDAO;
            _messageBank = messageBank;
            _accountVerificationService = accountVerificationService;
            _buildSettingsOptions = options.Value;

        }



        public async Task<Tuple<List<INode>,string>> CopyNodeAsync(List<long> nodeIDs, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();


                if(Thread.CurrentPrincipal == null || Thread.CurrentPrincipal.Identity.Name.Equals("guest"))
                {
                    return Tuple.Create(new List<INode>(), await _messageBank.GetMessage(IMessageBank.Responses.verificationFailure).ConfigureAwait(false));
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
                    return Tuple.Create(new List<INode>(), await _messageBank.GetMessage(IMessageBank.Responses.unknownRole).ConfigureAwait(false));
                }


                IAccount account = new UserAccount(Thread.CurrentPrincipal.Identity.Name, role);

                string resultVerifyAccount = await _accountVerificationService.VerifyAccountAsync(account, cancellationToken).ConfigureAwait(false);

                if (!resultVerifyAccount.Equals(await _messageBank.GetMessage(IMessageBank.Responses.verifySuccess).ConfigureAwait(false)))
                {
                    return Tuple.Create(new List<INode>(), resultVerifyAccount);
                }

                //Do i need to check for string resultVerifyAuthorized = await _accountVerificationService.VerifyAccountAuthorizedNodeChangesAsync(nodeIDs, userHash, cancellationToken);



                Tuple<List<INode>, string> resultCopy;



                if(nodeIDs == null || nodeIDs.Count <= 0)
                {
                    return Tuple.Create(new List<INode>(), await _messageBank.GetMessage(IMessageBank.Responses.copyNodeEmptyError).ConfigureAwait(false));
                }
                else
                {
                    resultCopy = await _copyAndPasteService.CopyNodeAsync(nodeIDs, cancellationToken).ConfigureAwait(false);
                    return resultCopy;
                }

            }

            catch (OperationCanceledException)
            {
                return Tuple.Create(new List<INode>(), await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested).ConfigureAwait(false));
            }
            catch (Exception ex)
            {
                return Tuple.Create(new List<INode>(), await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message);

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


                IAccount account = new UserAccount(Thread.CurrentPrincipal.Identity.Name, role);

                string resultVerifyAccount = await _accountVerificationService.VerifyAccountAsync(account, cancellationToken).ConfigureAwait(false);

                if (!resultVerifyAccount.Equals(await _messageBank.GetMessage(IMessageBank.Responses.verifySuccess).ConfigureAwait(false)))
                {
                    return resultVerifyAccount;
                }

                string resultPaste;



                if (nodes == null || nodes.Count <= 0)
                {
                    return await _messageBank.GetMessage(IMessageBank.Responses.pasteNodeEmptyError).ConfigureAwait(false));
                }
                else
                {
                    resultPaste = await _copyAndPasteService.PasteNodeAsync(nodeToPasteTo, nodes, cancellationToken).ConfigureAwait(false);
                    return resultPaste;
                }






            }


            catch (OperationCanceledException)
            {
                return Tuple.Create(new List<INode>(), await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested).ConfigureAwait(false));
            }
            catch (Exception ex)
            {
                return Tuple.Create(new List<INode>(), await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message);

            }
        }

    }
}
