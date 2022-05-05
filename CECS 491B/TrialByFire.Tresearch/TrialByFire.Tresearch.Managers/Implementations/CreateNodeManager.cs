using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
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
    public class CreateNodeManager : ICreateNodeManager
    {
        private ISqlDAO _sqlDAO { get; }
        private ILogService _logService { get; }
        private IAuthorizationService _authorizationService { get; }
        private ICreateNodeService _createNodeService { get; }
        private IMessageBank _messageBank { get; }

        /// <summary>
        /// Constructor for creating the manager
        /// </summary>
        /// <param name="sqlDAO"></param>
        /// <param name="logService"></param>
        /// <param name="authorizationService"></param>
        /// <param name="createNodeService"></param>
        /// <param name="messageBank"></param>
        public CreateNodeManager(ISqlDAO sqlDAO, ILogService logService, IAuthorizationService authorizationService, ICreateNodeService createNodeService, IMessageBank messageBank)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
            _authorizationService = authorizationService;
            _createNodeService = createNodeService;
            _messageBank = messageBank;
        }

        /// <summary>
        /// Checks that the User attempting to create a Node is the same as the onwer of the tree.
        /// </summary>
        /// <param name="userhash">The userHash attempting to create a Node</param>
        /// <param name="node">Node object for creation</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The result of the operation.</returns>
        /// <exception cref="OperationCanceledException"></exception>
        public async Task<IResponse<string>> CreateNodeAsync(string userhash, Node node, CancellationToken cancellationToken = default)
        {
            if(userhash == (Thread.CurrentPrincipal.Identity as RoleIdentity).UserHash)
            {
                IResponse<string> response = await _createNodeService.CreateNodeAsync(node, cancellationToken).ConfigureAwait(false);

                if (cancellationToken.IsCancellationRequested)
                {
                    MethodBase? m = MethodBase.GetCurrentMethod();
                    if(m != null)
                    {
                        response.ErrorMessage = await _messageBank.GetMessage(IMessageBank.Responses.operationTimeExceeded).ConfigureAwait(false) + m.Name;
                    }
                }

                return response;
            }
            else
            {
                return new CreateNodeResponse<string>(await _messageBank.GetMessage(
                    IMessageBank.Responses.notAuthorized).ConfigureAwait(false), null, 400, false);
            }
        }
    }
}
