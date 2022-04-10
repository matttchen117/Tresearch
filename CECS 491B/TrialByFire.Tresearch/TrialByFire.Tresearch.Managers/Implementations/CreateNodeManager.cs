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
    public class CreateNodeManager : ICreateNodeManager
    {
        private ISqlDAO _sqlDAO { get; }
        private ILogService _logService { get; }
        private IValidationService _validationService { get; }
        private IAuthenticationService _authenticationService { get; }
        private ICreateNodeService _createNodeService { get; }
        private IMessageBank _messageBank { get; }

        /// <summary>
        /// Constructor for creating the manager
        /// </summary>
        /// <param name="sqlDAO"></param>
        /// <param name="logService"></param>
        /// <param name="validationService"></param>
        /// <param name="createNodeService"></param>
        /// <param name="messageBank"></param>
        public CreateNodeManager(ISqlDAO sqlDAO, ILogService logService, IValidationService validationService, ICreateNodeService createNodeService, IMessageBank messageBank)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
            _validationService = validationService;
            _createNodeService = createNodeService;
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
        public async Task<string> CreateNodeAsync(string username, Node node, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string result = "";
            try
            {
                if(Thread.CurrentPrincipal != null)
                {
                    if(username == Thread.CurrentPrincipal.Identity.Name)
                    {
                        result = (await _createNodeService.CreateNodeAsync(username, node, cancellationToken).ConfigureAwait(false));
                    }
                    else
                    {
                        result = await _messageBank.GetMessage(IMessageBank.Responses.notFoundOrAuthorized).ConfigureAwait(false);
                    }
                }
                else
                {
                    result = await _messageBank.GetMessage(IMessageBank.Responses.notAuthenticated).ConfigureAwait(false);
                }

                if (cancellationToken.IsCancellationRequested && result.Equals(_messageBank.GetMessage(IMessageBank.Responses.generic).Result))
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
                }
                return result;
            }
            catch(CreateNodeFailedException cnfe)
            {
                return cnfe.Message;
            }
        }
    }
}
