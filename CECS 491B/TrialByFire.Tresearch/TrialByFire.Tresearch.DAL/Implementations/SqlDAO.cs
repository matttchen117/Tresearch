using Dapper;
using System.Linq;
using Microsoft.Extensions.Options;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Exceptions;
using TrialByFire.Tresearch.Models;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using System.Text;

namespace TrialByFire.Tresearch.DAL.Implementations
{
    /// <summary>
    ///     SqlDAO: Data Access Object abstraction to interact with Sql (relational) databases
    /// </summary>
    public class SqlDAO : ISqlDAO
    {
        private BuildSettingsOptions _options { get; }
        private IMessageBank _messageBank;

        public SqlDAO(IMessageBank messageBank, IOptionsSnapshot<BuildSettingsOptions> options)
        {
            _messageBank = messageBank;
            _options = options.Value;
        }

        /// <summary>
        ///     SearchForNodeAsync():
        ///         Returns a IResponse of IEnumerable<Node> of all nodes related to the Search provided by ISearchInput
        /// </summary>
        /// <param name="searchInput">Custom input object that contains relevant information for methods related to Search</param>
        /// <returns>Response that contains the results of querying the database</returns>
        public async Task<IResponse<string>> UpdateNodeContentAsync(INodeContentInput nodeContentInput)
        {
            // Check if search input null
            if (nodeContentInput == null)
            {
                return new NodeContentResponse<string>(await _messageBank.GetMessage(
                    IMessageBank.Responses.noSearchInput).ConfigureAwait(false), null, 400, false);
            }
            try
            {
                nodeContentInput.CancellationToken.ThrowIfCancellationRequested();
                using (var connection = new SqlConnection(_options.SqlConnectionString))
                {
                    var procedure = "[UpdateNodeContent]";
                    var parameters = new DynamicParameters();
                    parameters.Add("NodeID", nodeContentInput.NodeID);
                    parameters.Add("NodeTitle", nodeContentInput.NodeTitle);
                    parameters.Add("Summary", nodeContentInput.Summary);
                    parameters.Add("Result", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    var result = await connection.ExecuteAsync(procedure, parameters, commandType: CommandType.StoredProcedure);
                    return new NodeContentResponse<string>("", parameters.Get<int>("Result").ToString(), 200, true);
                }
            }
            catch (Exception ex)
            {
                return new NodeContentResponse<string>(await _messageBank.GetMessage(
                    IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message, null, 500, false);
            }
        }

        /// <summary>
        ///     SearchForNodeAsync():
        ///         Returns a IResponse of IEnumerable<Node> of all nodes related to the Search provided by ISearchInput
        /// </summary>
        /// <param name="searchInput">Custom input object that contains relevant information for methods related to Search</param>
        /// <returns>Response that contains the results of querying the database</returns>
        public async Task<IResponse<IEnumerable<Node>>> SearchForNodeAsync(ISearchInput searchInput)
        {
            // Check if search input null
            if (searchInput == null)
            {
                return new SearchResponse<IEnumerable<Node>>(await _messageBank.GetMessage(
                    IMessageBank.Responses.noSearchInput).ConfigureAwait(false), null, 400, false);
            }
            try
            {
                searchInput.CancellationToken.ThrowIfCancellationRequested();
                using (var connection = new SqlConnection(_options.SqlConnectionString))
                {
                    var procedure = "[SearchNodes]";
                    var parameters = new DynamicParameters();
                    parameters.Add("Search", searchInput.Search);
                    // Utilizing Dapper Multi-Relationship capabilities to associate relevant data to Node's
                    var nodes = await connection.QueryAsync<Node, NodeTag, int, Node>(procedure, (node, tag, rating) =>
                    {
                        if (!(tag is null))
                        {
                            tag.NodeID = node.NodeID;
                            node.Tags.Add(tag);
                        }
                        node.RatingScore = rating;
                        return node;
                    },
                    parameters,
                    commandType: CommandType.StoredProcedure,
                    splitOn: "TagName, Rating");
                    // Group query results since it comes back as one row per relation
                    // (Grouping all tags of a node into a single Node's Tags property)
                    var results = nodes.GroupBy(n => n.NodeID).ToList().Select(g =>
                    {
                        var groupedNode = g.FirstOrDefault();
                        // Verify there is a node
                        if(groupedNode != null)
                        {
                            groupedNode.Tags = g.Select(n => n.Tags.SingleOrDefault()).Where(nt => nt != null).ToList();
                        }
                        return groupedNode;
                    }).ToList();
                    return new SearchResponse<IEnumerable<Node>>("", results, 200, true);
                }
            }
            catch (Exception ex)
            {
                return new SearchResponse<IEnumerable<Node>>(await _messageBank.GetMessage(
                    IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message, null, 500, false);
            }
        }

        public async Task<IResponse<string>> EditParentNodeAsync(long nodeID, string nodeIDs, CancellationToken cancellationToken = default)
        {
            if(nodeIDs != null)
            {
                try
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    using(var connection = new SqlConnection(_options.SqlConnectionString))
                    {
                        var procedure = "[EditParents]";
                        var parameters = new
                        {
                            NodeID = nodeID,
                            NodeIDs = nodeIDs
                        };
                        var result = await connection.QueryAsync(new CommandDefinition(procedure, parameters, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken)).ConfigureAwait(false);
                        return new EditParentResponse<string>("", 
                            await _messageBank.GetMessage(IMessageBank.Responses.editParentSuccess).ConfigureAwait(false), 200, true);
                    }
                }
                catch(SqlException ex)
                {
                    switch (ex.Number)
                    {
                        // Unable to connect to database
                        case -1:
                            return new EditParentResponse<string>(await _messageBank.GetMessage(
                                IMessageBank.Responses.databaseConnectionFail).ConfigureAwait(false), null, 400, false);
                        default:
                            return new EditParentResponse<string>("500: Database: " + ex.Message, null, 400, false);
                    }
                }
                catch(Exception ex)
                {
                    return new EditParentResponse<string>(await _messageBank.GetMessage(
                        IMessageBank.Responses.unhandledException).ConfigureAwait(false), null, 400, false);
                }
            }
            else
            {
                return new EditParentResponse<string>(await _messageBank.GetMessage(
                    IMessageBank.Responses.noEditParentNodeInput).ConfigureAwait(false), null, 400, false);
            }
        }

        public async Task<string> RemoveUserIdentityFromHashTable(string email, string authorizationLevel, string hashedEmail, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                using (var connection = new SqlConnection(_options.SqlConnectionString))
                {
                    //Perform sql statement
                    var procedure = "dbo.[RemoveHashIdentity]";                                                                 // Name of store procedure
                    var value = new { UserHash = hashedEmail };                                                                 // Parameters of stored procedure
                    int affectedRows = await connection.ExecuteAsync(new CommandDefinition(procedure, value, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken)).ConfigureAwait(false);

                    //Check if cancellation is requested ... remove user identity from hash table
                    if (cancellationToken.IsCancellationRequested)
                    {
                        
                    }

                    return await _messageBank.GetMessage(IMessageBank.Responses.generic);
                }
            }
            catch (SqlException ex)
            {
                switch (ex.Number)
                {
                    case 2627:
                        return await _messageBank.GetMessage(IMessageBank.Responses.accountAlreadyCreated);
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
                return await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message;
            }
        }

        public async Task<string> CreateUserHashAsync(int id, string hashedEmail, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                using (var connection = new SqlConnection(_options.SqlConnectionString))
                {
                    //Perform sql statement
                    var procedure = "dbo.[CreateUserHash]";                                                                 // Name of store procedure
                    var value = new { UserID = id, UserHash = hashedEmail };          // Parameters of stored procedure
                    int affectedRows = await connection.ExecuteAsync(new CommandDefinition(procedure, value, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken)).ConfigureAwait(false);

                    //Check if cancellation is requested ... remove user identity from hash table
                    if (cancellationToken.IsCancellationRequested)
                    {
                        
                    }

                    return await _messageBank.GetMessage(IMessageBank.Responses.generic);
                }
            }
            catch (SqlException ex)
            {
                switch (ex.Number)
                {
                    case 2627:
                        return await _messageBank.GetMessage(IMessageBank.Responses.accountAlreadyCreated);
                    case 547:   
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
                return await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message;
            }
        }
        
        public async Task<string> GetUserHashAsync(IAccount account, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var connection = new SqlConnection(_options.SqlConnectionString))
            {
                //Perform sql statement
                var procedure = "dbo.[GetUserHash]";
                var parameters = new DynamicParameters();
                parameters.Add("Username", account.Username);
                parameters.Add("AuthorizationLevel", account.AuthorizationLevel);
                parameters.Add("Result", dbType: DbType.AnsiString, size: 128, direction: ParameterDirection.Output);
                var result = await connection.ExecuteAsync(new CommandDefinition(procedure, parameters,
                    commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken))
                    .ConfigureAwait(false);
                return parameters.Get<string>("Result");
            }
        }

        public async Task<int> StoreLogAsync(ILog log, string destination, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using(var connection = new SqlConnection(_options.SqlConnectionString))
            {
                var procedure = "[StoreLog]";
                var parameters = new DynamicParameters();
                parameters.Add("Timestamp", log.Timestamp);
                parameters.Add("Level", log.Level);
                parameters.Add("UserHash", log.UserHash);
                parameters.Add("Category", log.Category);
                parameters.Add("Description", log.Description);
                parameters.Add("Hash", log.Hash);
                parameters.Add("Destination", destination);
                parameters.Add("Result", dbType: DbType.Int32, direction: ParameterDirection.Output);
                var result = await connection.ExecuteAsync(new CommandDefinition(procedure, parameters,
                    commandType: CommandType.StoredProcedure))
                    .ConfigureAwait(false);
                return parameters.Get<int>("Result");
            }
        }


        /// <summary>
        ///     DisableAccountAsync()
        ///         Disables accounts account passed in asynchrnously.
        /// </summary>
        /// <param name="account">UserAccount to disable</param>
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
        /// <param name="account">UserAccount to enable</param>
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

                    var Accounts = await connection.QueryAsync<UserAccount>(new CommandDefinition(procedure, parameters, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken)).ConfigureAwait(false);

                    //Check if account was returned
                    if (Accounts.Count() < 1)
                        return Tuple.Create(nullAccount, _messageBank.GetMessage(IMessageBank.Responses.accountNotFound).Result);            //UserAccount doesn't exist

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
                    var affectedRows = await connection.ExecuteAsync(new CommandDefinition(procedure, value, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken)).ConfigureAwait(false);
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
                    case 2627: return _messageBank.GetMessage(IMessageBank.Responses.confirmationLinkExists).Result;
                    case 547: return _messageBank.GetMessage(IMessageBank.Responses.accountNotFound).Result;    //Foreign key constraint  AKA NO ACCOUNT EXISTS
                    default: return "500: Database: " + ex.Number;
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
                    IConfirmationLink link = await connection.QuerySingleOrDefaultAsync<ConfirmationLink>(new CommandDefinition(procedure, value, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken)).ConfigureAwait(false);
                    if (cancellationToken.IsCancellationRequested)
                        throw new OperationCanceledException();
                    if(link == null)
                        return Tuple.Create(link, _messageBank.GetMessage(IMessageBank.Responses.confirmationLinkNotFound).Result);
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
                return Tuple.Create(nullLink, "500: Database: " +  ex.Message);
            }

        }

