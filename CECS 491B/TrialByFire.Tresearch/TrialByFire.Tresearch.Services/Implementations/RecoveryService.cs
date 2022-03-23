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
                return linkResult;
            }
            catch (OperationCanceledException ex)
            {
                //Service has been cancelled
                return Tuple.Create(nullLink, _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested).Result);
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
                    return "404";
                string result = await _sqlDAO.RemoveRecoveryLinkAsync(recoveryLink, cancellationToken);
                return result;
            }
            catch (OperationCanceledException)
            {
                //Service has been cancelled
                return _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested).Result;
            }
            catch (Exception ex)
            {
                return "500: Server: " + ex;
            }
        }

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
                //Service has been cancelled
                return Tuple.Create(nullAccount, _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested).Result);
            }
            catch (Exception ex)
            {
                return Tuple.Create(nullAccount, "500: Server: " + ex);
            }
        }


        public async Task<Tuple<IRecoveryLink, string>> CreateRecoveryLinkAsync(IAccount account, CancellationToken cancellationToken=default(CancellationToken))
        {
            IRecoveryLink linkCreated;
            IRecoveryLink nullLink = null;
            string result = "";
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                linkCreated  = new RecoveryLink(account.Email, account.AuthorizationLevel, DateTime.Now, Guid.NewGuid());
                result = await _sqlDAO.CreateRecoveryLinkAsync(linkCreated, cancellationToken);
                
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

        public async Task<string> EnableAccountAsync(string email, string authorizationLevel, CancellationToken cancellationToken= default(CancellationToken))
        {
            string result = "";
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                
                result = await _sqlDAO.EnableAccountAsync(email, authorizationLevel, cancellationToken);
                
                if(cancellationToken.IsCancellationRequested && result == _messageBank.SuccessMessages["generic"])
                {
                    string rollbackResult = await _sqlDAO.DisableAccountAsync(email, authorizationLevel, cancellationToken);
                    if (rollbackResult != _messageBank.SuccessMessages["generic"])
                        return rollbackResult;
                } else if(cancellationToken.IsCancellationRequested)
                    return _messageBank.ErrorMessages["cancellationRequested"];
                return result;
            }
            catch (OperationCanceledException ex)
            {
                return _messageBank.ErrorMessages["cancellationRequested"];
            }
            catch (Exception ex)
            {
                return "500: Server: " + ex;
            }

        }

        public async Task<string> DisableAccountAsync(string email, string authorizationLevel, CancellationToken cancellationToken = default(CancellationToken))
        {
            string result = "";
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                result = await _sqlDAO.DisableAccountAsync(email, authorizationLevel, cancellationToken);
                if (cancellationToken.IsCancellationRequested && result == _messageBank.SuccessMessages["generic"])
                {
                    string rollbackResult = await _sqlDAO.DisableAccountAsync(email, authorizationLevel, cancellationToken);
                    if (rollbackResult != _messageBank.SuccessMessages["generic"])
                        return rollbackResult;
                }
                else if (cancellationToken.IsCancellationRequested)
                    return _messageBank.ErrorMessages["cancellationRequested"];
                return result;
            }
            catch (OperationCanceledException)
            {
                return _messageBank.ErrorMessages["cancellationRequested"];
            }
            catch (Exception ex)
            {
                return "500: Server: " + ex;
            }

        }
    }
}
