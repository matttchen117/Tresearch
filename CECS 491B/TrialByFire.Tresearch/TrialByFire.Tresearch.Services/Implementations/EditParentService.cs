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
    public class EditParentService : IEditParentService
    {
        private ISqlDAO _sqlDAO;
        private IMessageBank _messageBank;

        public EditParentService(ISqlDAO sqlDAO, IMessageBank messageBank)
        {
            _sqlDAO = sqlDAO;
            _messageBank = messageBank;
        }

        public async Task<IResponse<string>> EditParentNodeAsync(long nodeID, string nodeIDs, CancellationToken cancellationToken = default)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                IResponse<string> response = await _sqlDAO.EditParentNodeAsync(nodeID, nodeIDs, cancellationToken);
                return response;
            }
            catch(Exception ex)
            {
                return new EditParentResponse<string>(await _messageBank.GetMessage(
                    IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message, null, 400, false);
            }
        }
    }
}
