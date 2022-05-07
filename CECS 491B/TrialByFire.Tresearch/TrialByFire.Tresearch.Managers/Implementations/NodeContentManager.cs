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
    public class NodeContentManager : INodeContentManager
    {
        private IMessageBank _messageBank;
        private INodeContentService _nodeContentService;
        private BuildSettingsOptions _options { get; }
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource(
            TimeSpan.FromSeconds(5));
        public NodeContentManager(IMessageBank messageBank, INodeContentService nodeContentService, IOptionsSnapshot<BuildSettingsOptions> options)
        {
            _messageBank = messageBank;
            _nodeContentService = nodeContentService;
            _options = options.Value;
        }

        public async Task<IResponse<string>> UpdateNodeContentAsync(INodeContentInput nodeContentInput)
        {
            if (nodeContentInput == null)
            {
                return new NodeContentResponse<string>(await _messageBank.GetMessage(IMessageBank.Responses.noSearchInput).ConfigureAwait(false), "", 400, false);
            }
            if (Thread.CurrentPrincipal == null || Thread.CurrentPrincipal.Identity == null)
            {
                return new NodeContentResponse<string>(await _messageBank.GetMessage(IMessageBank.Responses.principalNotSet).ConfigureAwait(false), "", 400, false);
            }
            try
            {
                if(nodeContentInput.Owner != (Thread.CurrentPrincipal.Identity as IRoleIdentity).UserHash)
                {
                    return new NodeContentResponse<string>(await _messageBank.GetMessage(IMessageBank.Responses.notAuthorized).ConfigureAwait(false), "", 400, false);
                }
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
