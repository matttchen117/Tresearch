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

        /// <summary>
        ///     public RecoveryClass () :
        ///         Constructor useed for Recovery Service class
        /// </summary>
        /// <param name="sqlDAO"> Data access layer used to interact with database</param>
        /// <param name="logService">Logger</param>
        public RecoveryService(ISqlDAO sqlDAO, ILogService logService)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
        }
        /// <summary>
        ///     GetRecoveryLinkAsync():
        ///         Returns a tuple containing the corresponding Recoverylink to the guid and a string with statuscode
        /// </summary>
        /// <param name="guid">Unique identifier to the return link stored in database</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Tuple of recoverylink and statuscode string</returns>
        public async Task<Tuple<IRecoveryLink, string>> GetRecoveryLinkAsync(Guid guid, CancellationToken cancellationToken = default)
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
                return Tuple.Create(nullLink, "499");
            }
            catch (Exception ex)
            {
                return Tuple.Create(nullLink, "500");
            }
        }

        /// <summary>
        ///     RemoveRecoveryLinkAsync()
        ///         Returns a string result of deleting a recovery link from the database
        /// </summary>
        /// <param name="recoveryLink"> Link to remove from database</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>status code upon completion</returns>
        public async Task<string> RemoveRecoveryLinkAsync(IRecoveryLink recoveryLink, CancellationToken cancellationToken=default)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (recoveryLink == null)
                    return "404";
                string result = await _sqlDAO.RemoveRecoveryLinkAsync(recoveryLink, cancellationToken);
                return result;
            }
            catch (OperationCanceledException ex)
            {
                //Service has been cancelled
                return "499";
            }
            catch (Exception ex)
            {
                return "500";
            }
        }

        public async Task<string> RemoveAllRecoveryLinksAsync(string email, CancellationToken cancellationToken=default)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                Tuple<int, string> totalLinks = _sqlDAO.GetTotalRecoveryLinksAsync(email, cancellationToken).Result;
                if (totalLinks.Item1 > 0)
                {
                    Tuple<int, string> linksDeleted = await _sqlDAO.RemoveAllRecoveryLinksAsync(email, cancellationToken);
                    //Check if total links removed is equal to the total links with email in database
                    if (linksDeleted.Item1 == totalLinks.Item1)
                    {
                        return "200";
                    }
                    else
                    {
                        return "500";
                    }


                }
                else if (totalLinks.Item1 == 0)
                {
                    // There is no links to remove. 
                    return "200";
                }
                else
                {
                    return totalLinks.Item2;
                }
            }
            catch (OperationCanceledException ex)
            {
                //Service has been cancelled
                return "499";
            }
            catch (Exception ex)
            {
                return "500";
            }

        }

        public async Task<Tuple<IAccount, string>> GetAccountAsync(string email, string authorizationLevel, CancellationToken cancellationToken=default)
        {
            IAccount nullAccount = null;
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                Tuple<IAccount, string> accountTuple = await _sqlDAO.GetAccountAsync(email, authorizationLevel, cancellationToken);
                return accountTuple;
            }
            catch (OperationCanceledException ex)
            {
                //Service has been cancelled
                return Tuple.Create(nullAccount, "499");
            }
            catch (Exception ex)
            {
                return Tuple.Create(nullAccount, "500");
            }
        }

        public async Task<Tuple<bool, string>> IsAccountDisabledAsync(IAccount account, CancellationToken cancellationToken = default)
        {
            try
            {
                if (account.AccountStatus == false)
                    return Tuple.Create(true, "200");
                else
                    return Tuple.Create(false, "200");
            }
            catch (ArgumentNullException ex)
            {
                //Account passed in is null
                return Tuple.Create(false, "404");
            }
            catch (OperationCanceledException ex)
            {
                //Service has been cancelled
                //No need to make changes (no changes made to db)
                return Tuple.Create(false, "500");
            }
            catch (Exception ex)
            {
                return Tuple.Create(false, "500");
            }
        }

        public async Task<Tuple<IRecoveryLink, string>> CreateRecoveryLinkAsync(IAccount account, CancellationToken cancellationToken=default)
        {
            IRecoveryLink linkCreated = null;
            string result = "";
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                Tuple<int, string> totalRecoveryLinks = await _sqlDAO.GetTotalRecoveryLinksAsync(account.Email, cancellationToken);
                if(totalRecoveryLinks.Item1 == -1)
                {
                    return Tuple.Create(linkCreated, "500");
                    // Not sure of how many recovery links exist in database
                } else if(totalRecoveryLinks.Item1 > 5)
                {
                    // Only a max of 5 total RecoveryLinks per calendar month
                    // Return
                    return Tuple.Create(linkCreated, "403"); // 403 Forbidden - Account has more than 5 links and cannot create more this calendar month
                } else
                {
                    linkCreated  = new RecoveryLink(account.Email, Guid.NewGuid(), DateTime.Now, account.AuthorizationLevel);
                    result = await _sqlDAO.CreateRecoveryLinkAsync( linkCreated, cancellationToken);
                    if (cancellationToken.IsCancellationRequested && result == "200")
                    {
                        //Cancellation has been requested and changes have been made, need to roll back
                        string rollbackResult = await _sqlDAO.RemoveRecoveryLinkAsync(linkCreated);
                        linkCreated = null;
                        if (rollbackResult != "200")
                            return Tuple.Create(linkCreated, "503");    // 503 Service Unavailable - Roll back failed
                        else
                            return Tuple.Create(linkCreated, "500");    // 500 Generic Failed - Roll back su
                    }
                    return Tuple.Create(linkCreated, "200");
                }
                
            } catch (OperationCanceledException ex)
            {
                // Cancelled before execution
                return Tuple.Create(linkCreated, "500"); // 500 Generic Failed - cancel request
            }
            catch (Exception ex)
            {
                return Tuple.Create(linkCreated, "500"); // 500 Generic Failed - cancel request
            }
        }

        public async Task<string> EnableAccountAsync(IAccount account, CancellationToken cancellationToken=default)
        {
            string result = "";
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                result = await _sqlDAO.EnableAccountAsync(account, cancellationToken);
                if(cancellationToken.IsCancellationRequested && result == "200")
                {
                    string rollbackResult = await _sqlDAO.DisableAccountAsync(account, cancellationToken);
                    if (rollbackResult != "200")
                        return rollbackResult;
                }
                return result;
            }
            catch (OperationCanceledException ex)
            {
                return "499";
            }
            catch (Exception ex)
            {
                return "500";
            }

        }
    }
}
