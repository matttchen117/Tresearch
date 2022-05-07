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
    /// <summary>
    /// A service class for creating Nodes
    /// </summary>
    public class CreateNodeService : ICreateNodeService
    {
        private ISqlDAO _sqlDAO { get; }
        private IMessageBank _messageBank { get; }

        /// <summary>
        /// Constructor for creating the service
        /// </summary>
        /// <param name="sqlDAO"></param>
        /// <param name="logService"></param>
        /// <param name="messageBank"></param>
        public CreateNodeService(ISqlDAO sqlDAO, ILogService logService, IMessageBank messageBank)
        {
            _sqlDAO = sqlDAO;
            _messageBank = messageBank;
        }

        /// <summary>
        /// Checks that the User attempting to create a Node is the same as the onwer of the tree.
        /// </summary>
        /// <param name="node">Node object for creation</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The result of the operation.</returns>
        /// <returns>The result of the operation with any status codes if applicable</returns>
        public async Task<IResponse<string>> CreateNodeAsync(INode node, CancellationToken cancellationToken = default)
        {
            if (node != null)
            {
                try
                {
                    IResponse<string> response = await _sqlDAO.CreateNodeAsync(node, cancellationToken).ConfigureAwait(false);
                    return response;
                }
                catch (Exception ex)
                {
                    return new CreateNodeResponse<string>(await _messageBank.GetMessage(
                        IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message, null, 400, false);
                }
            }
            else
            {
                return new CreateNodeResponse<string>(await _messageBank.GetMessage(
                    IMessageBank.Responses.noNodeInput).ConfigureAwait(false), null, 400, false);
            }
        }
    }
}
