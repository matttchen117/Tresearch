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
    public class LogService : ILogService
    {
        private  ISqlDAO _sqlDAO { get; }
        private IMessageBank _messageBank { get; }

        public LogService(ISqlDAO sqlDAO, IMessageBank messageBank)
        {
            _sqlDAO = sqlDAO;
            _messageBank = messageBank;
        }

        public async Task<string> StoreLogAsync(DateTime timestamp, string level, string username, 
            string category, string description, CancellationToken cancellationToken = default)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.AppendFormat("{0} {1} {2} {3} {4}", timestamp.ToUniversalTime().ToString(), level, username, category,
                    description);
                string payload = builder.ToString();
                byte[] salt = new byte[0];
                byte[] key = KeyDerivation.Pbkdf2(payload, salt, KeyDerivationPrf.HMACSHA512, 10000, 64);
                string hash = Convert.ToHexString(key);
                ILog log = new Log(timestamp, level, username, category, description, hash);
                int result = await _sqlDAO.StoreLogAsync(log, cancellationToken).ConfigureAwait(false);
                switch (result)
                {
                    case 0:
                        return await _messageBank.GetMessage(IMessageBank.Responses.logFail)
                            .ConfigureAwait(false);
                    case 1:
                        return await _messageBank.GetMessage(IMessageBank.Responses.logSuccess)
                            .ConfigureAwait(false); ;
                    case 2:
                        return await _messageBank.GetMessage(IMessageBank.Responses.logRollback)
                            .ConfigureAwait(false); ;
                    default:
                        throw new NotImplementedException();
                };
            }catch (Exception ex)
            {
                // Revise
                return ex.Message;
            }
        }
    }
}
