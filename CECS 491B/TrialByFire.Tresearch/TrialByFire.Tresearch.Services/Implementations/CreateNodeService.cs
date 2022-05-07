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
        private ILogService _logService { get; }
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
            _logService = logService;
            _messageBank = messageBank;
        }

        /// <summary>
        /// Checks that the User attempting to create a Node is the same as the onwer of the tree.
        /// </summary>
        /// <param name="account">The username attempting to create a Node</param>
        /// <param name="node">Node object for creation</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The result of the operation.</returns>
        /// <returns>The result of the operation with any status codes if applicable</returns>
        public async Task<string> CreateNodeAsync(IAccount account, INode node, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string result;
            try
            {
                //node.accountOwner = (Thread.CurrentPrincipal.Identity as RoleIdentity).UserHash;
                result = await _sqlDAO.CreateNodeAsync(node, cancellationToken).ConfigureAwait(false);
                /*if (cancellationToken.IsCancellationRequested && result.Equals(_messageBank.GetMessage(IMessageBank.Responses.generic).Result))
                {
                    string rollbackResult = "Delete Node Success";
                    if (rollbackResult != "Delete Node Success")
                    {
                        return await _messageBank.GetMessage(IMessageBank.Responses.rollbackFailed).ConfigureAwait(false);
                    }
                    else
                    {
                        throw new OperationCanceledException();
                    }
                }*/
                return result;
            }
            catch (OperationCanceledException ece)
            {
                throw;
            }
            catch (Exception ex)
            {
                return ("500: Server: " + ex.Message);
            }
        }
    }
}
