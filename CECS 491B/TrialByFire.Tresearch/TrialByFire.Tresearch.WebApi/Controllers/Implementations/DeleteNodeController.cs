using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Text;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Managers.Implementations;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.WebApi.Controllers.Contracts;

namespace TrialByFire.Tresearch.WebApi.Controllers.Implementations
{
    /// <summary>
    ///     DeleteNodeController: Class that is part of the Controller abstraction layer that handles receiving and returning
    ///         HTTP response and requests
    /// </summary>
    [ApiController]
    [EnableCors]
    [Route("[controller]")]
    public class DeleteNodeController : Controller, IDeleteNodeController
    {
        private ILogManager _logManager { get; }
        private IDeleteNodeManager _deleteNodeManager { get; }
        private IMessageBank _messageBank { get; }  
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        /// <summary>
        /// Constructing for creating the Controller
        /// </summary>
        /// <param name="logManager"></param>
        /// <param name="deleteNodeManager"></param>
        /// <param name="messageBank"></param>
        public DeleteNodeController(ILogManager logManager, IDeleteNodeManager deleteNodeManager, IMessageBank messageBank)
        {
            _logManager = logManager;
            _deleteNodeManager = deleteNodeManager;
            _messageBank = messageBank;
        }

        /// <summary>
        /// Entry point for Node Delete requests that forwards the given userhash, nodeID, and nodeParentID, to the DeleteNodeManager for the opration to be performed.
        /// </summary>
        /// <param name="node.UserHash"></param>
        /// <param name="node.NodeID"></param>
        /// <param name="node.ParentNodeID"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("deleteNode")]
        //public async Task<ActionResult<string>> DeleteNodeAsync(string node.UserHash, long node.NodeID, long node.ParentNodeID)
        public async Task<ActionResult<string>> DeleteNodeAsync(Node node)
        {
            StringBuilder stringBuilder = new StringBuilder();
            try
            {
                CancellationToken cancellationToken = new CancellationToken();
                IResponse<string> response = await _deleteNodeManager.DeleteNodeAsync(node.UserHash, node.NodeID, node.ParentNodeID, cancellationToken).ConfigureAwait(false);
                // Check that the response data is not null as well as the result of the operation
                if (response.Data != null && response.IsSuccess && response.StatusCode == 200)
                {
                    // Check if time was exceeded
                    if (response.ErrorMessage.Equals(""))
                    {
                        await _logManager.StoreAnalyticLogAsync(DateTime.UtcNow, ILogManager.Levels.Info, ILogManager.Categories.Server,
                            stringBuilder.ToString());
                    }
                    else
                    {
                        await _logManager.StoreAnalyticLogAsync(DateTime.UtcNow, ILogManager.Levels.Error, ILogManager.Categories.Server,
                            response.ErrorMessage);
                    }
                    return response.Data;
                }
                else if (response.StatusCode >= 500)
                {
                    stringBuilder.AppendFormat(response.ErrorMessage, node.UserHash, node.NodeID, node.ParentNodeID);
                    await _logManager.StoreAnalyticLogAsync(DateTime.UtcNow, ILogManager.Levels.Error, ILogManager.Categories.Data,
                        stringBuilder.ToString());
                    return StatusCode(response.StatusCode, response.ErrorMessage);
                }
                else
                {
                    stringBuilder.AppendFormat(response.ErrorMessage, node.UserHash, node.NodeID, node.ParentNodeID);
                    await _logManager.StoreAnalyticLogAsync(DateTime.UtcNow, ILogManager.Levels.Error, ILogManager.Categories.Data,
                        stringBuilder.ToString());
                    return new BadRequestObjectResult(response.ErrorMessage) { StatusCode = response.StatusCode };
                }
            }
            catch (Exception ex)
            {
                stringBuilder.AppendFormat(await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false),
                    ex.Message, stringBuilder.AppendFormat("node.UserHash: {0}, node.NodeID: {1}, node.ParentNodeID: {2}",
                    node.UserHash, node.NodeID, node.ParentNodeID).ToString());
                await _logManager.StoreAnalyticLogAsync(DateTime.UtcNow, ILogManager.Levels.Error, ILogManager.Categories.Server,
                    stringBuilder.ToString());
                return BadRequest();
            }
        }
    }
}
