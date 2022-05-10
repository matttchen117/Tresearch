using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.WebApi.Controllers.Contracts;

namespace TrialByFire.Tresearch.WebApi.Controllers.Implementations
{
    /// <summary>
    ///     NodeContentController: Class that is part of the Controller abstraction layer that handles receiving and returning
    ///         HTTP response and requests for UpdateNodeContent operations
    /// </summary>
    [ApiController]
    [EnableCors]
    [Route("[controller]")]
    public class NodeContentController : ControllerBase, INodeContentController
    {
        private ILogManager _logManager;
        private INodeContentManager _nodeContentManager;
        private IMessageBank _messageBank;

        /// <summary>
        ///     public NodeContentController():
        ///         Constructor for NodeContentController class
        /// </summary>
        /// <param name="logManager">Manager object for Manager abstraction layer to handle business rules related to Logging</param>
        /// <param name="nodeContentManager">Manager object for Manager abstraction layer to handle business rules related to UpdateNodeContent</param>
        /// <param name="messageBank">Object that contains error and success messages</param>
        public NodeContentController(ILogManager logManager, INodeContentManager nodeContentManager, IMessageBank messageBank)
        {
            _logManager = logManager;
            _nodeContentManager = nodeContentManager;
            _messageBank = messageBank;
        }

        /// <summary>
        ///     UpdateNodeContentAsync: 
        ///         Async method that handles receiving HTTP requests for UpdateNodeContent operation and returning the results  
        /// </summary>
        /// <param name="owner">The owner of the node to edit</param>
        /// <param name="nodeID">The id of the node to edit</param>
        /// <param name="title">The new title to update</param>
        /// <param name="summary">The new summary to update</param>
        /// <returns>The result of the operation</returns>
        [HttpPost]
        [Route("update")]
        // Do object input, extensible
        public async Task<IActionResult> UpdateNodeContentAsync(string owner, long nodeID, string title, string summary)
        {
            try
            {
                INodeContentInput nodeContentInput = new NodeContentInput(owner, nodeID, title, summary);
                IResponse<string> response = await _nodeContentManager.UpdateNodeContentAsync(nodeContentInput).ConfigureAwait(false);
                
                // Check if data is not null and successful operation
                if (response.Data != null && response.IsSuccess && response.StatusCode == 200)
                {
                    // Check if time was exceeded
                    if (!response.ErrorMessage.Equals(""))
                    {
                        await _logManager.StoreArchiveLogAsync(DateTime.UtcNow, ILogManager.Levels.Error, ILogManager.Categories.Server,
                            response.ErrorMessage);
                        await _logManager.StoreAnalyticLogAsync(DateTime.UtcNow, ILogManager.Levels.Info, ILogManager.Categories.Server,
                            await _messageBank.GetMessage(IMessageBank.Responses.updateNodeContentSuccess).ConfigureAwait(false));
                    }

                    await _logManager.StoreAnalyticLogAsync(DateTime.UtcNow, ILogManager.Levels.Info, ILogManager.Categories.Server,
                            await _messageBank.GetMessage(IMessageBank.Responses.updateNodeContentSuccess).ConfigureAwait(false));
                    return new OkObjectResult(response.Data);
                }
                else if (response.StatusCode >= 500)
                {
                    await _logManager.StoreArchiveLogAsync(DateTime.UtcNow, ILogManager.Levels.Error, ILogManager.Categories.Server, response.ErrorMessage);
                    return StatusCode(response.StatusCode, response.ErrorMessage);
                }
                else
                {
                    await _logManager.StoreArchiveLogAsync(DateTime.UtcNow, ILogManager.Levels.Error, ILogManager.Categories.Data, response.ErrorMessage);
                    return new BadRequestObjectResult(response.ErrorMessage) { StatusCode = response.StatusCode };
                }
            }
            catch (Exception ex)
            {
                await _logManager.StoreArchiveLogAsync(DateTime.UtcNow, ILogManager.Levels.Error, ILogManager.Categories.Server,
                    await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false));
                return StatusCode(500);
            }
        }
    }
}
