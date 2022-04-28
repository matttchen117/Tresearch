using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Exceptions;
using TrialByFire.Tresearch.Models;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Services.Contracts;

namespace TrialByFire.Tresearch.Services.Implementations
{
    public class AccountDeletionService : IAccountDeletionService
    {

        private ISqlDAO _sqlDAO { get; }
        private ILogService _logService { get; }
        private IMessageBank _messageBank { get; }
        private BuildSettingsOptions _options { get; }



        public AccountDeletionService(ISqlDAO sqlDAO, ILogService logService, IMessageBank messageBank, IOptionsSnapshot<BuildSettingsOptions> options)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
            _messageBank = messageBank;
            _options = options.Value;

        }


        /// <summary>
        /// Method to delete account by calling DAO to delete object
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns> Returns a message indicating success or failure </returns>
        public async Task<string> DeleteAccountAsync(CancellationToken cancellationToken = default(CancellationToken))
        {


            string result = "";
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                result = await _sqlDAO.DeleteAccountAsync(cancellationToken).ConfigureAwait(false);

                //redundant code here
                if (result.Equals(await _messageBank.GetMessage(IMessageBank.Responses.accountDeletionSuccess).ConfigureAwait(false)))
                {
                    return await _messageBank.GetMessage(IMessageBank.Responses.accountDeletionSuccess).ConfigureAwait(false);
                }
                else if (result.Equals(await _messageBank.GetMessage(IMessageBank.Responses.accountNotFound).ConfigureAwait(false)))
                {
                    return await _messageBank.GetMessage(IMessageBank.Responses.accountNotFound).ConfigureAwait(false);
                } 
                else
                {
                    return await _messageBank.GetMessage(IMessageBank.Responses.accountDeleteFail).ConfigureAwait(false);
                }
            }
            catch (OperationCanceledException)
            {
                return await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested).ConfigureAwait(false);
            }
            catch (AccountDeletionFailedException)
            {
                return await _messageBank.GetMessage(IMessageBank.Responses.accountDeleteFail).ConfigureAwait(false); ;
            }
            catch (Exception ex)
            {
                return await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message;
            }
        }

        /// <summary>
        /// method to get if there is more than 1 admin in database as business rule
        /// so that at least 1 admin account is always attached to application at all times
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>returns message indicating failure or success based on whether more than 1 admin exists or not.</returns>
        public async Task<string> GetAmountOfAdminsAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _sqlDAO.GetAmountOfAdminsAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