        public async Task<string> UpdateAccountToUnconfirmedAsync(string username, string authorizationlevel, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                using (var connection = new SqlConnection(_options.SqlConnectionString))
                {
                    var procedure = "[UnconfirmAccount]";      //INSERT on duplicate key update linkscreted++
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

                    if (affectedRows < 1)
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

                    if (affectedRows < 1)
                        return _messageBank.GetMessage(IMessageBank.Responses.accountNotFound).Result;
                    else
                        return _messageBank.GetMessage(IMessageBank.Responses.generic).Result;
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
                    var affectedRows = await connection.ExecuteScalarAsync<int>(new CommandDefinition(procedure, value, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken)).ConfigureAwait(false);

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

        public async Task<string> CreateOTPAsync(string username, string authorizationLevel, int failCount, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                using (var connection = new SqlConnection(_options.SqlConnectionString))
                {
                    await connection.OpenAsync();

                    //Perform Sql Statement
                    var procedure = "dbo.[CreateOTP]";
                    var value = new
                    {
                        Username = username, 
                        AuthorizationLevel = authorizationLevel,
                        FailCount = failCount
                    };
                    var affectedRows = await connection.ExecuteAsync(new CommandDefinition(procedure, value, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken)).ConfigureAwait(false);
                    
                    if (affectedRows == 1)
                        return _messageBank.GetMessage(IMessageBank.Responses.generic).Result;
                    else
                        return _messageBank.GetMessage(IMessageBank.Responses.accountAlreadyCreated).Result;    //CHECK IF THIS IS ACCOUNT ALREADY EXISTS
                }
            }
            catch (SqlException ex)
            {
                switch (ex.Number)
                {
                    //Unable to connect to database
                    case -1:
                        return await _messageBank.GetMessage(IMessageBank.Responses.databaseConnectionFail);
                    //Adding otp violates foreign key constraint (AKA account doesn't exist in bank)
                    case 547:
                        return _messageBank.GetMessage(IMessageBank.Responses.accountNotFound).Result;
                    case 2627: 
                        return _messageBank.GetMessage(IMessageBank.Responses.accountAlreadyCreated).Result;
                    default: 
                        return _options.UnhandledExceptionMessage + ex.Message;
                }
            }
            catch (OperationCanceledException)
            {
                // Rollback handled
                throw;
            }
            catch (Exception ex)
            {
                return "500 : Database: " + ex;
            }
        }
        public async Task<Tuple<int, string>> CreateAccountAsync(IAccount account, CancellationToken cancellationToken = default(CancellationToken))
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
                        Passphrase = account.Passphrase,
                        AuthorizationLevel = account.AuthorizationLevel,
                        AccountStatus = account.AccountStatus,
                        Confirmed = account.Confirmed
                    };

                    var userID = await connection.ExecuteScalarAsync<int>(new CommandDefinition(procedure, value, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken)).ConfigureAwait(false);

