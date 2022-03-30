using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Services.Contracts;

namespace TrialByFire.Tresearch.Services.Implementations
{
    public class AccountDeletionService : IAccountDeletionService
    {

        private ISqlDAO SqlDAO { get; }

        private ILogService LogService { get; }



        private IMessageBank _messageBank;
        
        //cancellation token doesnt go into constructor, only in passed methods
        //DI MESSAGEBANKN IN CONSTRUCTOR
        public AccountDeletionService(ISqlDAO sqlDAO, ILogService logService)
        {
            this.SqlDAO = sqlDAO;
            this.LogService = logService;

        }

        //ANOTHER METHOD IN SERVICE, AND MANAGER SHOULD CALL GETADMINS METHOD, MANAGER CALLS SERVICE CALLS DAO, RETURN BACK OPPOSITE WAY
        //1) CONFIGURE AWAIT(FALSE) FOR ALLASYHC METHOD CALLS, IF NOT THERE THE REQUEST MIGHT NOT START OFF IMMEDIATELY, 
        //2) 
        public async Task<string> DeleteAccountAsync(CancellationToken cancellationToken = default)
        {
            string admins;
            string result;
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                admins = await SqlDAO.GetAmountOfAdminsAsync(cancellationToken);

                if (admins.Equals(_messageBank.SuccessMessages["generic"]))
                {
                    result = await SqlDAO.DeleteAccountAsync(cancellationToken).ConfigureAwait(false);

                    if (result.Equals(_messageBank.SuccessMessages["generic"]))
                    {
                        return await _messageBank.GetMessage(IMessageBank.Responses.generic).ConfigureAwait(false);
                    }

                }
                else
                {
                    return await _messageBank.GetMessage(IMessageBank.Responses.lastAdminFail).ConfigureAwait(false);
                }



                return await _messageBank.GetMessage(IMessageBank.Responses.deleteAccountFail).ConfigureAwait(false);




            }

            catch (OperationCanceledException)
            {
                return await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested).ConfigureAwait(false);
            }

            catch (Exception ex)
            {
                return await _messageBank.GetMessage(IMessageBank.Responses.deleteAccountFail).ConfigureAwait(false); ;
            }

        }

        public async Task<string> GetAmountOfAdmins(CancellationToken cancellationToken = default)
        {
            return await SqlDAO.GetAmountOfAdminsAsync(cancellationToken).ConfigureAwait(false);
        }


    }
}
