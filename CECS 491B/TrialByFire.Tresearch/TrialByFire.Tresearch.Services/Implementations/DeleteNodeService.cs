using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Services.Contracts;


namespace TrialByFire.Tresearch.Services.Implementations
{
    public class DeleteNodeService : IDeleteNodeService
    {
        private ISqlDAO _sqlDAO { get; }
        private ILogService _logService { get; }
        private IMessageBank _messageBank { get; }

        /// <summary>
        /// Constructor for creating the service
        /// </summary>
        /// <param name="sqlDAO"></param>
        /// <param name="logService"></param>
        /// <param name="messageBank"></param>
        public DeleteNodeService(ISqlDAO sqlDAO, ILogService logService, IMessageBank messageBank)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
            _messageBank = messageBank;
        }

        public async Task<string> DeleteNodeAsync(long nodeID, long parentID, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string deleteResult;
            try
            {
                deleteResult = await _sqlDAO.DeleteNodeAsync(nodeID, parentID, cancellationToken).ConfigureAwait(false);
                return deleteResult;
            }
            catch(OperationCanceledException ece)
            {
                throw;
            }
            catch(Exception ex)
            {
                return ("500: Server: " + ex.Message);
            }
        }
    }
}
