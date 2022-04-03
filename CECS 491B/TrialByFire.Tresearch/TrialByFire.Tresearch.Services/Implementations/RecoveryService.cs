using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Services.Contracts;


namespace TrialByFire.Tresearch.Services.Implementations
{
    /// <summary>
    ///     RecoveryService: Class used to perform complex logic regarding recovering an account
    /// </summary>
    public class RecoveryService : IRecoveryService
    {
        private ISqlDAO _sqlDAO { get; set; }
        private ILogService _logService { get; set; }
        private IMessageBank _messageBank { get; set; }

        /// <summary>
        ///     public RecoveryClass () :
        ///         Constructor useed for Recovery Service class
        /// </summary>
        /// <param name="sqlDAO"> Data access layer used to interact with database</param>
        /// <param name="logService">Logger</param>
        public RecoveryService(ISqlDAO sqlDAO, ILogService logService, IMessageBank messageBank)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
            _messageBank = messageBank;
        }
        /// <summary>
        ///     GetRecoveryLinkAsync():
        ///         Returns a tuple containing the corresponding Recoverylink to the guid and a string with statuscode
        /// </summary>
        /// <param name="guid">Unique identifier to the return link stored in database</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Tuple of recoverylink and statuscode string</returns>
        public async Task<Tuple<IRecoveryLink, string>> GetRecoveryLinkAsync(string guid, CancellationToken cancellationToken = default(CancellationToken))
        {
            IRecoveryLink nullLink = null;  // To return if there is an exception
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                Tuple<IRecoveryLink, string> linkResult = await _sqlDAO.GetRecoveryLinkAsync(guid, cancellationToken);
                if (cancellationToken.IsCancellationRequested)
                    throw new OperationCanceledException();
                return linkResult;
            }
            catch (OperationCanceledException ex)
            {
                //Service has been cancelled
                throw;
            }
            catch (Exception ex)
            {
                return Tuple.Create(nullLink, "500: Server: " + ex);
            }
        }

        /// <summary>
        ///     RemoveRecoveryLinkAsync()
        ///         Returns a string result of deleting a recovery link from the database
        /// </summary>
        /// <param name="recoveryLink"> Link to remove from database</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>status code upon completion</returns>
        public async Task<string> RemoveRecoveryLinkAsync(IRecoveryLink recoveryLink, CancellationToken cancellationToken= default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (recoveryLink == null)
                    return _messageBank.GetMessage(IMessageBank.Responses.recoveryLinkNotFound).Result;
                string result = await _sqlDAO.RemoveRecoveryLinkAsync(recoveryLink, cancellationToken);
                if(cancellationToken.IsCancellationRequested && result.Equals(await _messageBank.GetMessage(IMessageBank.Responses.generic)))
                {
                    string rollbackResult = await _sqlDAO.CreateRecoveryLinkAsync(recoveryLink);
                    if (rollbackResult.Equals(await _messageBank.GetMessage(IMessageBank.Responses.generic)))
                        throw new OperationCanceledException();
                    else
                        return await _messageBank.GetMessage(IMessageBank.Responses.rollbackFailed);
                }
                else if (cancellationToken.IsCancellationRequested)
                    throw new OperationCanceledException();
                return result;
            }
            catch (OperationCanceledException)
            {
                //Service has been cancelled
                throw;
            }
            catch (Exception ex)
            {
                return "500: Server: " + ex;
            }
        }

        /// <summary>
        ///     GetAccountAsync(email, authorizationLevel)
        ///         Returns Account from database matching credentials
        /// </summary>
        /// <param name="email">Email of account to get</param>
        /// <param name="authorizationLevel">Authorization Level of account</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>String with status code</returns>
        public async Task<Tuple<IAccount, string>> GetAccountAsync(string email, string authorizationLevel, CancellationToken cancellationToken= default(CancellationToken))
        {
            IAccount nullAccount = null;
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                Tuple<IAccount, string> accountTuple = await _sqlDAO.GetAccountAsync(email, authorizationLevel, cancellationToken);
                if (cancellationToken.IsCancellationRequested)                                                  //No rollback necessary if canceled
                    throw new OperationCanceledException();
                else
                    return accountTuple;
            }
            catch (OperationCanceledException)
            {
                //Service has been cancelled...no rollback necessary
                throw;
            }
            catch (Exception ex)
            {
                return Tuple.Create(nullAccount, "500: Server: " + ex);
            }
        }

        /// <summary>
        ///     CreateRecoveryLinkAsync(account)
        ///        Creates and updates database with recovery link
        /// </summary>
        /// <param name="account">Account to create recovery link for</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>String with status code</returns>
        public async Task<Tuple<IRecoveryLink, string>> CreateRecoveryLinkAsync(IAccount account, CancellationToken cancellationToken=default(CancellationToken))
        {
            IRecoveryLink linkCreated;
            IRecoveryLink nullLink = null;
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                linkCreated  = new RecoveryLink(account.Email, account.AuthorizationLevel, DateTime.Now, Guid.NewGuid());
                string result = await _sqlDAO.CreateRecoveryLinkAsync(linkCreated, cancellationToken);

                if (cancellationToken.IsCancellationRequested && result.Equals(_messageBank.GetMessage(IMessageBank.Responses.generic).Result))
                {
                    //Cancellation has been requested and changes have been made, need to roll back
                    string rollbackResult = await _sqlDAO.RemoveRecoveryLinkAsync(linkCreated);
                    linkCreated = null;
                    if (rollbackResult != _messageBank.GetMessage(IMessageBank.Responses.generic).Result)
                        return Tuple.Create(nullLink, _messageBank.GetMessage(IMessageBank.Responses.rollbackFailed).Result);    // 503 Service Unavailable - Roll back failed
                    else
                        throw new OperationCanceledException();
                }
                else if (cancellationToken.IsCancellationRequested)
                    throw new OperationCanceledException();
                return Tuple.Create(linkCreated, result);

            } catch (OperationCanceledException ex)
            {
                // No rollback necessary
                throw;
            }
            catch (Exception ex)
            {
                return Tuple.Create(nullLink, "500: Server: " + ex);
            }
        }

        /// <summary>
        ///     EnableAccountAync(email, authorizationLevel)
        ///         Updates account status to enabled
        /// </summary>
        /// <param name="email">Email of account</param>
        /// <param name="authorizationLevel">Authorization Level of account</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>String status code</returns>
        public async Task<string> EnableAccountAsync(string email, string authorizationLevel, CancellationToken cancellationToken= default(CancellationToken))
        {
            string result = "";
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                result = await _sqlDAO.EnableAccountAsync(email, authorizationLevel, cancellationToken);

                if(cancellationToken.IsCancellationRequested && result == _messageBank.GetMessage(IMessageBank.Responses.generic).Result)
                {
                    string rollbackResult = await _sqlDAO.DisableAccountAsync(email, authorizationLevel, cancellationToken);
                    if (rollbackResult != await _messageBank.GetMessage(IMessageBank.Responses.generic))
                        return rollbackResult;
                } 
                else if(cancellationToken.IsCancellationRequested)
                    throw new OperationCanceledException();
                return result;
            }
            catch (OperationCanceledException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                return "500: Server: " + ex;
            }

        }

        public async Task<string> DisableAccountAsync(string email, string authorizationLevel, CancellationToken cancellationToken = default(CancellationToken))
        {

            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                string result = await _sqlDAO.DisableAccountAsync(email, authorizationLevel, cancellationToken);
                if (cancellationToken.IsCancellationRequested && result == await _messageBank.GetMessage(IMessageBank.Responses.generic))
                {
                    string rollbackResult = await _sqlDAO.DisableAccountAsync(email, authorizationLevel, cancellationToken);
                    if (rollbackResult != await _messageBank.GetMessage(IMessageBank.Responses.generic))
                        return rollbackResult;
                }
                else if (cancellationToken.IsCancellationRequested)
                    throw new OperationCanceledException();
                return result;
            }
            catch (OperationCanceledException)
            {
                // No rollback necessary
                throw;
            }
            catch (Exception ex)
            {
                return "500: Server: " + ex;
            }

        }

        /// <summary>
        ///     DecrementRecoveryLinkCountAsync(email, authorizationLevel)
        ///         Decrements count of recovery links user has created this month
        /// </summary>
        /// <param name="email">Email of account</param>
        /// <param name="authorizationLevel">AuthorizationLevel of account</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>string status code</returns>
        public async Task<string> DecrementRecoveryLinkCountAsync(string email, string authorizationLevel, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                string result = await _sqlDAO.DecrementRecoveryLinkCountAsync(email, authorizationLevel, cancellationToken).ConfigureAwait(false);
                if (cancellationToken.IsCancellationRequested && result == _messageBank.GetMessage(IMessageBank.Responses.generic).Result)
                {
                    string rollbackResult = await _sqlDAO.IncrementRecoveryLinkCountAsync(email, authorizationLevel);
                    if (rollbackResult != await _messageBank.GetMessage(IMessageBank.Responses.generic))
                        return _messageBank.GetMessage(IMessageBank.Responses.rollbackFailed).Result;
                    else
                        throw new OperationCanceledException();
                }
                else if (cancellationToken.IsCancellationRequested)
                    throw new OperationCanceledException();
                return result;
            }
            catch (OperationCanceledException)
            {
                // No rollback necessary
                throw;
            }
            catch (Exception ex)
            {
                return "500: Server: " + ex;
            }
        }

        /// <summary>
        ///     IncrementRecoveryLinkCountAsync(email, authorizationLevel)
        ///         Increments count of recovery links user has created this month
        /// </summary>
        /// <param name="email">Email of account</param>
        /// <param name="authorizationLevel">Authorization level of account</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>String status code</returns>
        public async Task<string> IncrementRecoveryLinkCountAsync(string email, string authorizationLevel, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                string result = await _sqlDAO.IncrementRecoveryLinkCountAsync(email, authorizationLevel, cancellationToken).ConfigureAwait(false);
                if (cancellationToken.IsCancellationRequested && result == await _messageBank.GetMessage(IMessageBank.Responses.generic))
                {
                    string rollbackResult = await _sqlDAO.DecrementRecoveryLinkCountAsync(email, authorizationLevel);
                    if (rollbackResult != _messageBank.GetMessage(IMessageBank.Responses.generic).Result)
                        return _messageBank.GetMessage(IMessageBank.Responses.rollbackFailed).Result;
                    else
                        throw new OperationCanceledException();
                }
                else if (cancellationToken.IsCancellationRequested)
                    throw new OperationCanceledException();
                return result;
            }
            catch (OperationCanceledException)
            {
                // No rollback necessary
                throw;
            }
            catch (Exception ex)
            {
                return "500: Server: " + ex;
            }
        }

        /// <summary>
        ///     GetRecoveryLinkCountAsync(string email, string authorizationLevel)
        ///         Returns Count of recovery links user has created this month
        /// </summary>
        /// <param name="email">Email of user</param>
        /// <param name="authorizationLevel">Authorization Level of user</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Integer amount of recovery links created</returns>
        public async Task<int> GetRecoveryLinkCountAsync(string email, string authorizationLevel, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                int result = await _sqlDAO.GetRecoveryLinkCountAsync(email, authorizationLevel, cancellationToken).ConfigureAwait(false);
                if (cancellationToken.IsCancellationRequested)
                    throw new OperationCanceledException();
                return result;
            }
            catch (OperationCanceledException)
            {
                // No rollback necessary
                throw;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
    }
}
