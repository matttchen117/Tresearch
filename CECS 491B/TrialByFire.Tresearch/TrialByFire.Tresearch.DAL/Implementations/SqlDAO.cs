﻿using Dapper;
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
                cancellationToken.ThrowIfCancellationRequested();                                                       // Check if cancellation token has requested cancellation
                using (var connection = new SqlConnection(_options.SqlConnectionString))                                        // Establish connection with database
                {
                    //Perform sql statement
                    var procedure = "dbo.[DisableAccount]";                                                                 // Name of store procedure
                    var value = new { Username = email, AuthorizationLevel = authorizationLevel };   //Columns to check in database
                    int affectedRows = await connection.ExecuteAsync(new CommandDefinition(procedure, value, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken)).ConfigureAwait(false);

                    //Check if cancellation is requested
                    if (cancellationToken.IsCancellationRequested)
                    {
                        //Cancellation has been requested, undo everything
                        string rollbackResult = await EnableAccountAsync(email, authorizationLevel);                                      // Enables account.. result should be generic success
                        if (rollbackResult != _messageBank.GetMessage(IMessageBank.Responses.generic).Result)
                            return _messageBank.GetMessage(IMessageBank.Responses.rollbackFailed).Result;                                       // Rollback failed, account is still in database
                        else
                            throw new OperationCanceledException();                                 // Cancellation requested, successfully rolledback account disable
                    }

                    //Check rows affected... If account exists, should be 1 otherwise error
                    if (affectedRows < 1)
                        return _messageBank.GetMessage(IMessageBank.Responses.accountNotFound).Result;
                    else
                        return _messageBank.GetMessage(IMessageBank.Responses.generic).Result;
                }
            }
            catch (OperationCanceledException)
            {
                // Cancellation requested, nothing to rollback
                throw;
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
                using (var connection = new SqlConnection(_options.SqlConnectionString))                                    // Establish connection with database
                {
                    //Perform sql statement
                    var procedure = "dbo.[EnableAccount]";                                                                      // Store Procedure
                    var value = new { Username = email, AuthorizationLevel = authorizationLevel };
                    var affectedRows = await connection.ExecuteAsync(new CommandDefinition(procedure, value, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken)).ConfigureAwait(false);

                    // Check if cancellation is requested
                    if (cancellationToken.IsCancellationRequested)
                    {
                        string rollbackResult = await DisableAccountAsync(email, authorizationLevel);
                        if (rollbackResult != _messageBank.GetMessage(IMessageBank.Responses.generic).Result)
                            return _messageBank.GetMessage(IMessageBank.Responses.rollbackFailed).Result;
                        else
                            throw new OperationCanceledException();
                    }

                    if(affectedRows < 1)
                        return _messageBank.GetMessage(IMessageBank.Responses.accountNotFound).Result;
                    else
                        return _messageBank.GetMessage(IMessageBank.Responses.generic).Result;
                }
            }
            catch (SqlException ex)
            {
                switch (ex.Number)
                {
                    case 547:   //Adding recovery link violates foreign key constraint (AKA NO ACCOUNT)
                        return _messageBank.GetMessage(IMessageBank.Responses.accountNotFound).Result;
                    default:
                        return "500: Database: " + ex.Message;
                }
            }
            catch (OperationCanceledException)
            {
                // Cancellation requested, nothing to rollback
                throw;
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
                    await connection.OpenAsync();
                    var procedure = "dbo.[GetAccount]";
                    var parameters = new { Username = email, AuthorizationLevel = authorizationLevel };

                    var Accounts = await connection.QueryAsync<Account>(new CommandDefinition(procedure, parameters, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken)).ConfigureAwait(false);
                    
                    //Check if account was returned
                    if (Accounts.Count() < 1)
                        return Tuple.Create(nullAccount, _messageBank.GetMessage(IMessageBank.Responses.accountNotFound).Result);            //Account doesn't exist

                    IAccount account = Accounts.FirstOrDefault();


                    // Check if cancellation is requested .. no rollback necessary
                    if (cancellationToken.IsCancellationRequested)
                        throw new OperationCanceledException();
                    else
                        return Tuple.Create(account, _messageBank.GetMessage(IMessageBank.Responses.generic).Result);
                }
            }
            catch (OperationCanceledException)
            {
                //No rollback necessary
                throw;
            }
            catch (Exception ex)
            {
                return Tuple.Create(nullAccount, "500: server:" + ex.Message);
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
                    var procedure = "dbo.[RemoveRecoveryLink]";
                    var value = new { GUIDLink = recoveryLink.GUIDLink };
                    var affectedRows = await connection.ExecuteScalarAsync<int>(new CommandDefinition(procedure, value, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken)).ConfigureAwait(false);

                    //Check if cancellation requested
                    if (cancellationToken.IsCancellationRequested)
                    {
                        string rollbackResults = await CreateRecoveryLinkAsync(recoveryLink);
                        if (rollbackResults != _messageBank.GetMessage(IMessageBank.Responses.generic).Result)
                            return _messageBank.GetMessage(IMessageBank.Responses.rollbackFailed).Result;
                        else
                            throw new OperationCanceledException();
                    }

                    //Check if recovery link removed
                    if (affectedRows >= 0)
                        return _messageBank.GetMessage(IMessageBank.Responses.generic).Result;
                    else
                        return _messageBank.GetMessage(IMessageBank.Responses.recoveryLinkRemoveFail).Result;
                }
            }
            catch (OperationCanceledException)
            {
                //No rollback required
                throw;
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
        public async Task<Tuple<IRecoveryLink, string>> GetRecoveryLinkAsync(string guidlink, CancellationToken cancellationToken = default(CancellationToken))
        {
            IRecoveryLink nullLink = null;
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                using (var connection = new SqlConnection(_options.SqlConnectionString))
                {
                    
                    //Perform sql statement
                    var procedureUsername = "dbo.[GetRecoveryLinkUsername]";                                    // Stored procedure
                    var procedureTimeCreated = "dbo.[GetRecoveryLinkTimeCreated]";
                    var procedureAuthorizationLevel = "dbo.[GetRecoveryLinkAuthorizationLevel]";
                    var value = new { GUIDLink = new Guid(guidlink) };
                    var parameters = new DynamicParameters();
                    parameters.Add("@GUIDLink", Guid.Parse(guidlink),DbType.Guid);
                    //command.Parameters.Add("@GUIDLink", SqlDbType.UniqueIdentifier).Value = new Guid(guidlink);
                    string username =  await connection.ExecuteScalarAsync<string>(new CommandDefinition(procedureUsername, parameters,commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken)).ConfigureAwait(false);
                    string timeCreated = await connection.ExecuteScalarAsync<string>(new CommandDefinition(procedureTimeCreated, parameters, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken)).ConfigureAwait(false);
                    string authorizationLevel = await connection.ExecuteScalarAsync<string>(new CommandDefinition(procedureAuthorizationLevel, parameters, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken)).ConfigureAwait(false);

                    if(username == null || timeCreated == null || authorizationLevel == null)
                        return Tuple.Create(nullLink, _messageBank.GetMessage(IMessageBank.Responses.recoveryLinkNotFound).Result);

                    IRecoveryLink link = new RecoveryLink(username, authorizationLevel, DateTime.Parse(timeCreated), new Guid(guidlink));

                    if (cancellationToken.IsCancellationRequested)
                        throw new OperationCanceledException();
                    else
                        return Tuple.Create(link, _messageBank.GetMessage(IMessageBank.Responses.generic).Result);
                }
            }
            catch(SqlException ex)
            {
                switch (ex.Number)
                {
                    case 547:   //Adding recovery link violates foreign key constraint (AKA NO ACCOUNT)
                        return Tuple.Create(nullLink, _messageBank.GetMessage(IMessageBank.Responses.recoveryLinkNotFound).Result);
                    default:
                        return Tuple.Create(nullLink, "500: Database: " + ex.Message);
                }
            }
            catch (OperationCanceledException)
            {
                //No rollback necessary
                throw;
            }
            catch (Exception ex)
            {
                return Tuple.Create(nullLink, "500: Database: " + ex.Message);
            }
        }

        public async Task<int> GetRecoveryLinkCountAsync(string email, string authorizationLevel, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                using (var connection = new SqlConnection(_options.SqlConnectionString))
                {
                    var procedure = "[GetRecoveryLinksCreated]";      //INSERT on duplicate key update linkscreted++
                    var value = new { Username = email, AuthorizationLevel = authorizationLevel };
                    var total = await connection.ExecuteScalarAsync<int>(new CommandDefinition(procedure, value, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken)).ConfigureAwait(false);
                    if (cancellationToken.IsCancellationRequested)
                        throw new OperationCanceledException();
                    return total;
                }
            }
            catch (SqlException ex)
            {
                switch (ex.Number)
                {
                    case 547:   //Adding recovery link violates foreign key constraint (AKA NO ACCOUNT)
                        return 0;
                    default:
                        return -1;
                }
            }
            catch (OperationCanceledException ex)
            {
                //No rollback necessary
                throw;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        /// <summary>
        ///     DecrementRecoveryLinkCountAsync(email, authorizationLevel, cancellationToken)
        ///         Decrements the count of recovery links a user has 
        /// </summary>
        /// <param name="email"> Email address of user to decrement</param>
        /// <param name="authorizationLevel">Authorization Level of user</param>
        /// <param name="cancellationToken">Cancellation Token </param>
        /// <returns></returns>
        public async Task<string> DecrementRecoveryLinkCountAsync(string email, string authorizationLevel, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                using (var connection = new SqlConnection(_options.SqlConnectionString))
                {
                    var procedure = "[DecrementRecoveryLinksCreated]";      //INSERT on duplicate key update linkscreted++
                    var value = new { Username = email, AuthorizationLevel = authorizationLevel };
                    var affectedRows = await connection.QueryAsync(new CommandDefinition(procedure, value, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken)).ConfigureAwait(false);
                    if (cancellationToken.IsCancellationRequested)
                    {
                        string rollbackResult = await IncrementRecoveryLinkCountAsync(email, authorizationLevel);
                        if (rollbackResult != _messageBank.GetMessage(IMessageBank.Responses.generic).Result)
                            return _messageBank.GetMessage(IMessageBank.Responses.rollbackFailed).Result;
                        else
                            throw new OperationCanceledException();
                    }
                    
                    return _messageBank.GetMessage(IMessageBank.Responses.generic).Result;
                }
            }
            catch (SqlException ex)
            {
                switch (ex.Number)
                {
                    case 547:   //Adding recovery link violates foreign key constraint (AKA NO ACCOUNT)
                        return _messageBank.GetMessage(IMessageBank.Responses.accountNotFound).Result;
                    default:
                        return "500: Database: " + ex.Message;
                }
            }
            catch (OperationCanceledException ex)
            {
                //No rollback necessary
                throw;
            }
            catch (Exception ex)
            {
                return "500: Database: " + ex.Message;
            }
        }

        /// <summary>
        ///     IncrementRecoveryLinkCountAsync(email, authorizationLevel, cancellationToken)
        ///         Increments the count of recovery links a user has 
        /// </summary>
        /// <param name="email">Email of user</param>
        /// <param name="authorizationLevel">Authorization Level of user</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns></returns>
        public async Task<string> IncrementRecoveryLinkCountAsync(string email, string authorizationLevel, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                using (var connection = new SqlConnection(_options.SqlConnectionString))
                {
                    var procedure = "[IncrementRecoveryLinksCreated]";      //INSERT on duplicate key update linkscreted++
                    var value = new { Username = email, AuthorizationLevel = authorizationLevel };
                    var affectedRows = await connection.QueryAsync(new CommandDefinition(procedure, value, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken)).ConfigureAwait(false);
                    if (cancellationToken.IsCancellationRequested)
                    {
                        string rollbackResult = await DecrementRecoveryLinkCountAsync(email, authorizationLevel);
                        if (rollbackResult != _messageBank.GetMessage(IMessageBank.Responses.generic).Result)
                            return _messageBank.GetMessage(IMessageBank.Responses.rollbackFailed).Result;
                        else
                            throw new OperationCanceledException();
                    }
                    return _messageBank.GetMessage(IMessageBank.Responses.generic).Result;
                }
            }
            catch(SqlException ex)
            {
                switch (ex.Number)
                {
                    case 547:   //Adding recovery link violates foreign key constraint (AKA NO ACCOUNT)
                        return _messageBank.GetMessage(IMessageBank.Responses.accountNotFound).Result;
                    default:
                        return "500: Database: " + ex.Message;
                }
            }
            catch (OperationCanceledException ex)
            {
                //No rollback necessary
                throw;
            }
            catch (Exception ex)
            {
                return "500: Database: " + ex.Message;
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
                    
                    //Check if cancellation is requested
                    if (cancellationToken.IsCancellationRequested)
                    {
                        //Perform rollback
                        string rollbackResult = await RemoveRecoveryLinkAsync(recoveryLink);
                        if (rollbackResult != _messageBank.GetMessage(IMessageBank.Responses.generic).Result)
                            return _messageBank.GetMessage(IMessageBank.Responses.rollbackFailed).Result;
                        else
                            throw new OperationCanceledException();
                    }

                    return _messageBank.GetMessage(IMessageBank.Responses.generic).Result;
                }
            }
            catch(SqlException ex)
            {
                switch (ex.Number)
                {
                    case 2627:  //Recovery link already exists in database
                        return _messageBank.GetMessage(IMessageBank.Responses.recoveryLinkExists).Result;
                    case 547:   //Adding recovery link violates foreign key constraint (AKA NO ACCOUNT)
                        return _messageBank.GetMessage(IMessageBank.Responses.accountNotFound).Result;
                    default:
                        return _messageBank.GetMessage(IMessageBank.Responses.recoveryLinkCreateFail).Result;
                }
            }
            catch (OperationCanceledException)
            {
                //No rollback necessary
                throw;
            }
            catch (Exception ex)
            {
                return "500: Database: " + ex;
            }
        }

        public async Task<string> CreateConfirmationLinkAsync(IConfirmationLink confirmationlink, CancellationToken cancellationToken = default(CancellationToken))
        { 
            try
            {
                using (var connection = new SqlConnection(_options.SqlConnectionString))
                {
                    var procedure = "[CreateConfirmationLink]";
                    var value = new { Username = confirmationlink.Username, GUIDLink = confirmationlink.GUIDLink, TimeCreated = confirmationlink.TimeCreated, AuthorizationLevel = confirmationlink.AuthorizationLevel };
                    var affectedRows = await connection.QueryAsync(new CommandDefinition(procedure, value, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken)).ConfigureAwait(false);
                    if (cancellationToken.IsCancellationRequested)
                    {
                        string rollbackResult = await RemoveConfirmationLinkAsync(confirmationlink);
                        if (rollbackResult != _messageBank.GetMessage(IMessageBank.Responses.generic).Result)
                            return _messageBank.GetMessage(IMessageBank.Responses.rollbackFailed).Result;
                        else
                            throw new OperationCanceledException();
                    }
                    return _messageBank.GetMessage(IMessageBank.Responses.generic).Result;
                }
            }
            catch(SqlException ex)
            {
                switch (ex.Number)
                {
                    default: return "500: Database: " + ex.Message;
                }
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                return "500: Database: " + ex.Message;
            }
        }

        public async Task<Tuple<IConfirmationLink, string>> GetConfirmationLinkAsync(string guid, CancellationToken cancellationToken = default(CancellationToken))
        {
            IConfirmationLink nullLink = null;
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                using (var connection = new SqlConnection(_options.SqlConnectionString))
                {

                    var procedure = "[GetConfirmationLink]";      //INSERT on duplicate key update linkscreted++
                    var value = new { GUIDLink = guid };
                    var link = await connection.QueryFirstAsync<IConfirmationLink>(new CommandDefinition(procedure, value, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken)).ConfigureAwait(false);
                    if (cancellationToken.IsCancellationRequested)
                        throw new OperationCanceledException();
                    return Tuple.Create(link, _messageBank.GetMessage(IMessageBank.Responses.generic).Result);
                }
            }
            catch(SqlException ex)
            {
                switch (ex.Number)
                {
                    default: return Tuple.Create(nullLink, "500: Database: " + ex.Message);
                }
            }
            catch (OperationCanceledException)
            {
                //Rollbackc taken care of
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Tuple.Create(nullLink, "500: Server: " + ex.Message);
            }

        }

        public async Task<string> UpdateAccountToUnconfirmedAsync(string username, string authorizationlevel, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                using (var connection = new SqlConnection(_options.SqlConnectionString))
                {
                    var procedure = "[UnConfirmAccount]";      //INSERT on duplicate key update linkscreted++
                    var value = new { Username = username, AuthorizationLevel = authorizationlevel };
                    var affectedRows = await connection.ExecuteAsync(new CommandDefinition(procedure, value, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken)).ConfigureAwait(false);

                    if (cancellationToken.IsCancellationRequested)
                    {
                        string rollbackResult = await UpdateAccountToConfirmedAsync(username, authorizationlevel);
                        if (rollbackResult != _messageBank.GetMessage(IMessageBank.Responses.generic).Result)
                            return _messageBank.GetMessage(IMessageBank.Responses.rollbackFailed).Result;
                        else
                            throw new OperationCanceledException();
                    }

                    if (affectedRows >= 1)
                        return _messageBank.GetMessage(IMessageBank.Responses.generic).Result;
                    else
                        return _messageBank.GetMessage(IMessageBank.Responses.accountNotFound).Result;
                }
            }

            catch (OperationCanceledException)
            {
                //Rolback taken care of 
                throw;
            }
            catch (Exception ex)
            {
                return "500: Server " + ex.Message;
            }
        }

        public async Task<string> UpdateAccountToConfirmedAsync(string username, string authorizationLevel, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                using (var connection = new SqlConnection(_options.SqlConnectionString))
                {
                    var procedure = "[ConfirmAccount]";      //INSERT on duplicate key update linkscreted++
                    var value = new { Username = username, AuthorizationLevel = authorizationLevel };
                    var affectedRows = await connection.ExecuteAsync(new CommandDefinition(procedure, value, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken)).ConfigureAwait(false);

                    if (cancellationToken.IsCancellationRequested)
                    {
                        string rollbackResult = await UpdateAccountToUnconfirmedAsync(username, authorizationLevel);
                        if (rollbackResult != _messageBank.GetMessage(IMessageBank.Responses.generic).Result)
                            return _messageBank.GetMessage(IMessageBank.Responses.rollbackFailed).Result;
                        else
                            throw new OperationCanceledException();
                    }

                    if (affectedRows >= 1)
                        return _messageBank.GetMessage(IMessageBank.Responses.generic).Result;
                    else
                        return _messageBank.GetMessage(IMessageBank.Responses.accountNotFound).Result;
                }
            }
             
            catch (OperationCanceledException)
            {
                //Rolback taken care of 
                throw;
            }
            catch(Exception ex)
            {
                    return "500: Server " + ex.Message;   
            }
        }
        public async Task<string> RemoveConfirmationLinkAsync(IConfirmationLink confirmationLink, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                using (var connection = new SqlConnection(_options.SqlConnectionString))
                {
                    var procedure = "dbo.[RemoveConfirmationLink]";
                    var value = new { GUIDLink = confirmationLink.GUIDLink };
                    var affectedRows = await connection.ExecuteAsync(new CommandDefinition(procedure, value, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken)).ConfigureAwait(false);

                    //Check if cancellation requested
                    if (cancellationToken.IsCancellationRequested)
                    {
                        string rollbackResults = await CreateConfirmationLinkAsync(confirmationLink);
                        if (rollbackResults != _messageBank.GetMessage(IMessageBank.Responses.generic).Result)
                            return _messageBank.GetMessage(IMessageBank.Responses.rollbackFailed).Result;
                        else
                            throw new OperationCanceledException();
                    }

                    //Check if recovery link removed
                    if (affectedRows >= 0)
                        return _messageBank.GetMessage(IMessageBank.Responses.generic).Result;
                    else
                        return _messageBank.GetMessage(IMessageBank.Responses.recoveryLinkRemoveFail).Result;

                }
            }
            catch(SqlException ex)
            {
                switch (ex.Number)
                {
                    default: return "500: Database: " + ex.Message;
                }
            }
            catch (OperationCanceledException)
            {
                // Rollback taken care of
                throw;
            }
            catch(Exception ex)
            {
                return "500: Server: " + ex.Message;
            }
        }

        public async Task<string> CreateAccountAsync(IAccount account, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                using (var connection = new SqlConnection(_options.SqlConnectionString))
                {
                    await connection.OpenAsync();

                    //Perform Sql Statement
                    var procedure = "dbo.[CreateAccount]";
                    var value = new
                    {
                        Username = account.Username,
                        Email = account.Email,
                        Passphrase = account.Passphrase,
                        AuthorizationLevel = account.AuthorizationLevel,
                        AccountStatus = account.AccountStatus,
                        Confirmed = account.Confirmed
                    };
                    var affectedRows = await connection.ExecuteAsync(new CommandDefinition(procedure, value, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken)).ConfigureAwait(false);
                    if (affectedRows == 1)
                        return _messageBank.GetMessage(IMessageBank.Responses.generic).Result;
                    else
                        return _messageBank.GetMessage(IMessageBank.Responses.accountAlreadyCreated).Result;    //CHECK IF THIS IS ACCOUNT ALREADY EXISTS
                }
            }
            catch(SqlException ex)
            {
                switch (ex.Number)
                {
                    default: return "";
                }
            }
            catch(OperationCanceledException)
            {
                // Rollback handled
                throw;
            }
            catch (Exception ex)
            {
                return "500 : Database: " + ex;
            }
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

        public async Task<string> VerifyAccountAsync(IAccount account, 
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                using (var connection = new SqlConnection(_options.SqlConnectionString))
                {
                    string query = "SELECT * FROM Accounts WHERE Username = @Username AND AuthorizationLevel = @AuthorizationLevel";
                    IAccount dbAccount = await connection.QueryFirstAsync<Account>(new CommandDefinition(
                        query, 
                        new{
                            Username = account.Username,
                            AuthorizationLevel = account.AuthorizationLevel
                        }, 
                        cancellationToken: default
                    )).ConfigureAwait(false);
                    if (dbAccount == null)
                    {
                        return _messageBank.ErrorMessages["notFoundOrAuthorized"];
                    }
                    else if (dbAccount.Confirmed == false)
                    {
                        return _messageBank.ErrorMessages["notConfirmed"];
                    }
                    else if (dbAccount.AccountStatus == false)
                    {
                        return _messageBank.ErrorMessages["notFoundOrEnabled"];
                    }
                    else
                    {
                        if (account.Passphrase.Equals(dbAccount.Passphrase))
                        {
                            return _messageBank.SuccessMessages["generic"];
                        }
                        else
                        {
                            return _messageBank.ErrorMessages["badNameOrPass"];
                        }
                    }
                }
            }
            catch (AccountCreationFailedException acfe)
            {
                return "400: Server: " + acfe.Message;
                //return acfe.Message;
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

        public async Task<List<string>> AuthenticateAsync(IOTPClaim otpClaim, 
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            List<string> results = new List<string>();
            int affectedRows = 0;
            try
            {
                using (var connection = new SqlConnection(_options.SqlConnectionString))
                {
                    string query = "SELECT * FROM Accounts WHERE Username = @Username AND AuthorizationLevel = @AuthorizationLevel";
                    IAccount dbAccount = await connection.QueryFirstAsync<Account>(new CommandDefinition(
                        query, 
                        new{
                            Username = otpClaim.Username,
                            AuthorizationLevel = otpClaim.AuthorizationLevel
                        }, 
                        cancellationToken: cancellationToken
                    )).ConfigureAwait(false);
                    // Just make the decision, multiple calls or leak abstraction
                    // Do as stored procedure, do all this work in the database, one request and
                    // encapsulated, and faster - requires database to have concept of stored procedure
                    // Would need to make a decision again if not using a relational DB

                    // Service layer responsibility - interpret return of DAO
                    // Service that Creates User - generic logic to be reusable
                    //  DAO says "row added", Service says "User created"

                    if (dbAccount.Confirmed == false)
                    {
                        results.Add(_messageBank.ErrorMessages["notConfirmed"]);
                        return results;
                    }
                    if (dbAccount.AccountStatus == false)
                    {
                        results.Add(_messageBank.ErrorMessages["notFoundOrEnabled"]);
                        return results;
                    }
                    query = "SELECT * FROM OTPClaims WHERE Username = @Username AND AuthorizationLevel = @AuthorizationLevel";
                    IOTPClaim dbOTPClaim = await connection.QueryFirstAsync<OTPClaim>(new CommandDefinition(
                        query, 
                        new{
                            Username = otpClaim.Username,
                            AuthorizationLevel = otpClaim.AuthorizationLevel
                        }, 
                        cancellationToken: cancellationToken
                    )).ConfigureAwait(false);
                    if (dbOTPClaim == null)
                    {
                        results.Add(_messageBank.ErrorMessages["accountNotFound"]);
                        return results;
                    }
                    if (!otpClaim.OTP.Equals(dbOTPClaim.OTP))
                    {
                        int failCount = ++dbOTPClaim.FailCount;
                        if (failCount >= 5)
                        {
                            query = "UPDATE Accounts SET AccountStatus = 0 WHERE " +
                            "Username = @Username AND AuthorizationLevel = @AuthorizationLevel";
                            affectedRows = await connection.ExecuteAsync(new CommandDefinition(
                                query, 
                                new{
                                    Username = otpClaim.Username,
                                    AuthorizationLevel = otpClaim.AuthorizationLevel,
                                }, 
                                cancellationToken: cancellationToken
                            )).ConfigureAwait(false);
                            if (affectedRows != 1)
                            {
                                results.Add(_messageBank.ErrorMessages["accountDisableFail"]);
                                return results;
                            }
                            else
                            {
                                results.Add(_messageBank.ErrorMessages["tooManyFails"]);
                                return results;
                            }
                        }
                        query = "UPDATE OTPClaims SET FailCount = @FailCount WHERE " +
                        "Username = @Username AND AuthorizationLevel = @AuthorizationLevel";
                        affectedRows = await connection.ExecuteAsync(new CommandDefinition(
                            query, 
                            new{
                                Username = otpClaim.Username,
                                AuthorizationLevel = otpClaim.AuthorizationLevel,
                                FailCount = dbOTPClaim.FailCount++
                            }, 
                            cancellationToken: cancellationToken
                        )).ConfigureAwait(false);
                        if (affectedRows != 1)
                        {
                            results.Add(_messageBank.ErrorMessages["accountNotFound"]);
                            return results;
                        }
                        else
                        {
                            results.Add(_messageBank.ErrorMessages["badNameOrOTP"]);
                            return results;
                        }
                    }
                    // This should be done at service level
                    // (DAO only does a SQL operation then returns results)
                    // If not related to DB, know it should not be in DAO
                    else if ((otpClaim.TimeCreated >= dbOTPClaim.TimeCreated) && (otpClaim.TimeCreated <= dbOTPClaim.TimeCreated.AddMinutes(2)))
                    {
                        results.Add(_messageBank.SuccessMessages["generic"]);
                        results.Add($"username:{otpClaim.Username},authorizationLevel:{otpClaim.AuthorizationLevel}");
                        return results;
                    }
                    else
                    {
                        results.Add(_messageBank.ErrorMessages["otpExpired"]);
                        return results;
                    }
                }
            }
            catch (OTPClaimCreationFailedException occfe)
            {
                results.Add(("400: Server: " + occfe.Message));
                return results;
            }
            catch (InvalidOperationException ioe)
            {
                results.Add(_messageBank.ErrorMessages["notFoundOrEnabled"]);
                return results;
            }
            catch (Exception ex)
            {
                results.Add("500: Database: " + ex.Message);
                return results;
            }
        }
        public async Task<string> VerifyAuthorizedAsync(string requiredAuthLevel, 
            CancellationToken cancellationToken = default)
        {
            // In catch block in above layer, need to roll back if cancel requested
            // check token before operation, if cancelled then dont do op
            // check token after operation, if cancelled, then rollback (closer to DAO, easier it is to rollback)
            // design decision to rollback or not
            // if cancel at end, designate as either cancelled or undo situation

            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                string userAuthLevel = Thread.CurrentPrincipal.IsInRole("admin") ? "admin" : "user";
                using (var connection = new SqlConnection(_options.SqlConnectionString))
                {
                    string query = "SELECT * FROM Accounts WHERE Username = @Username AND AuthorizationLevel = @AuthorizationLevel";
                    IAccount dbAccount = await connection.QueryFirstAsync<Account>(new CommandDefinition(
                        query, 
                        new{
                            Username = Thread.CurrentPrincipal.Identity.Name,
                            AuthorizationLevel = userAuthLevel
                        }, 
                        cancellationToken: cancellationToken
                    )).ConfigureAwait(false);
                    if (dbAccount.Confirmed == false)
                    {
                        return _messageBank.ErrorMessages["notConfirmed"];
                    }
                    else if (dbAccount.AccountStatus == false)
                    {
                        return _messageBank.ErrorMessages["notFoundOrEnabled"];
                    }
                    else
                    {
                        if (dbAccount.AuthorizationLevel.Equals("admin") ||
                            dbAccount.AuthorizationLevel.Equals(requiredAuthLevel))
                        {
                            return _messageBank.SuccessMessages["generic"];
                        }
                        else
                        {
                            return _messageBank.ErrorMessages["notFoundOrAuthorized"];
                        }
                    }
                }
            }
            catch (AccountCreationFailedException acfe)
            {
                return "400: Server: " + acfe.Message;
                //return acfe.Message;
            }
            catch (InvalidOperationException ioe)
            {
                return _messageBank.ErrorMessages["notFoundOrAuthorized"];
            }
            catch (Exception ex)
            {
                return "500: Database: " + ex.Message;
            }
        }

        public async Task<string> StoreOTPAsync(IOTPClaim otpClaim, 
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                using (var connection = new SqlConnection(_options.SqlConnectionString))
                {
                    string query = "SELECT * FROM OTPClaims WHERE Username = @Username AND AuthorizationLevel = @AuthorizationLevel";
                    IOTPClaim dbOTPClaim = await connection.QueryFirstAsync<OTPClaim>(new CommandDefinition(
                        query, 
                        new{
                            Username = otpClaim.Username,
                            AuthorizationLevel = otpClaim.AuthorizationLevel
                        }, 
                        cancellationToken: cancellationToken
                    )).ConfigureAwait(false);
                    if (dbOTPClaim == null)
                    {
                        return _messageBank.ErrorMessages["notFound"];
                    }
                    else
                    {
                        int failCount = dbOTPClaim.FailCount;
                        if (otpClaim.TimeCreated > dbOTPClaim.TimeCreated.AddDays(1))
                        {
                            failCount = 0;
                        }
                        query = "UPDATE OTPClaims SET OTP = @OTP,TimeCreated = @TimeCreated, " +
                        "FailCount = @FailCount WHERE Username = @Username AND AuthorizationLevel = @AuthorizationLevel";
                        var affectedRows = await connection.ExecuteAsync(new CommandDefinition(
                            query, 
                            new{
                                Username = otpClaim.Username,
                                AuthorizationLevel = otpClaim.AuthorizationLevel,
                                OTP = otpClaim.OTP,
                                TimeCreated = otpClaim.TimeCreated,
                                FailCount = otpClaim.FailCount
                            }, 
                            cancellationToken: cancellationToken
                        )).ConfigureAwait(false);
                        if (affectedRows != 1)
                        {
                            return _messageBank.ErrorMessages["otpFail"];
                        }
                        else
                        {
                            return _messageBank.SuccessMessages["generic"];
                        }
                    }
                }
            }
            catch (OTPClaimCreationFailedException occfe)
            {
                return "400: Server: " + occfe.Message;
                //return occfe.Message;
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