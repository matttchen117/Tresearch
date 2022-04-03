using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;

namespace TrialByFire.Tresearch.Services.Implementations
{
    public class RegistrationService : IRegistrationService
    {
        private ISqlDAO _sqlDAO { get; set; }
        private ILogService _logService { get; set; }
        private IMessageBank _messageBank { get; set; }

        private int linkActivationLimit = 24;

        public RegistrationService(ISqlDAO sqlDAO, ILogService logService, IMessageBank messageBank)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
            _messageBank = messageBank;
        }

        public async Task<string> CreateAccountAsync(string email, string passphrase, string authorizationLevel, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                IAccount account = new Account(email, email, passphrase, authorizationLevel, true, false);

                string createResult = await _sqlDAO.CreateAccountAsync(account, cancellationToken).ConfigureAwait(false);

                if(cancellationToken.IsCancellationRequested && createResult == _messageBank.GetMessage(IMessageBank.Responses.generic).Result)
                {
                    //Perform Rollback
                }

                return createResult;
            }
            catch (OperationCanceledException)
            {
                //Nothing to rollback
                throw;
            }
            catch (Exception ex)
            {
                return "500: Server: " + ex.Message;
            }
        }

        public async Task<Tuple<IConfirmationLink, string>> CreateConfirmationAsync(string email, string authorizationLevel, CancellationToken cancellationToken = default(CancellationToken))
        {
            IConfirmationLink nullLink = null;
            try
            {
                Guid guid = Guid.NewGuid();
                IConfirmationLink confirmationLink = new ConfirmationLink(email, authorizationLevel, guid, DateTime.Now);

                string result = await _sqlDAO.CreateConfirmationLinkAsync(confirmationLink, cancellationToken).ConfigureAwait(false);

                if (result != _messageBank.GetMessage(IMessageBank.Responses.generic).Result)
                    return Tuple.Create(nullLink, result);

                if(cancellationToken.IsCancellationRequested && result == _messageBank.GetMessage(IMessageBank.Responses.generic).Result)
                {
                    string rollbackResult = await _sqlDAO.RemoveConfirmationLinkAsync(confirmationLink);
                    if (rollbackResult != _messageBank.GetMessage(IMessageBank.Responses.generic).Result)
                        return Tuple.Create(nullLink, _messageBank.GetMessage(IMessageBank.Responses.rollbackFailed).Result);
                    else
                        throw new OperationCanceledException();
                }
                return Tuple.Create(confirmationLink, result);
            }
            catch (OperationCanceledException)
            {
                //Rollback taken care of
                throw;
            }
            catch (Exception ex)
            {
                return Tuple.Create(nullLink, "500: Server: " + ex.Message);

            }
        }

        public async Task<string> CreateConfirmationAsync(IConfirmationLink link, CancellationToken cancellationToken = default(CancellationToken))
        {
            IConfirmationLink nullLink = null;
            try
            {

                string result = await _sqlDAO.CreateConfirmationLinkAsync(link, cancellationToken).ConfigureAwait(false);

                if (result != _messageBank.GetMessage(IMessageBank.Responses.generic).Result)
                    return result;

                if (cancellationToken.IsCancellationRequested && result == _messageBank.GetMessage(IMessageBank.Responses.generic).Result)
                {
                    string rollbackResult = await _sqlDAO.RemoveConfirmationLinkAsync(link);
                    if (rollbackResult != await _messageBank.GetMessage(IMessageBank.Responses.generic))
                        return await _messageBank.GetMessage(IMessageBank.Responses.rollbackFailed);
                    else
                        throw new OperationCanceledException();
                }
                return result;
            }
            catch (OperationCanceledException)
            {
                //Rollback taken care of
                throw;
            }
            catch (Exception ex)
            {
                return "500: Server: " + ex.Message;

            }
        }

        public async Task<string> ConfirmAccountAsync(string username, string authorizationLevel, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                string results = await _sqlDAO.UpdateAccountToConfirmedAsync(username, authorizationLevel, cancellationToken);
                if(cancellationToken.IsCancellationRequested && results == _messageBank.GetMessage(IMessageBank.Responses.generic).Result)
                {
                    //Implement rollback
                    string rollbackResults = await _sqlDAO.UpdateAccountToUnconfirmedAsync(username, authorizationLevel, cancellationToken).ConfigureAwait(false);
                    if (rollbackResults != _messageBank.GetMessage(IMessageBank.Responses.generic).Result)
                        return _messageBank.GetMessage(IMessageBank.Responses.rollbackFailed).Result;
                    else
                        throw new OperationCanceledException();
                }
                return results;
            }
            catch (OperationCanceledException)
            {
                //No rollback necessary (or taken care of)
                throw;
            }
            catch(Exception ex)
            {
                return "500: Server: " + ex.Message;
            }
        }

        public async Task<Tuple<IConfirmationLink, string>> GetConfirmationLinkAsync(string guid, CancellationToken cancellationToken = default(CancellationToken))
        {
            IConfirmationLink nullLink = null;
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                Tuple<IConfirmationLink, string> confirmationLink = await _sqlDAO.GetConfirmationLinkAsync(guid).ConfigureAwait(false);
                if (cancellationToken.IsCancellationRequested)
                    throw new OperationCanceledException();
                return confirmationLink;
            }
            catch (OperationCanceledException)
            {
                //No rollback necessary
                throw;
            }
            catch(Exception ex)
            {
                return Tuple.Create(nullLink, "500: Server: " + ex.Message);
            }
        }

        public async Task<string> RemoveConfirmationLinkAsync(IConfirmationLink confirmationLink, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                string result = await _sqlDAO.RemoveConfirmationLinkAsync(confirmationLink, cancellationToken).ConfigureAwait(false);
                if(cancellationToken.IsCancellationRequested && result != _messageBank.GetMessage(IMessageBank.Responses.generic).Result)
                {
                    string rollbackResult = await _sqlDAO.CreateConfirmationLinkAsync(confirmationLink).ConfigureAwait(false);
                    if (rollbackResult != _messageBank.GetMessage(IMessageBank.Responses.generic).Result)
                        return _messageBank.GetMessage(IMessageBank.Responses.rollbackFailed).Result;
                    else
                        throw new OperationCanceledException();
                }
                return result;
            }
            catch (OperationCanceledException)
            {
                //No rollback necessary
                throw;
            }
            catch (Exception ex)
            {
                return "500: Server: " + ex.Message;
            }
        }
    }
}
