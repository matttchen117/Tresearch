using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Services.Implementations
{
    /// <summary>
    ///     Tag Service: Class used to perform business logic regarding a node's tags
    /// </summary>
    public class TagService
    {
        private ISqlDAO _sqlDAO { get; set; }

        private ILogService _logService { get; set; }

        private IMessageBank _messageBank { get; set; }

        /// <summary>
        ///     public RecoveryClass(sqlDAO, logService, messageBank)
        ///         Constructor for Tag service class
        /// </summary>
        /// <param name="sqlDAO"> SQL object to perform changes to database</param>
        /// <param name="logService"> Log service</param>
        /// <param name="messageBank"> Message bank containing status code enumerables</param>
        public TagService(ISqlDAO sqlDAO, ILogService logService, IMessageBank messageBank)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
            _messageBank = messageBank;
        }

        


    }
}
