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
                return _messageBank.ErrorMessages["notFoundOrEnabled"];
            }
            catch (Exception ex)
            {
                return "500: Database: " + ex.Message;
            }  
        }

        public async Task<string> VerifyAccountAuthorizedNodeChangesAsync(List<long> nodeIDs, IAccount account, CancellationToken cancellationToken = default)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                cancellationToken.ThrowIfCancellationRequested();

                string result = await _sqlDAO.IsAuthorizedToMakeNodeChangesAsync(nodeIDs, account, cancellationToken);

                return result;
            }
            catch(InvalidOperationException)
            {
                return await _messageBank.GetMessage(IMessageBank.Responses.notFoundOrAuthorized);
            }
            catch(Exception ex)
            {
                return "500: Database: " + ex.Message;
            }
        }
    }
}
