using Dapper;
using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlClient;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Exceptions;
using TrialByFire.Tresearch.Models;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;

namespace TrialByFire.Tresearch.DAL.Implementations
{
    public class SqlDAO : ISqlDAO
    {
        private BuildSettingsOptions _options { get; }
        private IMessageBank _messageBank;

        public SqlDAO(IMessageBank messageBank, IOptionsSnapshot<BuildSettingsOptions> options)
        {
            _messageBank = messageBank;
            _options = options.Value;
        }
        public async Task<int> LogoutAsync(IAccount account, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var connection = new SqlConnection(_options.SqlConnectionString))
            {
                //Perform sql statement
                var procedure = "[Logout]";
                var parameters = new DynamicParameters();
                parameters.Add("Username", account.Username);
                parameters.Add("AuthorizationLevel", account.AuthorizationLevel);
                parameters.Add("Result", dbType: DbType.Int32, direction: ParameterDirection.Output);
                var result = await connection.ExecuteAsync(new CommandDefinition(procedure, parameters,
                    commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken))
                    .ConfigureAwait(false);
                return parameters.Get<int>("Result");
            }
        }
        public async Task<string> StoreLogAsync(ILog log, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using(var connection = new SqlConnection(_options.SqlConnectionString))
            {
                var procedure = "[StoreLog]";                                                               
                var value = new { Timestamp = log.Timestamp, Level = log.Level, Username = log.Username, 
                Category = log.Category, Description = log.Description };   
                int affectedRows = await connection.ExecuteAsync(new CommandDefinition(procedure, value, cancellationToken: cancellationToken)).ConfigureAwait(false);
                if (affectedRows != 1)
                {
                    affectedRows = await connection.ExecuteAsync(new CommandDefinition(procedure, value, cancellationToken: cancellationToken)).ConfigureAwait(false);
                    if (affectedRows != 1)
                    {
                        return await _messageBank.GetMessage(IMessageBank.Responses.storeLogFail).ConfigureAwait(false);
                    }
                }
            }
            return await _messageBank.GetMessage(IMessageBank.Responses.generic).ConfigureAwait(false);
        }


