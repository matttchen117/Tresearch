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


                string userName = Thread.CurrentPrincipal.Identity.Name;
                string userAuthLevel = Thread.CurrentPrincipal.IsInRole("admin") ? "admin" : "user";
                string confirmed = "";
                Tuple<List<INode>, string> resultCopy;

                IAccount account = new UserAccount(userName, userAuthLevel);
                confirmed = await _accountVerificationService.VerifyAccountAsync(account, cancellationToken).ConfigureAwait(false);

                //Account is verified and good to perform operation
                if (confirmed.Equals(await _messageBank.GetMessage(IMessageBank.Responses.verifySuccess).ConfigureAwait(false)))
                {

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
                else
                {
                    return Tuple.Create(new List<INode>(), await _messageBank.GetMessage(IMessageBank.Responses.verificationFailure).ConfigureAwait(false));
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


        public async Task<string> PasteNodeAsync(List<INode> nodes, CancellationToken cancellationToken = default(CancellationToken))
        {

        }

    }
}
