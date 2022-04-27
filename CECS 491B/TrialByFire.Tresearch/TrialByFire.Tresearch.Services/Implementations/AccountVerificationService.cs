using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Services.Contracts;

namespace TrialByFire.Tresearch.Services.Implementations
{
    public class AccountVerificationService : IAccountVerificationService
    {
        private ISqlDAO _sqlDAO { get; }
        private ILogService _logService { get; }
        private IMessageBank _messageBank { get; }
        public AccountVerificationService(ISqlDAO sqlDAO, ILogService logService, IMessageBank messageBank)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
            _messageBank = messageBank;
        }

        /// <summary>
        ///  Verifies if an account exists and is enabled.
        /// </summary>
        /// <param name="account">User's Account</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>String result</returns>
        public async Task<string> VerifyAccountAsync(IAccount account, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                int result = await _sqlDAO.VerifyAccountAsync(account, cancellationToken).ConfigureAwait(false);
                switch (result)
                {
                    case 0:
                        return await _messageBank.GetMessage(IMessageBank.Responses.accountNotFound).ConfigureAwait(false);
                    case 1:
                        return await _messageBank.GetMessage(IMessageBank.Responses.verifySuccess).ConfigureAwait(false);
                    case 2:
                        return await _messageBank.GetMessage(IMessageBank.Responses.notConfirmed).ConfigureAwait(false);
                    case 3:
                        return await _messageBank.GetMessage(IMessageBank.Responses.notEnabled).ConfigureAwait(false);
                    default:
                        throw new NotImplementedException();
                };
            }
            catch (InvalidOperationException ioe)
            {
                return await _messageBank.GetMessage(IMessageBank.Responses.notFoundOrEnabled).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message;
            }  
        }

        /// <summary>
        ///     Verifies if a user is authorized to make node changes.
        /// </summary>
        /// <param name="nodeIDs">List of node IDs to check</param>
        /// <param name="userHash">User's Hash</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>String verification status</returns>
        public async Task<string> VerifyAccountAuthorizedNodeChangesAsync(List<long> nodeIDs, string userHash, CancellationToken cancellationToken = default)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                string result = await _sqlDAO.IsAuthorizedToMakeNodeChangesAsync(nodeIDs, userHash, cancellationToken).ConfigureAwait(false);

                return result;
            }
            catch(InvalidOperationException)
            {
                return await _messageBank.GetMessage(IMessageBank.Responses.notFoundOrAuthorized).ConfigureAwait(false);
            }
            catch(Exception ex)
            {
                return await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message;
            }
        }
    }
}
