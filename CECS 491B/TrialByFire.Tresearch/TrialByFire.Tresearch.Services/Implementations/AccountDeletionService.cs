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

        //ANOTHER METHOD IN SERVICE, AND MANAGER SHOULD CALL GETADMINS METHOD, MANAGER CALLS SERVICE CALLS DAO, RETURN BACK OPPOSITE WAY

        public async Task<string> DeleteAccountAsync(CancellationToken cancellationToken = default(CancellationToken))
        {

            Console.WriteLine(Thread.GetCurrentProcessorId);




            string result = "";
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                result = await _sqlDAO.DeleteAccountAsync(cancellationToken).ConfigureAwait(false);

                //added await to if statement
                //instead of an else if here, i can just return result instead, make the manager figure out status codes for me
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
                    //doing account was not found here
                    return await _messageBank.GetMessage(IMessageBank.Responses.accountDeleteFail).ConfigureAwait(false);

                    //return await _messageBank.GetMessage(IMessageBank.Responses.accountNotFound).ConfigureAwait(false);
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
                return _options.UncaughtExceptionMessage + ex.Message;
            }
            

            

        }

        public async Task<string> GetAmountOfAdminsAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _sqlDAO.GetAmountOfAdminsAsync(cancellationToken).ConfigureAwait(false);
        }


    }
}
