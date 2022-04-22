using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
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
    /// Manager class for enforcing business rules for creating a Node and calling the service for operation
    /// </summary>
    public class DeleteNodeManager : IDeleteNodeManager
    {
        private ISqlDAO _sqlDAO { get; }
        private ILogService _logService { get; }
        private IAuthorizationService _authorizationService { get; }
        private IDeleteNodeService _deleteNodeService { get; }
        private IMessageBank _messageBank { get; }

        /// <summary>
        /// Constructor for creating the manager
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
        /// <param name="username">The username attempting to create a Node</param>
        /// <param name="node">Node object for creation</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The result of the operation.</returns>
        /// <exception cref="OperationCanceledException"></exception>
        public async Task<string> DeleteNodeAsync(IAccount account, long nodeID, long parentID, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string deleteResult;
            try
            {
                if (Thread.CurrentPrincipal.Identity.Name != "guest")
                {
                    if (account.Username == Thread.CurrentPrincipal.Identity.Name)
                    {
                        bool verificationResult = await _authorizationService.VerifyAuthorizedAsync(account.AuthorizationLevel, account.Username, cancellationToken).ConfigureAwait(false);
                        if (verificationResult)
                        {
                            deleteResult = await _deleteNodeService.DeleteNodeAsync(nodeID, parentID, cancellationToken).ConfigureAwait(false);
                        }
                        else
                        {
                            deleteResult = await _messageBank.GetMessage(IMessageBank.Responses.notAuthorized).ConfigureAwait(false);
                        }
                        return deleteResult;
                    }
                    else
                    {
                        return await _messageBank.GetMessage(IMessageBank.Responses.notAuthorized).ConfigureAwait(false);
                    }
                }
                else
                {
                    return await _messageBank.GetMessage(IMessageBank.Responses.notAuthorized).ConfigureAwait(false);
                }
            }
            catch (DeleteNodeFailedException cnfe)
            {
                return cnfe.Message;
            }
        }
    }
}