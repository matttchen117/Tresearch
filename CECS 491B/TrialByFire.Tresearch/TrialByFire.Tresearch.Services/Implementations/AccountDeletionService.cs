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
        

        public AccountDeletionService(ISqlDAO sqlDAO, ILogService logService, CancellationToken cancellationToken = default)
        {
            this.SqlDAO = sqlDAO;
            this.LogService = logService;
        }



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
                    result = await SqlDAO.DeleteAccountAsync(cancellationToken);
                                        
                }
                else
                {
                    return _messageBank.ErrorMessages["lastAdminFail"];
                }

                if (result.Equals(_messageBank.SuccessMessages["generic"]))
                {
                    return _messageBank.SuccessMessages["generic"];
                }


            }
            catch (OperationCanceledException)
            {
                return _messageBank.ErrorMessages["cancellationRequested"];
            }
            catch (Exception ex)
            {
                return ("500: Database " + ex.Message);

            }
        }


    }
}