                    return Tuple.Create(userID, await _messageBank.GetMessage(IMessageBank.Responses.generic));
                }
            }
            catch(SqlException ex)
            {
                switch (ex.Number)
                {
                    case 2627:
                        return Tuple.Create(-1, await _messageBank.GetMessage(IMessageBank.Responses.accountAlreadyCreated));
                    default: 
                        return Tuple.Create(-1, await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) +  ex.Message);
                }
            }
            catch(OperationCanceledException)
            {
                // Rollback handled
                throw;
            }
            catch (Exception ex)
            {
                return Tuple.Create(-1, await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message);
            }
        }



        /// <summary>
        /// Get amount of admins method that runs stored procedure to see if there are enough admins left to delete an admin account
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>Message indicating if there are enough admins left</returns>

        public async Task<string> GetAmountOfAdminsAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            int affectedRows = 0;
            try
            {

                cancellationToken.ThrowIfCancellationRequested();

                if (Thread.CurrentPrincipal.Equals(null))
                {
                    return await _messageBank.GetMessage(IMessageBank.Responses.notAuthorized).ConfigureAwait(false);
                }



                using (var connection = new SqlConnection(_options.SqlConnectionString))
                {



                    var procedure = "dbo.[GetAmountOfAdmins]";
                    affectedRows = await connection.ExecuteScalarAsync<int>(new CommandDefinition(procedure, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken)).ConfigureAwait(false);
                    
                    Console.WriteLine(affectedRows);
                    //changed up logic for affectedRows, if there is more than 1 row
                    //then this should be valid.
                    
                    if (affectedRows > 1)
                    {
                        return await _messageBank.GetMessage(IMessageBank.Responses.getAdminsSuccess).ConfigureAwait(false);
                    }
                    else
                    {
                        return await _messageBank.GetMessage(IMessageBank.Responses.lastAdminFail).ConfigureAwait(false);
                    }
                }
            }


            //be clear in message
            catch (OperationCanceledException)
            {
                return await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested).ConfigureAwait(false);
            }

            catch (Exception ex)
            {
                return ("500: Database " + ex.Message);

            }

        }


        /// <summary>
        /// DAO method to delete account and all user identifying information
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>message stating if delete was successful</returns>


        public async Task<string> DeleteAccountAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            int affectedRows;


            try
            {
                cancellationToken.ThrowIfCancellationRequested();                                                      

                if (cancellationToken.IsCancellationRequested)
                {
                    return await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested).ConfigureAwait(false);
                }

                string userAuthLevel = Thread.CurrentPrincipal.IsInRole("admin") ? "admin" : "user";
                string userName = Thread.CurrentPrincipal.Identity.Name;


                using (var connection = new SqlConnection(_options.SqlConnectionString))
                {
                    var parameters = new { Username = userName, AuthorizationLevel = userAuthLevel };
                    var procedure = "dbo.[DeleteAccountStoredProcedure]";
                    affectedRows = await connection.ExecuteScalarAsync<int>(new CommandDefinition(procedure, parameters, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken)).ConfigureAwait(false);
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested).ConfigureAwait(false);
                    }
                    if(affectedRows == 0)
                    {
                        return await _messageBank.GetMessage(IMessageBank.Responses.accountDeletionSuccess).ConfigureAwait(false);
                    }
                    else
                    {
                        return await _messageBank.GetMessage(IMessageBank.Responses.accountDeleteFail).ConfigureAwait(false);
                    }
                }

            }
            catch (OperationCanceledException)
            {
                return await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return ("500: Database " + ex.Message);
            }
        }

        public async Task<string> DeleteAccountAsync(IAccount account, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                using (var connection = new SqlConnection(_options.SqlConnectionString))
                {
                    var parameters = new { Username = account.Username, AuthorizationLevel = account.AuthorizationLevel };
                    var procedure = "dbo.[DeleteAccountStoredProcedure]";
                    int affectedRows = await connection.ExecuteScalarAsync<int>(new CommandDefinition(procedure, parameters, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken)).ConfigureAwait(false);
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested).ConfigureAwait(false);
                    }
                    if (affectedRows == 0)
                    {
                        return await _messageBank.GetMessage(IMessageBank.Responses.accountDeletionSuccess).ConfigureAwait(false);
                    }
                    else
                    {
                        return await _messageBank.GetMessage(IMessageBank.Responses.accountDeleteFail).ConfigureAwait(false);
                    }
                }

            }
            catch (OperationCanceledException)
            {
                return await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return ("500: Database " + ex.Message);
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

        public async Task<int> AuthenticateAsync(IAuthenticationInput authenticationInput,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var connection = new SqlConnection(_options.SqlConnectionString))
            {
                //Perform sql statement
                var procedure = "[Authenticate]";
                var parameters = new DynamicParameters();
                parameters.Add("Username", authenticationInput.OTPClaim.Username);
                parameters.Add("OTP", authenticationInput.OTPClaim.OTP);
                parameters.Add("AuthorizationLevel", authenticationInput.OTPClaim.AuthorizationLevel);
                parameters.Add("TimeCreated", authenticationInput.OTPClaim.TimeCreated);
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
        /// <summary>
        ///         Adds a tag to list of node(s) passed in. 
        /// </summary>
        /// <param name="nodeIDs">List of node IDs</param>
        /// <param name="tagName">String tag name</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>String status result</returns>
        public async Task<string> AddTagAsync(List<long> nodeIDs, string tagName, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                // Throw Cancellation Exception if token requests cancellation
                cancellationToken.ThrowIfCancellationRequested();

                // Check if tag is null or empty
                if (tagName == null || tagName.Equals("") || tagName.Trim().Equals(""))
                {
                    return await _messageBank.GetMessage(IMessageBank.Responses.tagNameInvalid).ConfigureAwait(false);
                }
                // Check if node list is null or empty
                if (nodeIDs == null || nodeIDs.Count() <= 0)
                {
                    return await _messageBank.GetMessage(IMessageBank.Responses.nodeNotFound).ConfigureAwait(false);
                }

                // Establish connection to database
                using (var connection = new SqlConnection(_options.SqlConnectionString))
                {
                    // Open connection
                    await connection.OpenAsync();
                    // Iterate through each node and add tag
                    foreach(var nodeId in nodeIDs)
                    {
                        // Set up command
                        var procedure = "dbo.[AddTagToNode]";
                        var parameters = new
                        {
                            NodeID = nodeId,
                            TagName = tagName
                        };
                        // Execute command
                        var executed = await connection.ExecuteAsync(new CommandDefinition(procedure, parameters, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken)).ConfigureAwait(false);
                    }

                    // Tag(s) has been added, return success
                    return await _messageBank.GetMessage(IMessageBank.Responses.tagAddSuccess).ConfigureAwait(false);
                }
            }
            catch (SqlException ex)
            {
                // Check sql exception
                switch (ex.Number)
                {
                    // Unable to connect to database
                    case -1:
                        return await _messageBank.GetMessage(IMessageBank.Responses.databaseConnectionFail).ConfigureAwait(false);
                    // Adding tag to node violates foreign key constraint (AKA tag doesn't exist in bank)
                    case 547:   
                        return await _messageBank.GetMessage(IMessageBank.Responses.tagNotFound).ConfigureAwait(false);
                    default: 
                        return await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message;
                }
            }
            catch (OperationCanceledException)
            {
                return await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message;
            }
        }

        /// <summary>
        ///         Removes tag from list of nodes
        /// </summary>
        /// <param name="nodeIDs">List of node IDs</param>
        /// <param name="tagName">String tag name</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>String status result</returns>
        public async Task<string> RemoveTagAsync(List<long> nodeIDs, string tagName, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                // Throw Cancellation Exception if token requests cancellation
                cancellationToken.ThrowIfCancellationRequested();

                // Check if tag name is null, empty string or all space
                if (tagName == null || tagName.Equals("") || tagName.Trim().Equals(""))
                {
                    return await _messageBank.GetMessage(IMessageBank.Responses.tagNameInvalid).ConfigureAwait(false);
                }
                    
                // Check if node list is null or empty
                if (nodeIDs == null || nodeIDs.Count() <= 0)
                {
                    return await _messageBank.GetMessage(IMessageBank.Responses.nodeNotFound).ConfigureAwait(false);
                }

                // Establish connection to database
                using (var connection = new SqlConnection(_options.SqlConnectionString))
                {
                    // Open connection to database
                    await connection.OpenAsync();
                    // Iterate through each tag and remoe tag from node
                    foreach (var nodeId in nodeIDs)
                    {
                        // Set up command
                        var procedure = "dbo.[RemoveTagFromNode]";
                        var value = new
                        {
                            NodeID = nodeId,
                            TagName = tagName
                        };
                        // Execute command
                        var execute = await connection.ExecuteAsync(new CommandDefinition(procedure, value, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken)).ConfigureAwait(false);
                    }

                    // Tag has been removed, return success message
                    return await _messageBank.GetMessage(IMessageBank.Responses.tagRemoveSuccess);
                }
            }
            catch (SqlException ex)
            {
                switch (ex.Number)
                {
                    case -1:
                        return await _messageBank.GetMessage(IMessageBank.Responses.databaseConnectionFail);
                    default: 
                        return await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message;
                }
            }
            catch (OperationCanceledException)
            {
                return await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested);
            }
            catch (Exception ex)
            {
                return await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message;
            }
        }

        /// <summary>
        ///     Retreives a list of shared tags from a list of node(s). List with single node will return all tags.
        /// </summary>
        /// <param name="nodeIDs">List of node IDs</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>List of tags and string status result</returns>
        public async Task<Tuple<List<string>, string>> GetNodeTagsAsync(List<long> nodeIDs, CancellationToken cancellationToken = default(CancellationToken))
        {
           
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                
                // Check if node list is null or empty
                if (nodeIDs == null || nodeIDs.Count() <= 0)
                {
                    return Tuple.Create(new List<string>(), await _messageBank.GetMessage(IMessageBank.Responses.nodeNotFound).ConfigureAwait(false));
                }
                    

                // Establish connection to database
                using (var connection = new SqlConnection(_options.SqlConnectionString))
                {
                    // Open connection to database
                    await connection.OpenAsync();
                    
                    List<string> tags = new List<string>();         //List of tags that all node(s) share in common
                    
                    // Iterate through each node to get list of tags for each node then intersect
                    foreach (var nodeId in nodeIDs)
                    {
                        // Set up command
                        var procedure = "dbo.[GetNodeTags]";
                        var value = new
                        {
                            NodeID = nodeId
                        };
                        // Execute command: Get tags for current node
                        List<string> results = new List<string>(await connection.QueryAsync<string>
                                (new CommandDefinition(procedure, value, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken)).ConfigureAwait(false)).ToList();
                        
                        // Checks if node did not exist
                        if (results.Contains("-1 invalid"))
                        {
                            return Tuple.Create(new List<string>(), await _messageBank.GetMessage(IMessageBank.Responses.nodeNotFound));
                        }

                        // Checks if first node in list
                        if (nodeId == nodeIDs.First())
                        {
                            // Set first tag list as base list
                            tags = results;
                        }

                        // Get tags that are shared between all nodes
                        tags = tags.Intersect(results).ToList();
                    } 
                    // Tags have been retrieved, return success message

                    return Tuple.Create(tags, await _messageBank.GetMessage(IMessageBank.Responses.tagGetSuccess));
                }
            }
            catch (SqlException ex)
            {
                switch (ex.Number)
                {
                    // Unable to connect to database
                    case -1:
                        return Tuple.Create(new List<string>(), await _messageBank.GetMessage(IMessageBank.Responses.databaseConnectionFail));
                    default: 
                        return Tuple.Create(new List<string>(), await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message);
                }
            }
            catch (OperationCanceledException)
            {
                return Tuple.Create(new List<string>(), await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested));
            }
            catch (Exception ex)
            {
                return Tuple.Create(new List<string>(), await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message);
            }
        }

        /// <summary>
        ///      Creaes a tag in bank with a count of tags
        /// </summary>
        /// <param name="tagName">Tag name</param>
        /// <param name="count">Number of nodes tagged</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>String status result</returns>
        public async Task<string> CreateTagAsync(string tagName, int count, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                //Check tag input
                if (tagName == null || tagName.Equals("") || tagName.Trim().Equals(""))
                {
                    return await _messageBank.GetMessage(IMessageBank.Responses.tagNameInvalid).ConfigureAwait(false);
                }

                // Check tag count
                if (count < 0)
                {
                    return await _messageBank.GetMessage(IMessageBank.Responses.tagCountInvalid).ConfigureAwait(false);
                }     
                
                using (var connection = new SqlConnection(_options.SqlConnectionString))
                {
                    await connection.OpenAsync();

                    //Set up Statement
                    var procedure = "dbo.[CreateTag]";
                    var value = new
                    {
                        TagName = tagName,
                        TagCount = count
                    };

                    //Execute statement
                    var execute = await connection.ExecuteAsync(new CommandDefinition(procedure, value, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken)).ConfigureAwait(false);

                    //Tag created, return success
                    return await _messageBank.GetMessage(IMessageBank.Responses.tagCreateSuccess).ConfigureAwait(false);
                }
            }
            catch (SqlException ex)
            {
                switch (ex.Number)
                {
                    //Unable to connect to database
                    case -1:
                        return await _messageBank.GetMessage(IMessageBank.Responses.databaseConnectionFail).ConfigureAwait(false);
                    //Adding tag violates primary key constraint (AKA tag already exists in database)
                    case 2627: 
                        return await _messageBank.GetMessage(IMessageBank.Responses.tagDuplicate).ConfigureAwait(false);
                    default: 
                        return await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message;
                }
            }
            catch (OperationCanceledException)
            {
                // Rollback handled
                return await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message;
            }
        }

        /// <summary>
        ///     DeleteTagAsync(tagName)
        ///         Deletes tag from tag bank
        /// </summary>
        /// <param name="tagName">String tag to delete from bank</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>String status result</returns>
        public async Task<string> RemoveTagAsync(string tagName, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                // Throw if cancellation requested
                cancellationToken.ThrowIfCancellationRequested();

                // Check input
                if (tagName == null || tagName.Equals("") || tagName.Trim().Equals(""))
                {
                    return await _messageBank.GetMessage(IMessageBank.Responses.tagNameInvalid).ConfigureAwait(false);
                }
                    

                using (var connection = new SqlConnection(_options.SqlConnectionString))
                {
                    await connection.OpenAsync();

                    //create command
                    var procedure = "dbo.[RemoveTag]";
                    var value = new
                    {
                        TagName = tagName
                    };
                    //Execute command, returns count of tags
                    int count = await connection.ExecuteScalarAsync<int>(new CommandDefinition(procedure, value, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken)).ConfigureAwait(false);
                    //Tag deleted, return success
                    return await _messageBank.GetMessage(IMessageBank.Responses.tagDeleteSuccess).ConfigureAwait(false);
                }
            }
            catch (SqlException ex)
            {
                switch (ex.Number)
                {
                    //Unable to connect to database
                    case -1:
                        return await _messageBank.GetMessage(IMessageBank.Responses.databaseConnectionFail).ConfigureAwait(false);
                    default: 
                        return await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message;
                }
            }
            catch (OperationCanceledException)
            {
                // Rollback handled
                return await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message;
            }
        }

        /// <summary>
        ///     GetTgsAsync()
        ///         Returns a list of tags in tag bank
        /// </summary>
        /// <param name="cancellationToken">Cancellation  Token</param>
        /// <returns>List of tags and string status result</returns>
        public async Task<Tuple<List<ITag>, string>> GetTagsAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
           
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                //Establish connection
                using (var connection = new SqlConnection(_options.SqlConnectionString))
                {
                    //Open connection
                    await connection.OpenAsync();
                    //Setup statement
                    var procedure = "dbo.[GetTags]";
                    var value = new { };
                    //Execute statement
                    List<ITag> results = new List<ITag>(await connection.QueryAsync<Tag>(new CommandDefinition(procedure, value, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken)).ConfigureAwait(false)).ToList();
                    //tags retrieved, return success
                    return Tuple.Create(results, await _messageBank.GetMessage(IMessageBank.Responses.tagGetSuccess).ConfigureAwait(false));
                }
            }
            catch (SqlException ex)
            {
                switch (ex.Number)
                {
                    //Unable to connect to database
                    case -1:
                        return Tuple.Create(new List<ITag>(), await _messageBank.GetMessage(IMessageBank.Responses.databaseConnectionFail).ConfigureAwait(false));
                    default: 
                        return Tuple.Create(new List<ITag>(), await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message);
                }
            }
            catch (OperationCanceledException)
            {
                // Rollback handled
                return Tuple.Create(new List<ITag>(), await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested).ConfigureAwait(false));
            }
            catch (Exception ex)
            {
                return Tuple.Create(new List<ITag>(), await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message);
            }
        }

        /// <summary>
        ///     Checks if a user is authorized to make changes to a list of node(s).
        /// </summary>
        /// <param name="nodeIDs">List of node ids</param>
        /// <param name="userHash">Accounts User Hash</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns></returns>
        public async Task<string> IsAuthorizedToMakeNodeChangesAsync(List<long> nodeIDs, string userHash, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                using (var connection = new SqlConnection(_options.SqlConnectionString))
                {
                    // Iterate through each node and check
                    foreach (var nodeId in nodeIDs)
                    {
                        var procedure = "dbo.[IsAuthorizedNodeChanges]";
                        var value = new
                        {
                            NodeID = nodeId,
                            UserHash = userHash
                        };
                        var isAuthorized = await connection.ExecuteScalarAsync<int>(new CommandDefinition(procedure, value, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken)).ConfigureAwait(false);

                        if (isAuthorized == 0)
                            return await _messageBank.GetMessage(IMessageBank.Responses.notAuthorized).ConfigureAwait(false);
                    }
                    return await _messageBank.GetMessage(IMessageBank.Responses.verifySuccess).ConfigureAwait(false);
                }
            }
            catch(OperationCanceledException)
            {
                return await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested).ConfigureAwait(false);
            }
            catch(Exception ex)
            {
                return _options.UnhandledExceptionMessage + ex.Message;
            }
        }

        public async Task<IResponse<string>> CreateNodeAsync(INode node, CancellationToken cancellationToken = default)
        {
            try
            {
                using (var connection = new SqlConnection(_options.SqlConnectionString))
                {
                    var procedure = "dbo.[CreateNode]";
                    var values = new
                    {
                        UserHash = node.UserHash,
                        ParentNodeID = node.ParentNodeID,
                        NodeTitle = node.NodeTitle,
                        Summary = node.Summary,
                        TimeModified = node.TimeModified,
                        Visibility = node.Visibility,
                        //Deleted = false,
                    };
                    int affectedRows = await connection.ExecuteAsync(new CommandDefinition(procedure, values, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken)).ConfigureAwait(false);

                    if(affectedRows == 1)
                    {
                        return new CreateNodeResponse<string>("", await _messageBank.GetMessage(IMessageBank.Responses.createNodeSuccess).ConfigureAwait(false), 200, true);
                    }
                    else
                    {
                        return new CreateNodeResponse<string>(await _messageBank.GetMessage(IMessageBank.Responses.createNodeFail).ConfigureAwait(false), null, 500, false);
                    }
                }
            }
            catch(Exception ex)
            {
                return new CreateNodeResponse<string>(await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message, null, 500, false);
            }
        }

        public async Task<IResponse<string>> DeleteNodeAsync(long nodeID, long parentID, CancellationToken cancellationToken = default)
        {
            try
            {
                int affectedRows;
                cancellationToken.ThrowIfCancellationRequested();
                using (var connection = new SqlConnection(_options.SqlConnectionString))
                {
                    var procedure = "dbo.[DeleteNode]";
                    var parameters = new
                    {
                        NodeID = nodeID,
                        ParentNodeID = parentID
                    };
                    affectedRows = await connection.ExecuteScalarAsync<int>(new CommandDefinition(procedure, parameters, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken)).ConfigureAwait(false);
                    if (affectedRows == 0)
                    {
                        return new DeleteNodeResponse<string>("", await _messageBank.GetMessage(IMessageBank.Responses.deleteNodeSuccess).ConfigureAwait(false), 200, true);
                    }
                    else
                    {
                        return new DeleteNodeResponse<string>(await _messageBank.GetMessage(IMessageBank.Responses.deleteNodeFail).ConfigureAwait(false), null, 500, false);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch(Exception ex)
            {
                return new DeleteNodeResponse<string>(await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message, null, 500, false);
            }
        }

        public async Task<string> UpdateNodeAsync(INode updatedNode, INode previousNode, CancellationToken cancellationToken = default)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                using (var connection = new SqlConnection(_options.SqlConnectionString))
                {
                    var procedure = "[UpdateNode]";
                    var values = new
                    {
                        UserHash = updatedNode.UserHash,
                        NodeID = updatedNode.NodeID,
                        NodeParentID = updatedNode.ParentNodeID,
                        NodeTitle = updatedNode.NodeTitle,
                        Summary = updatedNode.Summary,
                        Visibility = updatedNode.Visibility
                    };

                    var affectedRows = await connection.ExecuteAsync(new CommandDefinition(procedure, values, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken)).ConfigureAwait(false);

                    if (cancellationToken.IsCancellationRequested)
                    {
                        var restoreValues = new
                        {
                            UserHash = previousNode.UserHash,
                            NodeID = previousNode.NodeID,
                            NodeParentID = previousNode.ParentNodeID,
                            NodeTitle = previousNode.NodeTitle,
                            Summary = previousNode.Summary,
                            Visibility = previousNode.Visibility
                        };
                        affectedRows = await connection.ExecuteAsync(new CommandDefinition(procedure, restoreValues, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken)).ConfigureAwait(false);

                        if (affectedRows < 1)
                            return _messageBank.GetMessage(IMessageBank.Responses.rollbackFailed).Result;
                        else
                            throw new OperationCanceledException();
                    }

                    if(affectedRows < 1)
                    {
                        return _messageBank.GetMessage(IMessageBank.Responses.updateNodeFail).Result;
                    }
                    else
                    {
                        return _messageBank.GetMessage(IMessageBank.Responses.generic).Result;
                    }
                }
            }
            catch(OperationCanceledException)
            {
                throw;
            }
            catch(Exception ex)
            {
                return "500: Database: " + ex.Message;
            }
        }

        

        public async Task<Tuple<INode, string>> GetNodeAsync(long nID, CancellationToken cancellationToken = default)
        {
            INode? nullNode = null;
            try
            {


                using (var connection = new SqlConnection(_options.SqlConnectionString))
                {
                    var procedure = "dbo.[GetNode]";
                    var parameters = new
                    {
                        nodeID = nID
                    };
                    var Nodes = await connection.QueryAsync<Node>(new CommandDefinition(procedure, parameters, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken)).ConfigureAwait(false);

                    if (Nodes.Count() == 0)
                    {
                        return Tuple.Create(nullNode, _messageBank.ErrorMessages["nodeNotFound"]);
                    }

                    INode node = Nodes.First();

                    if (cancellationToken.IsCancellationRequested)
                    {
                        return Tuple.Create(nullNode, _messageBank.ErrorMessages["cancellationRequested"]);
                    }
                    else
                    {
                        return Tuple.Create(node, _messageBank.SuccessMessages["generic"]);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                return Tuple.Create(nullNode, _messageBank.ErrorMessages["cancellationRequested"]);
            }
            catch (Exception ex)
            {
                return Tuple.Create(nullNode, "500: Database: " + ex.Message);
            }
        }

        public async Task<Tuple<List<INode>, string>> GetNodesAsync(string userHash, string accountHash, CancellationToken cancellationToken = default)
        {
            List<INode> nullNodes = null;
            try
            {
                using(var connection = new SqlConnection(_options.SqlConnectionString))
                {
                    var procedure = "dbo.[GetNodes]";
                    var parameters = new
                    {
                        UserHash = userHash,
                        AccountHash = accountHash
                    };
                    //List<Node> results = new List<Node>();
                    List<Node> results = new List<Node>(await connection.QueryAsync<Node>(new CommandDefinition(procedure, parameters, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken)).ConfigureAwait(false)).ToList(); 
                    /*foreach(Node n in (new List<Node>(await connection.QueryAsync<Node>(new CommandDefinition(procedure, parameters, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken)).ConfigureAwait(false)).ToList()))
                    {
                        results.Add(new Node(n));
                    }*/
                    if(results.Count == 0)
                    {
                        return Tuple.Create(nullNodes, await _messageBank.GetMessage(IMessageBank.Responses.nodeNotFound));
                    }
                    List<INode> castResults = new List<INode>();
                    foreach(Node n in results)
                    {
                        castResults.Add(n);
                    }

                    //TODO Add MessageBank success log
                    return Tuple.Create(castResults, await _messageBank.GetMessage(IMessageBank.Responses.getNodesSuccess));
                }
            }
            catch(SqlException se)
            {
                return Tuple.Create(nullNodes, se.Message);
            }
            catch (OperationCanceledException)
            {
                return Tuple.Create(nullNodes, await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested));
            }
            catch(Exception ex)
            {
                return Tuple.Create(nullNodes, ex.Message);
            }
        }

        public async Task<Tuple<List<INode>, string>> GetNodeChildren(long nID, CancellationToken cancellationToken = default)
        {
            List<INode> childList = new List<INode>();
            List <INode> nullList = null;
            try
            {
                using (var connection = new SqlConnection(_options.SqlConnectionString))
                {
                    var procedure = "dbo.{GetNodeChildren]";
                    var parameters = new
                    {
                        nodeID = nID
                    };
                    var Nodes = await connection.QueryAsync<Node>(new CommandDefinition(procedure, parameters, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken)).ConfigureAwait(false);

                    if (Nodes.Count() == 0)
                    {
                        return Tuple.Create(childList, _messageBank.ErrorMessages["nodeChildrenNotFound"]);
                    }

                    //Iterate through the list so the list of type INode takes in entries of type Node
                    foreach (var node in Nodes)
                    {
                        childList.Add(node);
                    }
                    return (Tuple.Create(childList, await _messageBank.GetMessage(IMessageBank.Responses.getNodesSuccess)));
                }
            }
            catch(Exception ex)
            {
                return Tuple.Create(nullList, "500: Server: " + ex.Message);
            }
        }

        public async Task<IResponse<int>> RateNodeAsync(List<long> nodeIDs, int rating, string userHash, CancellationToken cancellationToken)
        {
            try
            {
                //Throw Cancellation Exception if token requests cancellation
                cancellationToken.ThrowIfCancellationRequested();

                // Check if node list is null or empty
                if (nodeIDs == null || nodeIDs.Count.Equals(0))
                {
                    return new RateResponse<int>(await _messageBank.GetMessage(IMessageBank.Responses.nodeNotFound), rating, 404, false);
                }

                if (rating <= 0)
                    return new RateResponse<int>(await _messageBank.GetMessage(IMessageBank.Responses.invalidRating), rating, 422, false);

                StringBuilder nodeCSV = new StringBuilder();

                // Create Comma Seperated Values
                foreach (long node in nodeIDs)
                {
                    nodeCSV.Append(node + ",");
                }

                // Establish connection to database
                using (var connection = new SqlConnection(_options.SqlConnectionString))
                {
                    //Open connection
                    await connection.OpenAsync();
                    var procedure = "dbo.[RateNodes]";
                    var parameters = new
                    {
                        @UserHash = userHash,
                        @InNodes = nodeCSV.ToString(),
                        @Rating = rating
                    };
                    //Execute command
                    var executed = await connection.ExecuteAsync(new CommandDefinition(procedure, parameters, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken)).ConfigureAwait(false);
                    return new RateResponse<int>("", rating, 200, true);
                }
            }
            catch (SqlException ex)
            {
                //Check sql exception
                switch (ex.Number)
                {
                    //Unable to connect to database
                    case -1:
                        return new RateResponse<int>(await _messageBank.GetMessage(IMessageBank.Responses.databaseConnectionFail), rating, 503, false);
                    case 547:   //Adding rating violates foreign key constraint (AKA NO ACCOUNT)
                        return new RateResponse<int>(await _messageBank.GetMessage(IMessageBank.Responses.accountNotFound), rating, 500, false);
                    default:
                        return new RateResponse<int>(await _messageBank.GetMessage(IMessageBank.Responses.unhandledException), rating, 500, false);
                }
            }
            catch (OperationCanceledException)
            {
                // Rollback already handled
                return new RateResponse<int>(await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested), rating, 408, false);
            }
            catch (Exception ex)
            {
                return new RateResponse<int>(await _messageBank.GetMessage(IMessageBank.Responses.unhandledException), rating, 500, false);
            }
        }

        /// <summary>
        ///     Returns a list of nodes containing IDs and rating of node
        /// </summary>
        /// <param name="nodeIDs">List of Node IDs</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns></returns>
        public async Task<IResponse<IEnumerable<Node>>> GetNodeRatingAsync(List<long> nodeIDs, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                // Check if node list is null or empty
                if(nodeIDs == null || nodeIDs.Count.Equals(0))
                {
                    return new RateResponse<IEnumerable<Node>>(await _messageBank.GetMessage(IMessageBank.Responses.nodeNotFound), new List<Node>(), 404, false);
                }

                StringBuilder nodeCSV = new StringBuilder();

                // Create Comma Seperated Values
                foreach(long node in nodeIDs)
                {
                    nodeCSV.Append(node + ",");
                }

                // Establish connection to database
                using (var connection = new SqlConnection(_options.SqlConnectionString))
                {
                    // Open connection
                    await connection.OpenAsync();
                    var procedure = "dbo.[GetNodesRatings]";
                    var parameters = new DynamicParameters();
                    parameters.Add("InNodes", nodeCSV.ToString());

                    var result = await connection.QueryAsync<Node>(new CommandDefinition(procedure, parameters, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken));

                    List<Node> nodeResult = new List<Node>(result.ToList());

                    // Tag has been added, return success
                    return new RateResponse<IEnumerable<Node>>("", nodeResult , 200, true);
                }
            }
            catch (SqlException ex)
            {
                // Check sql exception
                switch (ex.Number)
                {
                    // Unable to connect to database
                    case -1:
                        return new RateResponse<IEnumerable<Node>>(await _messageBank.GetMessage(IMessageBank.Responses.databaseConnectionFail), new List<Node>(), 503, false);
                    default:
                        return new RateResponse<IEnumerable<Node>>(await _messageBank.GetMessage(IMessageBank.Responses.unhandledException) + ex.Message, new List<Node>(), 500, false);
                }
            }
            catch (OperationCanceledException)
            {
                // bubble up
                return new RateResponse<IEnumerable<Node>>(await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested), new List<Node>(), 408, false);
            }
            catch (Exception ex)
            {
                return new RateResponse<IEnumerable<Node>>(await _messageBank.GetMessage(IMessageBank.Responses.unhandledException) + ex.Message, new List<Node>(), 500, false);
            }
        }

        public async Task<IResponse<int>> GetUserNodeRatingAsync(long nodeID, string userHash, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                //Throw Cancellation Exception if token requests cancellation
                cancellationToken.ThrowIfCancellationRequested();

                if (nodeID < 0)
                    return new RateResponse<int>(await _messageBank.GetMessage(IMessageBank.Responses.nodeNotFound), 0, 404, false);

                // Establish connection to database
                using (var connection = new SqlConnection(_options.SqlConnectionString))
                {
                    //Open connection
                    await connection.OpenAsync();
                    var procedure = "dbo.[GetUserNodeRating]";
                    var parameters = new { @NodeID = nodeID, @UserHash = userHash };

                    int result = await connection.ExecuteScalarAsync<int>(new CommandDefinition(procedure, parameters, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken)).ConfigureAwait(false);

                    //Tag has been added, return success
                    return new RateResponse<int>("", result, 200, true);
                }
            }
            catch (SqlException ex)
            {
                //Check sql exception
                switch (ex.Number)
                {
                    //Unable to connect to database
                    case -1:
                        return new RateResponse<int>(await _messageBank.GetMessage(IMessageBank.Responses.databaseConnectionFail), 0, 503, false);
                    default:
                        return new RateResponse<int>(await _messageBank.GetMessage(IMessageBank.Responses.unhandledException) + ex.Message, 0, 500, false);
                }
            }
            catch (OperationCanceledException)
            {
                // Rollback already handled
                return new RateResponse<int>(await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested), 0, 408, false);
            }
            catch (Exception ex)
            {
                return new RateResponse<int>(await _messageBank.GetMessage(IMessageBank.Responses.unhandledException) + ex.Message, 0, 500, false);
            }
        }

        public async Task<string> VerifyAuthorizedToView(List<long> nodeIDs, string userHash, CancellationToken cancellationToken = default)
        {
            try
            {
                //Throw Cancellation Exception if token requests cancellation
                cancellationToken.ThrowIfCancellationRequested();

                if (nodeIDs == null || nodeIDs.Count <= 0)
                    return await _messageBank.GetMessage(IMessageBank.Responses.nodeNotFound).ConfigureAwait(false);

                string nodeCSV = "";

                // Create Comma Seperated Values
                if(nodeIDs.Count > 1)
                {
                    foreach (long node in nodeIDs)
                    {
                        nodeCSV += node + ",";
                    }
                }
                else
                {
                    nodeCSV  += nodeIDs[0];
                }

                // Establish connection to database
                using (var connection = new SqlConnection(_options.SqlConnectionString))
                {
                    //Open connection
                    await connection.OpenAsync();
                    var procedure = "dbo.[VerifyAuthorizedToView]";
                    var parameters = new { InNodes = nodeCSV, UserHash = userHash};

                    var result = await connection.ExecuteScalarAsync<int>(new CommandDefinition(procedure, parameters, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken)).ConfigureAwait(false);

                    if (result == 0)
                        return await _messageBank.GetMessage(IMessageBank.Responses.notAuthorized).ConfigureAwait(false);

                    return await _messageBank.GetMessage(IMessageBank.Responses.verifySuccess).ConfigureAwait(false);
                }
            }
            catch (SqlException ex)
            {
                //Check sql exception
                switch (ex.Number)
                {
                    //Unable to connect to database
                    case -1:
                        return await _messageBank.GetMessage(IMessageBank.Responses.databaseConnectionFail);
                    default:
                        return await _messageBank.GetMessage(IMessageBank.Responses.unhandledException) + ex.Message;
                }
            }
            catch (OperationCanceledException)
            {
                // Rollback already handled
                return await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested);
            }
            catch (Exception ex)
            {
                return await _messageBank.GetMessage(IMessageBank.Responses.unhandledException) + ex.Message;
            }
        }

        public async Task<string> UpdateAccountAsync(IAccount account, IAccount updatedAccount, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                //Throw Cancellation Exception if token requests cancellation
                cancellationToken.ThrowIfCancellationRequested();
                // Establish connection to database
                using (var connection = new SqlConnection(_options.SqlConnectionString))
                {
                    //Open connection
                    await connection.OpenAsync();
                    var procedure = "dbo.[UpdateAccount]";
                    var parameters = new
                    {
                        Username = account.Username,
                        AuthorizationRole = account.AuthorizationLevel,
                        UpdatedUsername = account.Username,
                        UpdatedAuthorizationLevel = account.AuthorizationLevel,
                        UpdatedPassphrase = account.Passphrase,
                        UpdatedAccountStatus = account.AccountStatus,
                        UpdatedConfirmed = account.Confirmed
                    };
                    //Execute command
                    var executed = await connection.ExecuteAsync(new CommandDefinition(procedure, parameters, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken)).ConfigureAwait(false);
                    //Check if cancellation token requests cancellation
                    if (cancellationToken.IsCancellationRequested)
                    {
                        //Perform rollback
                        string rollbackResults = await UpdateAccountAsync(updatedAccount, account);
                        if (rollbackResults.Equals(await _messageBank.GetMessage(IMessageBank.Responses.accountUpdateSuccess)))
                            return await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested);
                        else
                            return await _messageBank.GetMessage(IMessageBank.Responses.rollbackFailed);
                    }
                    //Tag has been added, return success
                    return await _messageBank.GetMessage(IMessageBank.Responses.accountUpdateSuccess);
                }
            }
            catch (SqlException ex)
            {
                //Check sql exception
                switch (ex.Number)
                {
                    //Unable to connect to database
                    case -1:
                        return await _messageBank.GetMessage(IMessageBank.Responses.databaseConnectionFail);
                    default:
                        return _options.UnhandledExceptionMessage + ex.Message;
                }
            }
            catch (OperationCanceledException)
            {
                // Rollback already handled
                return await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested);
            }
            catch (Exception ex)
            {
                return await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message;
            }
        }



        /// <summary>
        ///     CopyNodeAsync method 
        /// </summary>
        /// <param name="nodesCopy"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IResponse<IEnumerable<Node>>> CopyNodeAsync(List<long> nodesCopy, CancellationToken cancellationToken = default(CancellationToken))

        {
            if(nodesCopy != null)
            {
                try
                {
                    
                    cancellationToken.ThrowIfCancellationRequested();
                    using (var connection = new SqlConnection(_options.SqlConnectionString))
                    {

                        //dont use datatable for one column
                        //creating a DataTable to pass into the query as a table valued parameter
                        var workTable = new DataTable();
                        DataColumn workCol = workTable.Columns.Add("NodeIDsColumn", typeof(long));
                        workCol.AllowDBNull = false;
                        workCol.Unique = true;

                        //adding rows of nodeIDs to the DataTable
                        foreach (var n in nodesCopy)
                        {
                            var row = workTable.NewRow();
                            row[0] = n;
                            workTable.Rows.Add(row);
                        }

                        await connection.OpenAsync();

                        var procedure = "dbo.[GetNodesFromNodeID]";


                        // Calling stored procedure to get list of nodes from the azure database
                        List<Node> nodes = new List<Node>(await connection.QueryAsync<Node>(new CommandDefinition(procedure, new { NodeIDs = workTable.AsTableValuedParameter("dbo.NodeIDList") },
                        commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken)).ConfigureAwait(false)).ToList();


                        // Checking if got correct amount of nodes back from query
                        if (!nodes.Count.Equals(nodesCopy.Count))
                        {
                            return new CopyResponse<IEnumerable<Node>>(await _messageBank.GetMessage(IMessageBank.Responses.copyNodeMistmatchError).ConfigureAwait(false), null, 400, false);
                        }

                        return new CopyResponse<IEnumerable<Node>>("", nodes, 200, true);

                    }
                }


                catch (SqlException ex)
                {
                    //Check sql exception
                    switch (ex.Number)
                    {
                        //Unable to connect to database
                        case -1:
                            return new CopyResponse<IEnumerable<Node>>( await _messageBank.GetMessage(IMessageBank.Responses.databaseConnectionFail).ConfigureAwait(false), null, 400, false);
                        default:
                            return new CopyResponse<IEnumerable<Node>>(_options.UnhandledExceptionMessage + ex.Message, null, 400, false);
                    }
                }
                //let bubble up one layer, dont catch here
                catch (OperationCanceledException)
                {
                    //return code for operationCancelled is 500
                    return new CopyResponse<IEnumerable<Node>>(await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested).ConfigureAwait(false), null, 500, false);

                }
                catch (Exception ex)
                {
                    //return code for unhandledException is 500
                    return new CopyResponse<IEnumerable<Node>>(await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message, null, 500, false);

                }
            }

            else
            {
                return new CopyResponse<IEnumerable<Node>>(await _messageBank.GetMessage(IMessageBank.Responses.copyNodeEmptyError).ConfigureAwait(false), null, 400, false);
            }


        }


        //only need paste, copy kind of redundant
        //document based db, represents data better
        //openXML, XML rowset, send data as xml format, or csv format, it can load data into temp memtable, and do ops there,
        //turn list<inode> into csv and load it into db, extensible longterm, csv can be loaded into any db, 
        //DOING DESIGN, see if changing state or storing state then backend really needed, retriveing state, then backend just to read,
        //think of document, doing actiosn in dcoument, ex closing document without saving: data lost, vs typing no need to involve backend



        public async Task<IResponse<string>> PasteNodeAsync(string userHash, long nodeIDPasteTo, List<INode> nodes, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                using (var connection = new SqlConnection(_options.SqlConnectionString))
                {


                    var procedure = "dbo.[PasteNodes]";

                    var workTable = new DataTable();
                    DataColumn workCol = workTable.Columns.Add("UserHashColumn", typeof(string));
                    workTable.Columns.Add("NodeIDColumn", typeof(long));
                    workTable.Columns.Add("ParentNodeIDColumn", typeof(int));
                    workTable.Columns.Add("NodeTitleColumn", typeof(string));
                    workTable.Columns.Add("SummaryColumn", typeof(string));
                    workTable.Columns.Add("TimeModifiedColumn", typeof(DateTime));
                    workTable.Columns.Add("VisibilityColumn", typeof(int));
                    workTable.Columns.Add("DeletedColumn", typeof(int));
                    workCol.AllowDBNull = false;
                    workCol.Unique = true;

                    foreach (var n in nodes)
                    {
                        var row = workTable.NewRow();
                        row[0] = n.UserHash;
                        row[1] = n.NodeID;
                        row[2] = n.ParentNodeID;
                        row[3] = n.NodeTitle;
                        row[4] = n.Summary;
                        row[5] = n.TimeModified;
                        row[6] = n.Visibility;
                        row[7] = n.Deleted;
                        workTable.Rows.Add(row);
                    }

                    await connection.OpenAsync();


                    int affectedRows = await connection.ExecuteScalarAsync<int>(new CommandDefinition(procedure, new { NodesPaste = workTable.AsTableValuedParameter("dbo.NodesToPaste") }, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken)).ConfigureAwait(false);

                    if (affectedRows.Equals(0))
                    {
                        return new PasteResponse<string>(await _messageBank.GetMessage(IMessageBank.Responses.pasteNodeEmptyError).ConfigureAwait(false), null, 400, false);
                    }
                    else if (!affectedRows.Equals(nodes.Count))
                    {
                        return new PasteResponse<string>(await _messageBank.GetMessage(IMessageBank.Responses.pasteNodeMistmatchError).ConfigureAwait(false), null, 400, false);
                    }

                    return new PasteResponse<string>("", await _messageBank.GetMessage(IMessageBank.Responses.pasteNodeSuccess).ConfigureAwait(false), 200, true);

                }
            }

            catch (SqlException ex)
            {
                //Check sql exception
                switch (ex.Number)
                {
                    //Unable to connect to database
                    case -1:
                        return new PasteResponse<string>(await _messageBank.GetMessage(IMessageBank.Responses.databaseConnectionFail).ConfigureAwait(false), null, 400, false);
                    default:
                        return new PasteResponse<string>(_options.UnhandledExceptionMessage + ex.Message, null, 400, false);
                }
            }
            catch (OperationCanceledException)
            {
                //return code for operationCancelled is 500
                return new PasteResponse<string>(await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested).ConfigureAwait(false), null, 500, false);

            }
            catch (Exception ex)
            {
                //return code for unhandledException is 500
                return new PasteResponse<string>(await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message, null, 500, false);

            }
        }



        public async Task<string> IsNodeLeaf(long nodeIDToPasteTo, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (!nodeIDToPasteTo.Equals(null))
            {
                try
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    using (var connection = new SqlConnection(_options.SqlConnectionString))
                    {


                        var procedure = "dbo.[IsNodeLeaf]";

                        await connection.OpenAsync();

                        var parameters = new { nodeIDToPasteTo };

                        int affectedRows = await connection.ExecuteScalarAsync<int>(new CommandDefinition(procedure, parameters, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken)).ConfigureAwait(false);

                        if (!affectedRows.Equals(0))
                        {
                            return await _messageBank.GetMessage(IMessageBank.Responses.isNotLeaf).ConfigureAwait(false);
                        }

                        return await _messageBank.GetMessage(IMessageBank.Responses.isLeaf).ConfigureAwait(false);

                    }

                }

                catch (SqlException ex)
                {
                    //Check sql exception
                    switch (ex.Number)
                    {
                        //Unable to connect to database
                        case -1:
                            return await _messageBank.GetMessage(IMessageBank.Responses.databaseConnectionFail).ConfigureAwait(false);
                        default:
                            return _options.UnhandledExceptionMessage + ex.Message;
                    }
                }
                catch (OperationCanceledException)
                {
                    //return code for operationCancelled is 500
                    return await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested).ConfigureAwait(false);

                }
                catch (Exception ex)
                {
                    //return code for unhandledException is 500
                    return await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false);
                }
            }
            else
            {
                return await _messageBank.GetMessage(IMessageBank.Responses.copyNodeEmptyError).ConfigureAwait(false);
            }
        }


        public async Task<IResponse<string>> PrivateNodeAsync(List<long> nodes, CancellationToken cancellationToken = default(CancellationToken))
        {
            if(nodes != null || nodes.Count != 0)
            {

                try
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    int privatizedNodes;

                    using (var connection = new SqlConnection(_options.SqlConnectionString))
                    {
                        var procedure = "dbo.PrivatizeNodeS";

                        string nodeCSV = "";

                        foreach(long node in nodes)
                        {
                            nodeCSV += node + ",";
                        }

                        var parameters = new { @NodeIDS = nodeCSV };

                        await connection.OpenAsync();

                        privatizedNodes = await connection.ExecuteAsync(new CommandDefinition(procedure, parameters, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken)).ConfigureAwait(false);

                        if(privatizedNodes <= 0 || privatizedNodes != nodes.Count)
                        {
                            return new PrivateResponse<string>(await _messageBank.GetMessage(IMessageBank.Responses.privateNodeFailure).ConfigureAwait(false), null, 400, false);
                        }

                        return new PrivateResponse<string>("", await _messageBank.GetMessage(IMessageBank.Responses.privateNodeSuccess).ConfigureAwait(false), 200, true);


                    }
                }
                catch (OperationCanceledException)
                {
                    //return code for operationCancelled is 500
                    return new PrivateResponse<string>(await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested).ConfigureAwait(false), null, 500, false);

                }
                catch (Exception ex)
                {
                    //return code for unhandledException is 500
                    return new PrivateResponse<string>(await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message, null, 500, false);

                }

            }

            //if there was nothing to copy, should not have happened
            return new PrivateResponse<string>(await _messageBank.GetMessage(IMessageBank.Responses.privateNodeFailure).ConfigureAwait(false), null, 400, false);


        }




        
        public async Task<IResponse<string>> PublicNodeAsync(List<long> nodes, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (nodes != null || nodes.Count != 0)
            {

                try
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    int privatizedNodes;

                    using (var connection = new SqlConnection(_options.SqlConnectionString))
                    {
                        var procedure = "dbo.PublicizeNodeS";

                        string nodeCSV = "";

                        foreach (long node in nodes)
                        {
                            nodeCSV += node + ",";
                        }

                        var parameters = new { @NodeIDS = nodeCSV };

                        await connection.OpenAsync();

                        privatizedNodes = await connection.ExecuteAsync(new CommandDefinition(procedure, parameters, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken)).ConfigureAwait(false);

                        if (privatizedNodes <= 0 || privatizedNodes != nodes.Count)
                        {
                            return new PublicResponse<string>(await _messageBank.GetMessage(IMessageBank.Responses.publicNodeFailure).ConfigureAwait(false), null, 400, false);
                        }

                        return new PublicResponse<string>("", await _messageBank.GetMessage(IMessageBank.Responses.publicNodeSuccess).ConfigureAwait(false), 200, true);


                    }
                }
                catch (OperationCanceledException)
                {
                    //return code for operationCancelled is 500
                    return new PublicResponse<string>(await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested).ConfigureAwait(false), null, 500, false);

                }
                catch (Exception ex)
                {
                    //return code for unhandledException is 500
                    return new PublicResponse<string>(await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message, null, 500, false);

                }

            }

            //if there was nothing to copy, should not have happened
            return new PublicResponse<string>(await _messageBank.GetMessage(IMessageBank.Responses.publicNodeFailure).ConfigureAwait(false), null, 400, false);


        }

        







    }
}
