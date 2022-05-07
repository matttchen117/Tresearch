using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Exceptions;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Services.Implementations;

namespace TrialByFire.Tresearch.Managers.Implementations
{
    /// <summary>
    ///     Manager class for enforcing business rules for creating a Node and calling the service for operation
    /// </summary>
    public class DeleteNodeManager : IDeleteNodeManager
    {
        private ISqlDAO _sqlDAO { get; }
        private ILogService _logService { get; }
        private IAuthorizationService _authorizationService { get; }
        private IDeleteNodeService _deleteNodeService { get; }
        private IMessageBank _messageBank { get; }

        /// <summary>
        ///     Constructor for creating the manager
        /// </summary>
        /// <param name="sqlDAO"></param>
        /// <param name="logService"></param>
        /// <param name="authorizationService"></param>
        /// <param name="deleteNodeService"></param>
        /// <param name="messageBank"></param>
        public DeleteNodeManager(ISqlDAO sqlDAO, ILogService logService, IAuthorizationService authorizationService, IDeleteNodeService deleteNodeService, IMessageBank messageBank)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
            _authorizationService = authorizationService;
            _deleteNodeService = deleteNodeService;
            _messageBank = messageBank;
        }

        /// <summary>
        /// Checks that the User attempting to create a Node is the same as the onwer of the tree.
        /// </summary>
        /// <param name="userhash">The userhash of the Node attempting to be deleted</param>
        /// <param name="nodeID">The ParentNodeID of the Node being deleted</param>
        /// <param name="parentID">Node ID for delettion</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The result of the operation.</returns>
        /// <exception cref="OperationCanceledException"></exception>
        public async Task<IResponse<string>> DeleteNodeAsync(string userhash, long nodeID, long parentID, CancellationToken cancellationToken = default)
        {
            // Perform a check that current user has the ability to delete the Node it's given userHash
            if(userhash == (Thread.CurrentPrincipal.Identity as RoleIdentity).UserHash)
            {
                try
                {
                    IResponse<string> response = await _deleteNodeService.DeleteNodeAsync(nodeID, parentID, cancellationToken).ConfigureAwait(false);
                    
                    // Set error message for cancellation
                    if (cancellationToken.IsCancellationRequested)
                    {
                        MethodBase? m = MethodBase.GetCurrentMethod();
                        if (m != null)
                        {
                            response.ErrorMessage = await _messageBank.GetMessage(IMessageBank.Responses.operationTimeExceeded).
                                ConfigureAwait(false) + m.Name;
                        }
                    }
                    return response;
                }
                catch (Exception ex)
                {
                    return new DeleteNodeResponse<string>(await _messageBank.GetMessage(
                        IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message, null, 400, false);
                }
            }
            else
            {
                return new DeleteNodeResponse<string>(await _messageBank.GetMessage(
                    IMessageBank.Responses.notAuthenticated).ConfigureAwait(false), null, 400, false);
            }
        }
    }
}