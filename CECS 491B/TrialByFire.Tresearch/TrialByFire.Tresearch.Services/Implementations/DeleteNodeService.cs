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

        public async Task<IResponse<string>> DeleteNodeAsync(long nodeID, long parentID, CancellationToken cancellationToken = default)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                IResponse<string> response = await _sqlDAO.DeleteNodeAsync(nodeID, parentID, cancellationToken).ConfigureAwait(false);
                return response;
            }
            catch(Exception ex)
            {
                return new DeleteNodeResponse<string>(await _messageBank.GetMessage(
                    IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message, null, 400, false);
            }
        }
    }
}