        /// <summary>
        ///     DisableAccountAsync()
        ///         Disables accounts account passed in asynchrnously.
        /// </summary>
        /// <param name="account">Account to disable</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>String with statuscode</returns>
        public async Task<string> DisableAccountAsync(string email, string authorizationLevel, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();                                                      
                using (var connection = new SqlConnection(_options.SqlConnectionString)) 
                {
                    //Perform sql statement
                    var procedure = "[DisableAccount]";
                    var value = new { Username = email, AuthorizationLevel = authorizationLevel };
                    int affectedRows = await connection.ExecuteScalarAsync<int>(new CommandDefinition(procedure, value, cancellationToken: cancellationToken)).ConfigureAwait(false);

                    //Check if cancellation is requested
                    if (cancellationToken.IsCancellationRequested)
                    {
                        //Cancellation has been requested, undo everything
                        string rollbackResult = await EnableAccountAsync(email, authorizationLevel);                                      // Enables account.. result should be generic success
                        if (rollbackResult != _messageBank.SuccessMessages["generic"])
                            return _messageBank.ErrorMessages["rollbackFailed"];                                        // Rollback failed, account is still in database
                        else
                            return _messageBank.ErrorMessages["cancellationRequested"];                                 // Cancellation requested, successfully rolledback account disable
                    }

                    //Check rows affected... If account exists, should be 1 otherwise error
                    if (affectedRows == 0)
                        return _messageBank.ErrorMessages["accountNotFound"];                                           // Account doesn't exist
                    else if (affectedRows != 1)
                        return _messageBank.ErrorMessages["accountDisableFail"];                                        // Could not disable account
                    return _messageBank.SuccessMessages["generic"];                                                     // Account successfully disabled
                }
            }
            catch (OperationCanceledException)
            {
                // Cancellation requested, nothing to rollback
                return _messageBank.ErrorMessages["cancellationRequested"];
            }
            catch (Exception ex)
            {
                return "500: Database: " + ex.Message;
            }

        }

        /// <summary>
        ///     EnableAccountAsync()
        ///         Enables accounts account passed in asynchrnously.
        /// </summary>
        /// <param name="account">Account to enable</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>String with statuscode</returns>
        public async Task<string> EnableAccountAsync(string email, string authorizationLevel, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();                                                           // Check if cancellation token has requested cancellation
                using (var connection = new SqlConnection(_options.SqlConnectionString))                                            // Establish connection with database
                {
                    //Perform sql statement
                    var procedure = "[EnableAccount]";                                                                      // Store Procedure
                    var value = new { Username = email, AuthorizationLevel = authorizationLevel };
                    int affectedRows = await connection.ExecuteScalarAsync<int>(new CommandDefinition(procedure, value, cancellationToken: cancellationToken)).ConfigureAwait(false);

                    // Check if cancellation is requested
                    if (cancellationToken.IsCancellationRequested)
                    {
                        string rollbackResult = await DisableAccountAsync(email, authorizationLevel);
                        if (rollbackResult != "200")
                            return "503";    // 503 Service Unavailable - Roll back failed
                        else
                            return "500";    // 500 Generic Failed - Roll back su
                    }

                    //Check rows affected... If account exists, should be 1 otherwise error
                    if (affectedRows == 0)
                        return _messageBank.ErrorMessages["accountNotFound"];                                               // Account doesn't exist
                    else if (affectedRows != 1)
                        return _messageBank.ErrorMessages["accountEnableFail"];                                             // Could not enable account
                    return _messageBank.SuccessMessages["generic"];                                                         // Account successfully disabled
                }
            }
            catch (OperationCanceledException)
            {
                // Cancellation requested, nothing to rollback
                return _messageBank.ErrorMessages["cancellationRequested"];
            }
            catch (Exception ex)
            {
                return "500: Database: " + ex.Message;
            }
        }

        /// <summary>
        ///     GetAccountAsync()
        ///         Returns an account 
        /// </summary>
        /// <param name="email">Email of account to find</param>
        /// <param name="authorizationLevel">Authorization level of account to find</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Tuple containing account found (if not found, null) and string status code</returns>
        public async Task<Tuple<IAccount, string>> GetAccountAsync(string email, string authorizationLevel, CancellationToken cancellationToken = default(CancellationToken))
        {
            IAccount nullAccount = null;
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                using (var connection = new SqlConnection(_options.SqlConnectionString))
                {
                    //Perform sql statement

                    var procedure = "dbo.[GetAccount]";
                    var parameters = new { Username = email, AuthorizationLevel = authorizationLevel };
                    var Accounts = await connection.QueryAsync<Account>(new CommandDefinition(procedure, parameters, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken)).ConfigureAwait(false);

                    //Check if account was returned
                    if (Accounts.Count() == 0)
                        return Tuple.Create(nullAccount, _messageBank.ErrorMessages["accountNotFound"]);            //Account doesn't exist

                    IAccount account = Accounts.First();


                    // Check if cancellation is requested .. no rollback necessary
                    if (cancellationToken.IsCancellationRequested)
                        return Tuple.Create(nullAccount, _messageBank.ErrorMessages["cancellationRequested"]);
                    else
                        return Tuple.Create(account, _messageBank.SuccessMessages["generic"]);
                }
            }
            catch (OperationCanceledException)
            {
                return Tuple.Create(nullAccount, _messageBank.ErrorMessages["cancellationRequested"]);
            }
            catch (Exception ex)
            {
                return Tuple.Create(nullAccount, "500: Database: " + ex.Message);
            }
        }

        /// <summary>
        ///     RemoveRecoveryLinkAsync()
        ///         Removes recovery link from database.
        /// </summary>
        /// <param name="recoveryLink">Recovery link to remove</param>
        /// <param name="cancellationToken"></param>
        /// <returns>String status code</returns>
        public async Task<string> RemoveRecoveryLinkAsync(IRecoveryLink recoveryLink, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                using (var connection = new SqlConnection(_options.SqlConnectionString))
                {
                    //Perform sql statement
                    var procedure = "[RemoveRecoveryLink]";
                    var value = new { GUIDLink = recoveryLink.GUIDLink };
                    var affectedRows = await connection.ExecuteScalarAsync<int>(new CommandDefinition(procedure, value, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken)).ConfigureAwait(false);

                    //Check if cancellation requested
                    if (cancellationToken.IsCancellationRequested)
                    {
                        string rollbackResults = await CreateRecoveryLinkAsync(recoveryLink);
                        if (rollbackResults != _messageBank.SuccessMessages["generic"])
                            return _messageBank.ErrorMessages["rollbackFailed"];
                        else
                            return _messageBank.ErrorMessages["cancellationRequested"];
                    }

                    //Check if recovery link removed
                    if (affectedRows == 0)
                        return _messageBank.ErrorMessages["accountNotFound"];
                    else if (affectedRows == 1)
                        return _messageBank.SuccessMessages["generic"];
                    else
                        return _messageBank.ErrorMessages["recoveryLinkRemoveFail"];
                }
            }
            catch (OperationCanceledException)
            {
                return _messageBank.ErrorMessages["cancellationRequested"];
            }
            catch (Exception ex)
            {
                return "500: Database: " + ex.Message;
            }
        }

        /// <summary>
        ///     GetRecoveryLinkAsync()
        ///         Return recovery link from database
        /// </summary>
        /// <param name="guid">Uniqueidentifier of link in database</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Tuple containing recovery link and string status code</returns>
        public async Task<Tuple<IRecoveryLink, string>> GetRecoveryLinkAsync(Guid guid, CancellationToken cancellationToken = default(CancellationToken))
        {
            IRecoveryLink nullLink = null;
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                using (var connection = new SqlConnection(_options.SqlConnectionString))
                {
                    //Perform sql statement
                    var procedure = "[GetRecoveryLink]";                                    // Stored procedure
                    var value = new { GUIDLink = guid };                                     // Guid to search in table
                    var links = await connection.QueryAsync<RecoveryLink>(new CommandDefinition(procedure, value, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken)).ConfigureAwait(false);

                    //Check for cancellation...no rollback necessary
                    if (cancellationToken.IsCancellationRequested)
                        return Tuple.Create(nullLink, _messageBank.ErrorMessages["cancellationRequested"]);

                    //Return recoverylink if found
                    if (links.Count() == 0)
                        return Tuple.Create(nullLink, _messageBank.ErrorMessages["recoveryLinkNotFound"]);
                    else
                    {
                        IRecoveryLink link = links.First();
                        return Tuple.Create(link, _messageBank.SuccessMessages["generic"]);
                    }

                }
            }
            catch (OperationCanceledException)
            {
                return Tuple.Create(nullLink, _messageBank.ErrorMessages["cancellationRequested"]);
            }
            catch (Exception ex)
            {
                return Tuple.Create(nullLink, "500: Database: " + ex);
            }
        }

        /// <summary>
        ///     GetTotalRecoveryLinksAsync()
        ///         Returns an integer count of all recovery links currently in the database matching credentials.
        /// </summary>
        /// <param name="email">Email credential of user</param>
        /// <param name="authorizationLevel">Authorization level of user</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Tuple containing count of all recovery links and string status code</returns>
        public async Task<Tuple<int, string>> GetTotalRecoveryLinksAsync(string email, string authorizationLevel, CancellationToken cancellationToken = default(CancellationToken))
        {

            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                using (var connection = new SqlConnection(_options.SqlConnectionString))
                {
                    //Perform sql statement
                    var procedure = "[GetTotalRecoveryLinks]";          // Stored procedure
                    var value = new { Username = email, AuthorizationLevel = authorizationLevel };               // Guid to search in table
                    int totalLinks = await connection.ExecuteScalarAsync<int>(new CommandDefinition(procedure, value, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken)).ConfigureAwait(false);
                    //Check for cancellation...no rollback necessary
                    if (cancellationToken.IsCancellationRequested)
                        return Tuple.Create(-1, _messageBank.ErrorMessages["cancellationRequested"]);

                    return Tuple.Create(totalLinks, _messageBank.SuccessMessages["generic"]);
                }
            }
            catch (OperationCanceledException ex)
            {
                return Tuple.Create(-1, _messageBank.ErrorMessages["cancellationRequested"]);
            }
            catch (Exception ex)
            {
                return Tuple.Create(-1, "500: Database: " + ex);
            }
        }

        /// <summary>
        ///     RemoveAllRecoveryLinksAsync()
        ///         Removes all recovery lists existing in a database with a given email and authorization level 
        /// </summary>
        /// <param name="email">Email of user to delete recovery links</param>
        /// <param name="authorizationLevel">Authorization level of user to delete recover links</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>Tuple containing integer of links removed and string status code</returns>
        public async Task<Tuple<int, string>> RemoveAllRecoveryLinksAsync(string email, string authorizationLevel, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                using (var connection = new SqlConnection(_options.SqlConnectionString))
                {
                    //Perform sql statement
                    var procedure = "[RemoveUserRecoveryLinks]"; // Stored procedure
                    var value = new { Username = email, AuthorizationLevel = authorizationLevel };               // Guid to search in table
                    int linksRemoved = await connection.ExecuteScalarAsync<int>(new CommandDefinition(procedure, value, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken)).ConfigureAwait(false);

                    //Check if links removed
                    if (linksRemoved < 0)
                        return Tuple.Create(-1, _messageBank.ErrorMessages["recoveryLinkRemoveFail"]);
                    else
                        return Tuple.Create(linksRemoved, _messageBank.SuccessMessages["generic"]);
                }
            }
            catch (OperationCanceledException)
            {
                return Tuple.Create(-1, _messageBank.ErrorMessages["cancellationRequested"]);
            }
            catch (Exception ex)
            {
                return Tuple.Create(-1, "500: Database: " + ex);
            }
        }

        /// <summary>
        ///     CreateRecoveryLinkAsync()
        ///         Adds recovery link to database.
        /// </summary>
        /// <param name="recoveryLink">Recovery link object containing email, Guid, datetime created and authorization level of user</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>String status code</returns>
        public async Task<string> CreateRecoveryLinkAsync(IRecoveryLink recoveryLink, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                using (var connection = new SqlConnection(_options.SqlConnectionString))
                {
                    //Perform sql statement
                    var procedure = "[CreateRecoveryLink]";
                    var value = new { Username = recoveryLink.Username, GUIDLink = recoveryLink.GUIDLink, TimeCreated = recoveryLink.TimeCreated, AuthorizationLevel = recoveryLink.AuthorizationLevel };
                    var affectedRows = await connection.QueryAsync(new CommandDefinition(procedure, value, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken)).ConfigureAwait(false);

                    if (cancellationToken.IsCancellationRequested)
                    {
                        string rollbackResult = await RemoveRecoveryLinkAsync(recoveryLink);
                        if (rollbackResult != _messageBank.SuccessMessages["generic"])
                            return _messageBank.ErrorMessages["rollbackFailed"];
                        else
                            return _messageBank.ErrorMessages["cancellationRequested"];
                    }

                    return _messageBank.SuccessMessages["generic"];
                }
            }
            catch (OperationCanceledException)
            {
                return _messageBank.ErrorMessages["cancellationRequested"];
            }
            catch (Exception ex)
            {
                return "500: Database: " + ex;
            }
        }



        public List<string> CreateConfirmationLink(IConfirmationLink _confirmationlink)
        {
            List<string> result = new List<string>();
            try
            {
                using (var connection = new SqlConnection(_options.SqlConnectionString))
                {
                    var insertQuery = "INSERT INTO dbo.EmailConfirmationLinks (username, GUID, timestamp) VALUES (@Username, @UniqueIdentifier, @Datetime)";
                    int affectedRows = connection.Execute(insertQuery, _confirmationlink);

                    if (affectedRows == 1)
                        result.Add("Success - Confirmation Link added to database");
                    else
                        result.Add("Failed - Email already has confirmation link");
                }
            }
            catch (Exception ex)
            {
                result.Add("Failed - SQLDAO " + ex);
            }
            return result;
        }

        public IConfirmationLink GetConfirmationLink(string url)
        {

            string guidString = url.Substring(url.LastIndexOf('=') + 1);
            //Guid guid = new Guid(guidString);


            IConfirmationLink _confirmationLink = new ConfirmationLink();

            try
            {
                using (var connection = new SqlConnection(_options.SqlConnectionString))
                {

                    var readQuery = "SELECT username FROM dbo.EmailConfirmationLinks WHERE GUID = @guid";
                    _confirmationLink.Username = connection.ExecuteScalar<string>(readQuery, new { guid = guidString });
                    readQuery = "SELECT GUID FROM dbo.EmailConfirmationLinks WHERE GUID = @guid";
                    _confirmationLink.UniqueIdentifier = connection.ExecuteScalar<Guid>(readQuery, new { guid = guidString });
                    readQuery = "SELECT datetime FROM dbo.EmailConfirmationLinks WHERE GUID = @guid";
                    _confirmationLink.Datetime = connection.ExecuteScalar<DateTime>(readQuery, new { guid = guidString });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return _confirmationLink;
            }

            return _confirmationLink;
        }


        public List<string> ConfirmAccount(IAccount account)
        {
            List<string> results = new List<string>();
            int affectedRows;
            try
            {
                using (var connection = new SqlConnection(_options.SqlConnectionString))
                {
                    var updateQuery = "UPDATE dbo.Accounts SET confirmation = 1 WHERE email = @Email and username = @Username";
                    affectedRows = connection.Execute(updateQuery, account);

                }
                if (affectedRows == 1)
                    results.Add("Success - Account confirmed in database");
                else
                    results.Add("Failed - Account doesn't exist in database");
            }
            catch
            {
                results.Add("Failed - SQLDAO  could not be confirm account in database");
            }
            return results;
        }
        public bool DeleteConfirmationLink(IConfirmationLink confirmationLink)
        {
            int affectedRows;
            try
            {
                using (var connection = new SqlConnection(_options.SqlConnectionString))
                {
                    var deleteQuery = "DELETE FROM dbo.EmailConfirmationLinks WHERE @Username=username and @Guid=guid and @Timestamp=Timestamp";
                    affectedRows = connection.Execute(deleteQuery, confirmationLink);
                }
                if (affectedRows == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public List<string> CreateAccount(IAccount account)
        {
            List<string> results = new List<string>();
            int affectedRows;
            try
            {
                using (var connection = new SqlConnection(_options.SqlConnectionString))
                {
                    var readQuery = "SELECT COUNT(*) FROM dbo.Accounts WHERE Email = @Email";
                    var accounts = connection.ExecuteScalar<int>(readQuery, new { Email = account.Email });

                    if (accounts > 0)
                    {
                        results.Add("Failed - Account already exists in database");
                        return results;
                    }
                    var insertQuery = "INSERT INTO dbo.Accounts (Username, Email, Passphrase, AuthorizationLevel, AccountStatus, Confirmed) " +
                        "VALUES (@Username, @Email, @Passphrase, @AuthorizationLevel, @AccountStatus, @Confirmed)";

                    affectedRows = connection.Execute(insertQuery, account);
                }
                if (affectedRows == 1)
                    results.Add("Success - Account created in database");
                else
                    results.Add("Failed - Account not created in database");
            }
            catch (Exception ex)
            {
                results.Add("Failed - " + ex);
            }
            return results;
        }

        public IAccount GetUnconfirmedAccount(string email)
        {
            IAccount account;
            try
            {
                using (var connection = new SqlConnection(_options.SqlConnectionString))
                {
                    var readQuery = "SELECT username FROM dbo.user_accounts WHERE email = @Email and authorization_level = 'User'";
                    string username = connection.ExecuteScalar<string>(readQuery, new { Email = email });
                    readQuery = "SELECT passphrase FROM dbo.user_accounts WHERE email = @Email and authorization_level = 'User'";
                    string passphrase = connection.ExecuteScalar<string>(readQuery, new { Email = email });
                    readQuery = "SELECT account_status FROM dbo.user_accounts WHERE email = @Email and authorization_level = 'User'";
                    bool status = connection.ExecuteScalar<bool>(readQuery, new { Email = email });
                    account = new Account(email, username, passphrase, "User", status, false);
                    return account;
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                return null;
            }

        }


        public List<string> RemoveConfirmationLink(IConfirmationLink confirmationLink)
        {
            List<string> results = new List<string>();
            int affectedRows;

            try
            {
                using (var connection = new SqlConnection(_options.SqlConnectionString))
                {
                    var deleteQuery = "DELETE FROM dbo.confirmation_links WHERE GUID = @guid";
                    affectedRows = connection.Execute(deleteQuery, new { guid = confirmationLink.UniqueIdentifier });
                }
                if (affectedRows == 1)
                    results.Add("Success - Confirmation Link removed from database");
                else
                    results.Add("Failed - Confirmation link unable to be removed from database");
            }
            catch (Exception ex)
            {
                results.Add("Failed - Confirmation link not removed in database" + ex);
            }
            return results;
        }
        public string DeleteAccount()
        {

            int affectedRows;
            string userAuthLevel = Thread.CurrentPrincipal.IsInRole("admin") ? "admin" : "user";
            try
            {
                using (var connection = new SqlConnection(_options.SqlConnectionString))
                {
                    var readQuery = "SELECT * FROM Accounts WHERE Username = @username AND AuthorizationLevel = @role";
                    var account = connection.ExecuteScalar<int>(readQuery, new { username = Thread.CurrentPrincipal.Identity.Name, role = userAuthLevel });
                    if (account == 0)
                    {
                        return _messageBank.ErrorMessages["notFoundOrAuthorized"];
                    }
                    else
                    {
                        var storedProcedure = "CREATE PROCEDURE dbo.deleteAccount @username varchar(25) AS BEGIN" +
                            "DELETE FROM Accounts WHERE Username = @username;" +
                            "DELETE FROM OTPClaims WHERE Username = @username;" +
                            "DELETE FROM Nodes WHERE account_own = @username;" +
                            "DELETE FROM UserRatings WHERE Username = @username;" +
                            "DELETE FROM EmailConfirmationLinks WHERE username = @username;" +
                            "END";

                        affectedRows = connection.Execute(storedProcedure, Thread.CurrentPrincipal.Identity.Name);
                    }


                }

                if (affectedRows >= 1)
                {
                    return _messageBank.SuccessMessages["generic"];
                }
                else
                {
                    return _messageBank.ErrorMessages["notFoundOrAuthorized"];
                }
            }
            catch (AccountDeletionFailedException adfe)
            {
                return adfe.Message;
            }

        }

        public async Task<int> VerifyAccountAsync(IAccount account, 
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var connection = new SqlConnection(_options.SqlConnectionString))
            {
                //Perform sql statement
                var procedure = "[VerifyAccount]";
                var parameters = new DynamicParameters();
                parameters.Add("Username", account.Username);
                parameters.Add("AuthorizationLevel", account.AuthorizationLevel);
                parameters.Add("Result", dbType: DbType.Int32, direction: ParameterDirection.Output);
                var result = await connection.ExecuteAsync(new CommandDefinition(procedure, parameters,
                    commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken))
                    .ConfigureAwait(false);

                return parameters.Get<int>("Result");
            }
        }

        public async Task<int> AuthenticateAsync(IOTPClaim otpClaim, string jwtToken,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var connection = new SqlConnection(_options.SqlConnectionString))
            {
                //Perform sql statement
                var procedure = "[Authenticate]";
                var parameters = new DynamicParameters();
                parameters.Add("Username", otpClaim.Username);
                parameters.Add("OTP", otpClaim.OTP);
                parameters.Add("AuthorizationLevel", otpClaim.AuthorizationLevel);
                parameters.Add("TimeCreated", otpClaim.TimeCreated);
                parameters.Add("Token", jwtToken);
                parameters.Add("Result", dbType: DbType.Int32, direction: ParameterDirection.Output);
                var result = await connection.ExecuteAsync(new CommandDefinition(procedure, parameters,
                    commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken))
                    .ConfigureAwait(false);

                return parameters.Get<int>("Result");

                // Just make the decision, multiple calls or leak abstraction
                // Do as stored procedure, do all this work in the database, one request and
                // encapsulated, and faster - requires database to have concept of stored procedure
                // Would need to make a decision again if not using a relational DB

                // Service layer responsibility - interpret return of DAO
                // Service that Creates User - generic logic to be reusable
                //  DAO says "row added", Service says "User created"

                // This should be done at service level
                // (DAO only does a SQL operation then returns results)
                // If not related to DB, know it should not be in DAO

                // In catch block in above layer, need to roll back if cancel requested
                // check token before operation, if cancelled then dont do op
                // check token after operation, if cancelled, then rollback (closer to DAO, easier it is to rollback)
                // design decision to rollback or not
                // if cancel at end, designate as either cancelled or undo situation
            }
        }

        public async Task<int> StoreOTPAsync(IAccount account, IOTPClaim otpClaim, 
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var connection = new SqlConnection(_options.SqlConnectionString))
            {
                //Perform sql statement
                var procedure = "[StoreOTP]";
                var parameters = new DynamicParameters();
                parameters.Add("Username", otpClaim.Username);
                parameters.Add("AuthorizationLevel", otpClaim.AuthorizationLevel);
                parameters.Add("Passphrase", account.Passphrase);
                parameters.Add("OTP", otpClaim.OTP);
                parameters.Add("TimeCreated", otpClaim.TimeCreated);
                parameters.Add("Result", dbType: DbType.Int32, direction: ParameterDirection.Output);
                var result = await connection.ExecuteAsync(new CommandDefinition(procedure, parameters, 
                    commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken))
                    .ConfigureAwait(false);

                return parameters.Get<int>("Result");
            }
        }

        /*public List<IKPI> LoadKPI(DateTime now)
        {
            List<IKPI> kpiList = new List<IKPI>();
            kpiList.Add(GetViewKPI());
            kpiList.Add(GetViewDurationKPI());
            kpiList.Add(GetNodeKPI(now));
            kpiList.Add(GetLoginKPI(now));
            kpiList.Add(GetRegistrationKPI(now));
            kpiList.Add(GetSearchKPI(now));
            return kpiList;
        }*/


        //1/6
        public async Task<IViewKPI> GetViewKPIAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            IViewKPI vKPI = new ViewKPI();
            try
            {
                IList<View> views = await GetAllViewsAsync(cancellationToken).ConfigureAwait(false);
                if (views.Count == 0)
                {
                    vKPI.result = "No Database Entires";
                }
                int n = views.Count;
                for(int i = 1; i <= 5; i++){
                    vKPI.views.Add(views[(n-i)]);
                }
                vKPI.result = "Success";
                return vKPI;
            }
            catch (Exception ex)
            {
                vKPI.result = ("500: Database: " + ex.Message);
                return vKPI;
            }
        }

        //2/6
        public async Task<IViewDurationKPI> GetViewDurationKPIAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            IViewDurationKPI vDKPI = new ViewDurationKPI();
            try
            {
                IList<View> views = await GetAllViewsAsync(cancellationToken).ConfigureAwait(false);
                if(views.Count == 0)
                {
                    vDKPI.result = "No Database Entries";
                    return vDKPI;
                }
                int n = views.Count;
                for(int i = 1; i < 5; i++)
                {
                    vDKPI.views.Add(views[(n - i)]);
                }
                vDKPI.result = "Success";
                return vDKPI;
            }
            catch(Exception ex)
            {
                vDKPI.result = ("500: Database: " + ex.Message);
                return vDKPI;
            }
        }

        /*public INodeKPI GetNodeKPI(DateTime now)
        {
            INodeKPI nodeKPI = new NodeKPI();
            IList<INodesCreated> nC = GetNodesCreated(now);
            if (nC.Count == 0)
            {
                nodeKPI.result = "Error";
                return nodeKPI;
            }
            for (int i = 1; i < nC.Count; i++)
            {
                nodeKPI.nodesCreated.Add(nC[(nC.Count - 1)]);
            }
            nodeKPI.result = "success";
            return nodeKPI;
        }*/

        /*public INodeKPI GetNodeKPI(DateTime now)
        {
            INodeKPI nodeKPI = new NodeKPI();
            int counter = 1;
            INodesCreated nCreated = GetNodesCreated(now);
            if (nCreated.nodeCreationCount == -1)
            {
                nodeKPI.result = "Error";
                return nodeKPI;
            }
            while((counter < 29) && (nCreated.nodeCreationCount != -1))
            {
                nodeKPI.nodesCreated.Add(nCreated);
                DateTime past = now.AddDays((counter * -1));
                nCreated = GetNodesCreated(past);
                counter++;
            }
            nodeKPI.result = "success";
            return nodeKPI;
        }*/

        //3/6
        public async Task<INodeKPI> GetNodeKPIAsync(DateTime now, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            INodeKPI nKPI = new NodeKPI();
            try
            {
                IList<NodesCreated> nodes = await GetNodesCreatedAsync(now, cancellationToken).ConfigureAwait(false);
                if(nodes.Count == 0)
                {
                    nKPI.result = "No Database Entries";
                    return nKPI;
                }
                foreach(var x in nodes)
                {
                    nKPI.nodesCreated.Add(x);
                }
                nKPI.result = "Success";
                return nKPI;
            }
            catch(Exception ex)
            {
                nKPI.result = ("500: Database: " + ex.Message);
                return nKPI;
            }
        }

        /*//4/6
        public async Task<ILoginKPI> GetLoginKPI(DateTime now)
        {
            ILoginKPI loginKPI = new LoginKPI();
            int counter = 1;
            IList<IailyLogin> dLogin = GetDailyLogin(now);
            if (dLogin.loginCount == -1)
            {
                loginKPI.result = "Error";
                return loginKPI;
            }
            while ((counter <= 90) && (dLogin.loginCount != -1))
            {
                loginKPI.dailyLogins.Add(dLogin);
                DateTime past = now.AddDays((counter * -1));
                dLogin = GetDailyLogin(past);
                counter++;
            }
            loginKPI.result = "success";
            return loginKPI;
        }*/

        //4/6
        public async Task<ILoginKPI> GetLoginKPIAsync(DateTime now, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ILoginKPI lKPI = new LoginKPI();
            try
            {
                IList<DailyLogin> logins = new List<DailyLogin>();
                logins = await GetDailyLoginAsync(now, cancellationToken).ConfigureAwait(false);
                if(logins.Count == 0)
                {
                    lKPI.result = "No Database Entries";
                    return lKPI;
                }
                foreach(var x in logins)
                {
                    lKPI.dailyLogins.Add(x);
                }
                lKPI.result = "Success";
                return lKPI;
            }
            catch(Exception ex)
            {
                lKPI.result = ("500: Database: " + ex.Message);
                return lKPI;
            }
        }

        /*public IRegistrationKPI GetRegistrationKPI(DateTime now)
        {
            IRegistrationKPI registrationKPI = new RegistrationKPI();
            int counter = 1;
            IDailyRegistration dRegistration = GetDailyRegistration(now);
            if (dRegistration.registrationCount == -1)
            {
                registrationKPI.result = "Error";
                return registrationKPI;
            }
            while ((counter <= 90) && (dRegistration.registrationCount != -1))
            {
                registrationKPI.dailyRegistrations.Add(dRegistration);
                DateTime past = now.AddDays((counter * -1));
                dRegistration = GetDailyRegistration(past);
                counter++;
            }
            registrationKPI.result = "success";
            return registrationKPI;
        }*/
        //5/6
        public async Task<IRegistrationKPI> GetRegistrationKPIAsync(DateTime now, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            IRegistrationKPI rKPI = new RegistrationKPI();
            try
            {
                IList<DailyRegistration> registrations = new List<DailyRegistration>();
                registrations = await GetDailyRegistrationAsync(now, cancellationToken).ConfigureAwait(false);
                if(registrations.Count == 0)
                {
                    rKPI.result = "No Database Entries";
                    return rKPI;
                }
                foreach(var x in registrations)
                {
                    rKPI.dailyRegistrations.Add(x);
                }
                rKPI.result = "Success";
                return rKPI;
            }
            catch(Exception ex)
            {
                rKPI.result = ("500: Database: " + ex.Message);
                return rKPI;
            }
        }

        /*//6/6
        public ISearchKPI GetSearchKPI(DateTime now)
        {
            ISearchKPI searchKPI = new SearchKPI();
            int counter = 1;
            ITopSearch sCreated = GetTopSearch(now);//Initial Check to see if InMemoryDatabase is not empty
            List<ITopSearch> preSort = new List<ITopSearch>();
            if (sCreated.searchCount == -1)
            {
                searchKPI.result = "Error";
                return searchKPI;
            }

            while ((counter <= 28) && (sCreated.searchCount != -1))
            {
                preSort.Add(sCreated);
                DateTime past = now.AddDays((counter * -1));
                sCreated = GetTopSearch(past);
                counter++;
            }

            List<ITopSearch> afterSort = preSort.OrderBy(x => x.searchCount).ToList();
            int n = (afterSort.Count);
            for (int i = 1; i <= 5 || i < n; i++)
            {
                Console.WriteLine(n);
                searchKPI.topSearches.Add(afterSort[(n - i)]);
            }
            searchKPI.result = "success";
            return searchKPI;
        }*/

        //6/6
        public async Task<ISearchKPI> GetSearchKPIAsync(DateTime now, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ISearchKPI sKPI = new SearchKPI();
            try
            {
                IList<TopSearch> searches = new List<TopSearch>();
                searches = await GetTopSearchAsync(now, cancellationToken).ConfigureAwait(false);
                if(searches.Count == 0)
                {
                    sKPI.result = "No Database Entries";
                    return sKPI;
                }
                IList<TopSearch> sorted = searches.OrderBy(x => x.searchCount).ToList();
                int n = sorted.Count;
                for(int i = 1; i <= 5 || i < n; i++)
                {
                    sKPI.topSearches.Add(sorted[(n-i)]);
                }
                sKPI.result = "Success";
                return sKPI;
            }
            catch(Exception ex)
            {
                sKPI.result = ("500: Database: " + ex.Message);
                return sKPI;
            }
        }

        //Done
        public async Task<List<View>> GetAllViewsAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            List<View> results = new List<View>();
            try
            {
                using (var connection = new SqlConnection(_options.SqlConnectionString))
                {
                    //return connection.Execute<IView>("SELECT DateCreated, ViewName, Visits, AverageDuration from dbo.ViewTable").ToList();
                    var selectQuery = "SELECT DateCreated, ViewName, Visits, AverageDuration FROM dbo.ViewTable";
                    results = (await connection.QueryAsync<View>(new CommandDefinition(selectQuery, cancellationToken: cancellationToken)).ConfigureAwait(false)).ToList();
                    return results;
                }
            }
            catch (Exception ex)
            {
                return results;
            }
        }

        public string CreateView(IView view)
        {
            int affectedRows;
            try
            {
                using (var connection = new SqlConnection(_options.SqlConnectionString))
                {
                    var insertQuery = @"INSERT INTO dbo.ViewTable (DateCreated, ViewName, Visits, AverageDuration)" +
                        "Values (@DateCreated, @ViewName, @Visits, @AverageDuration)";
                    affectedRows = connection.Execute(insertQuery, new
                    {
                        DateCreated = view.date,
                        ViewName = view.viewName,
                        Visits = view.visits,
                        AverageDuration = view.averageDuration
                    });
                }
                if (affectedRows == 1)
                {
                    return "View Creation Successful";
                }
                else
                {
                    return "View Creation Failed";
                }
            }
            catch (Exception ex)
            {
                return "Fail";
            }
        }

        public string CreateNodesCreated(INodesCreated nodesCreated)
        {
            try
            {
                using (var connection = new SqlConnection(_options.SqlConnectionString))
                {
                    var selectQuery = "SELECT * FROM Tresearch.nodes_created" +
                                      "WHERE _node_creation_date >= @node_creation_date - 30";

                    int affectedRows = connection.Execute(selectQuery,
                                    new
                                    {
                                        nodesCreatedDate = nodesCreated.nodeCreationDate,
                                        nodesCreatedCount = nodesCreated.nodeCreationCount
                                    });
                }
                return _messageBank.SuccessMessages["generic"];
            }
            catch (Exception ex)
            {
                return _messageBank.ErrorMessages["createdNodesExists"];
            }
        }

        public async Task<List<NodesCreated>> GetNodesCreatedAsync(DateTime nodesCreatedDate, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            List<NodesCreated> results = new List<NodesCreated>();
            try
            {
                using (var connection = new SqlConnection(_options.SqlConnectionString))
                {
                    var selectQuery = "SELECT * FROM tresearchStudentServer.dbo.nodesCreated WHERE nodesCreatedDate BETWEEN DATEADD(day, -30, @nodeCreationDate) AND @nodeCreationDate";
                    results = (await connection.QueryAsync<NodesCreated>(new CommandDefinition(selectQuery, new { nodeCreationDate = nodesCreatedDate }, cancellationToken: cancellationToken)).ConfigureAwait(false)).ToList();
                    return results;
                }
            }
            catch
            {
                return results;
            }
        }

        public string UpdateNodesCreated(INodesCreated nodesCreated)
        {
            using (var connection = new SqlConnection(_options.SqlConnectionString))
            {
                string updateQuery = @"UPDATE tresearchStudentServer.dbo.nodesCreated SET nodesCreatedCount = @nodesCreatedCount WHERE nodesCreatedDate = @nodesCreatedDate";

                int rowsAffected = connection.Execute(updateQuery,
                            new
                            {
                                nodesCreatedDate = nodesCreated.nodeCreationDate,
                                nodesCreatedCount = nodesCreated.nodeCreationCount
                            }
                            );
                if (rowsAffected == 1)
                {
                    return _messageBank.SuccessMessages["generic"];
                }
                else
                {
                    return _messageBank.ErrorMessages["createdNodesNotExists"];
                }
            }
        }



        public string CreateDailyLogin(IDailyLogin dailyLogin)
        {
            try
            {
                using (var connection = new SqlConnection(_options.SqlConnectionString))
                {
                    string insertQuery = @"INSERT INTO tresearchStudentServer.dbo.dailyLogins (loginDate, loginCount)
                                        Values (@loginDate, @loginCount)";
                    int affectedRows = connection.Execute(insertQuery,
                                        new
                                        {
                                            loginDate = dailyLogin.loginDate,
                                            loginCount = dailyLogin.loginCount
                                        });
                }
                return _messageBank.SuccessMessages["generic"];
            }
            catch (Exception ex)
            {
                return _messageBank.ErrorMessages["dailyLoginsExists"];
            }
        }

        public async Task<List<DailyLogin>> GetDailyLoginAsync(DateTime getDate, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            List<DailyLogin> results = new List<DailyLogin>();
            try
            {
                using (var connection = new SqlConnection(_options.SqlConnectionString))
                {
                    var selectQuery = "SELECT * FROM tresearchStudentServer.dbo.dailyLogins WHERE loginDate BETWEEN DATEADD(day, -30, @loginDate) AND @loginDate";
                    results = (await connection.QueryAsync<DailyLogin>(new CommandDefinition(selectQuery, new { loginDate = getDate }, cancellationToken: cancellationToken)).ConfigureAwait(false)).ToList();
                    return results;
                }
            }
            catch
            {
                return results;
            }
        }

        public string UpdateDailyLogin(IDailyLogin dailyLogin)
        {
            IDailyLogin logins;

            using (var connection = new SqlConnection(_options.SqlConnectionString))
            {
                string updateQuery = @"UPDATE tresearchStudentServer.dbo.dailyLogins SET loginCount = @loginCount WHERE loginDate = @loginDate";

                int rowsAffected = connection.Execute(updateQuery, new
                {
                    loginDate = dailyLogin.loginDate,
                    loginCount = dailyLogin.loginCount
                });

                if (rowsAffected == 1)
                {
                    return _messageBank.SuccessMessages["generic"];
                }
                else
                {
                    return _messageBank.ErrorMessages["dailyLoginsNotExists"];
                }
            }
        }



        public string CreateTopSearch(ITopSearch topSearch)
        {
            try
            {
                using (var connection = new SqlConnection(_options.SqlConnectionString))
                {
                    string insertQuery = @"INSERT INTO tresearchStudentServer.dbo.TopSearches (topSearchDate, topSearchString, topSearchCount) VALUES (@topSearchDate, @topSearchString, @topSearchCount)";

                    int affectedRows = connection.Execute(insertQuery,
                                        new
                                        {
                                            topSearchDate = topSearch.topSearchDate,
                                            topSearchString = topSearch.searchString,
                                            topSearchCount = topSearch.searchCount
                                        });
                }

                return _messageBank.SuccessMessages["generic"];
            }
            catch (Exception ex)
            {
                return _messageBank.ErrorMessages["topSearchesExists"];
            }
        }

        public async Task<List<TopSearch>> GetTopSearchAsync(DateTime getTopSearchDate, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            List<TopSearch> result = new List<TopSearch>();
            try
            {
                using (var connection = new SqlConnection(_options.SqlConnectionString))
                {
                    string selectQuery = "SELECT * FROM tresearchStudentServer.dbo.topSearches WHERE topSearchDate BETWEEN DATEADD(day, -30, @topSearchDate) AND @topSearchDate;";
                    result = (await connection.QueryAsync<TopSearch>(new CommandDefinition(selectQuery, new { topSearchDate = getTopSearchDate }, cancellationToken: cancellationToken)).ConfigureAwait(false)).ToList();
                    return result;
                }
            }
            catch
            {
                return result;
            }
        }

        public string UpdateTopSearch(ITopSearch topSearch)
        {
            using (var connection = new SqlConnection(_options.SqlConnectionString))
            {
                string updateQuery = @"UPDATE tresearchStudentServer.dbo.topSearches SET topSearchCount = @topSearchCount, topSearchString = @topSearchString WHERE topSearchDate = @topSearchDate;";

                int rowsAffected = connection.Execute(updateQuery,
                                                    new
                                                    {
                                                        topSearchDate = topSearch.topSearchDate,
                                                        topSearchString = topSearch.searchCount,
                                                        topSearchCount = topSearch.searchCount
                                                    });
                if (rowsAffected == 1)
                {
                    return _messageBank.SuccessMessages["generic"];
                }
                else
                {
                    return _messageBank.ErrorMessages["topSearchesNotExists"];
                }
            }
        }



        public string CreateDailyRegistration(IDailyRegistration dailyRegistration)
        {
            int affectedRows;
            try
            {
                using (var connection = new SqlConnection(_options.SqlConnectionString))
                {
                    var insertQuery = @"INSERT INTO tresearchStudentServer.dbo.dailyRegistrations (registrationDate, registrationCount) VALUES (@registrationDate, @registrationCount)";
                    affectedRows = connection.Execute(insertQuery,
                                     new
                                     {
                                         registrationDate = dailyRegistration.registrationDate,
                                         registrationCount = dailyRegistration.registrationCount
                                     });
                }
                return _messageBank.SuccessMessages["generic"];
            }
            catch (Exception ex)
            {
                return _messageBank.ErrorMessages["dailyRegistrationsExists"];
            }
        }

        public async Task<List<DailyRegistration>> GetDailyRegistrationAsync(DateTime getRegistrationDate, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            List<DailyRegistration> results = new List<DailyRegistration>();
            try
            {
                using (var connection = new SqlConnection(_options.SqlConnectionString))
                {
                    var selectQuery = "SELECT * FROM tresearchStudentServer.dbo.dailyRegistrations WHERE registrationDate BETWEEN DATEADD(day, -30, @registrationDate) AND @registrationDate";
                    results = (await connection.QueryAsync<DailyRegistration>(new CommandDefinition(selectQuery, new { registrationDate = getRegistrationDate }, cancellationToken: cancellationToken)).ConfigureAwait(false)).ToList();
                    return results;
                }
            }
            catch
            {
                return results;
            }
        }

        public string UpdateDailyRegistration(IDailyRegistration dailyRegistration)
        {
            using (var connection = new SqlConnection(_options.SqlConnectionString))
            {
                string updateQuery = @"UPDATE tresearchStudentServer.dbo.dailyRegistrations SET registrationCount = @registrationCount WHERE registrationDate = @registrationDate";

                int rowsAffected = connection.Execute(updateQuery,
                                new
                                {
                                    registrationDate = dailyRegistration.registrationDate,
                                    registrationCount = dailyRegistration.registrationCount
                                });

                if (rowsAffected == 1)
                {
                    return _messageBank.SuccessMessages["generic"];
                }
                else
                {
                    return _messageBank.ErrorMessages["dailyRegistrationsNotExists"];
                }
            }
        }
    }
}