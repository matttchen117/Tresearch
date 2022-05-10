using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;

namespace TrialByFire.Tresearch.DAL.Implementations
{
    /// <summary>
    ///     NodeSearchService: Class that is part of the Service abstraction layer that performs services related to Logging
    /// </summary>
    public class LogService : ILogService
    {
        private  ISqlDAO _sqlDAO { get; }
        private IMessageBank _messageBank { get; }

        /// <summary>
        ///     public LogService():
        ///         Constructor for LogService class
        /// </summary>
        /// <param name="sqlDAO">SQL Data Access Object to interact with the database</param>
        /// <param name="messageBank">Object that contains error and success messages</param>
        public LogService(ISqlDAO sqlDAO, IMessageBank messageBank)
        {
            _sqlDAO = sqlDAO;
            _messageBank = messageBank;
        }

        /// <summary>
        ///     CreateLogAsync:
        ///         Async method to create a Log object
        /// </summary>
        /// <param name="timestamp">The time of the Log</param>
        /// <param name="level">The level of the Log</param>
        /// <param name="category">The category of the Log</param>
        /// <param name="description">The description of the Log</param>
        /// <param name="cancellationToken">The cancellation token of the operation</param>
        /// <returns>The newly created Log object</returns>
        public async Task<ILog> CreateLogAsync(DateTime timestamp, string level, string category, 
            string description, CancellationToken cancellationToken = default)
        {
            // 
            if(Thread.CurrentPrincipal is null || Thread.CurrentPrincipal.Identity is null || !(Thread.CurrentPrincipal.Identity is IRoleIdentity))
            {
                return null;
            }
            try
            {
                string userHash = (Thread.CurrentPrincipal.Identity as IRoleIdentity).UserHash;
                StringBuilder builder = new StringBuilder();
                builder.AppendFormat("{0} {1} {2} {3} {4}", timestamp.ToUniversalTime().ToString(), level,
                    userHash, category, description);
                string payload = builder.ToString();
                byte[] salt = new byte[0];
                byte[] key = KeyDerivation.Pbkdf2(payload, salt, KeyDerivationPrf.HMACSHA512, 10000, 64);
                string hash = Convert.ToHexString(key);
                ILog log = new Log(timestamp, level, userHash, category, description, hash);
                return log;
            }catch (Exception ex)
            {
                throw ex;   
            }
        }

        /// <summary>
        ///     StoreLogAsync:
        ///         Async method to store a Log to a destination
        /// </summary>
        /// <param name="log">The Log to store</param>
        /// <param name="destination">Where to store the Log (table in the database)</param>
        /// <param name="cancellationToken">The cancellation token of the operation</param>
        /// <returns>The result of the attempt to store the Log</returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<string> StoreLogAsync(ILog log, string destination, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                int result = await _sqlDAO.StoreLogAsync(log, destination, cancellationToken).ConfigureAwait(false);
                switch (result)
                {
                    case 0:
                        return await _messageBank.GetMessage(IMessageBank.Responses.logFail)
                            .ConfigureAwait(false);
                    case 1:
                        return await _messageBank.GetMessage(IMessageBank.Responses.logSuccess)
                            .ConfigureAwait(false);
                    case 2:
                        return await _messageBank.GetMessage(IMessageBank.Responses.logRollback)
                            .ConfigureAwait(false);
                    default:
                        throw new NotImplementedException();
                };
            }catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }
    }
}
