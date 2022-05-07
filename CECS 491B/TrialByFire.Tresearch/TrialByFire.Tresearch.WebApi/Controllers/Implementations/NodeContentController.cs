using Microsoft.AspNetCore.Mvc;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.WebApi.Controllers.Contracts;

namespace TrialByFire.Tresearch.WebApi.Controllers.Implementations
{
    public class NodeContentController : ControllerBase, INodeContentController
    {
        private ILogManager _logManager;
        private INodeContentManager _nodeContentManager;
        private IMessageBank _messageBank;
        
        public NodeContentController(ILogManager logManager, INodeContentManager nodeContentManager, IMessageBank messageBank)
        {
            _logManager = logManager;
            _nodeContentManager = nodeContentManager;
            _messageBank = messageBank;
        }

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
            }catch (Exception ex)
            {
                await _logManager.StoreArchiveLogAsync(DateTime.UtcNow, ILogManager.Levels.Error, ILogManager.Categories.Server,
                    await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false));
                // 500 instead
                return StatusCode(500);
            }
        }
    }
}
