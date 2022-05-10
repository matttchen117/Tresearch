using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Models;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Services.Contracts;

namespace TrialByFire.Tresearch.Managers.Implementations
{
    /// <summary>
    ///     NodeContentManager: Class that is part of the Manager abstraction layer that handles business rules related to UpdateNodeContent
    /// </summary>
    public class NodeContentManager : INodeContentManager
    {
        private IMessageBank _messageBank;
        private INodeContentService _nodeContentService;
        private BuildSettingsOptions _options { get; }
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource(
            TimeSpan.FromSeconds(5));
        /// <summary>
        ///     
        /// </summary>
        /// <param name="messageBank">Object that contains error and success messages</param>
        /// <param name="nodeContentService">Service object for Service abstraction layer to perform services related to UpdateNodeContent</param>
        /// <param name="options">Snapshot object that represents the setings/configurations of the application</param>
        public NodeContentManager(IMessageBank messageBank, INodeContentService nodeContentService, IOptionsSnapshot<BuildSettingsOptions> options)
        {
            _messageBank = messageBank;
            _nodeContentService = nodeContentService;
            _options = options.Value;
        }

        /// <summary>
        ///     UpdateNodeContentAsync:
        ///         Async method that checks business rules related to UpdateNodeContent before calling Service layer
        /// </summary>
        /// <param name="nodeContentInput">Custom input object that contains relevant information for methods related to UpdateNodeContent</param>
        /// <returns>Response that contains the result of the database operation</returns>
        public async Task<IResponse<string>> UpdateNodeContentAsync(INodeContentInput nodeContentInput)
        {
            // Secutiry cehck as private methdo, separate top and bottom
            // Input validation typicall reusable, have a class for each validation check
            // Check most likely scenarios first to exit early, check other things later
            // Do input validation before check if valid user

            // Check if input null
            if (nodeContentInput == null)
            {
                return new NodeContentResponse<string>(await _messageBank.GetMessage(IMessageBank.Responses.noSearchInput).ConfigureAwait(false), "", 400, false);
            }
            // Check if principal not set
            if (Thread.CurrentPrincipal == null || Thread.CurrentPrincipal.Identity == null)
            {
                return new NodeContentResponse<string>(await _messageBank.GetMessage(IMessageBank.Responses.principalNotSet).ConfigureAwait(false), "", 400, false);
            }
            try
            {
                // Check if owner
                if(nodeContentInput.Owner != (Thread.CurrentPrincipal.Identity as IRoleIdentity).UserHash)
                {
                    return new NodeContentResponse<string>(await _messageBank.GetMessage(IMessageBank.Responses.notAuthorized).ConfigureAwait(false), "", 400, false);
                }
                // Check if within lengths
                if (nodeContentInput.NodeTitle.Length > _options.NodeTitleMaxLength)
                {
                    return new NodeContentResponse<string>(await _messageBank.GetMessage(IMessageBank.Responses.nodeTitleLengthExceeded).ConfigureAwait(false), "", 400, false);
                }
                if(nodeContentInput.Summary.Length > _options.NodeSummaryMaxLength)
                {
                    return new NodeContentResponse<string>(await _messageBank.GetMessage(IMessageBank.Responses.nodeSummaryLengthExceeded).ConfigureAwait(false), "", 400, false);
                }
                nodeContentInput.CancellationToken = _cancellationTokenSource.Token;
                return await _nodeContentService.UpdateNodeContentAsync(nodeContentInput).ConfigureAwait(false);
            }
            catch(Exception ex)
            {
                return new NodeContentResponse<string>(await _messageBank.GetMessage(
                    IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message, "", 500, false);
            }
        }
    }
}
