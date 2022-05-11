using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Managers.Implementations;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.WebApi.Controllers.Contracts;
using System.Text;

namespace TrialByFire.Tresearch.WebApi.Controllers.Implementations
{
    /// <summary>
    /// Controller class for creating Nodes.
    /// </summary>
    [ApiController]
    [EnableCors]
    [Route("[controller]")]
    public class CreateNodeController : Controller, ICreateNodeController
    {
        private ILogManager _logManager { get; }
        private ICreateNodeManager _createNodeManager { get; }
        private IMessageBank _messageBank { get; }
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        /// <summary>
        /// Constructing for creating the Controller
        /// </summary>
        /// <param name="logManager"></param>
        /// <param name="createNodeManager"></param>
        /// <param name="messageBank"></param>
        public CreateNodeController(ILogManager logManager, ICreateNodeManager createNodeManager, IMessageBank messageBank)
        {
            _logManager = logManager;
            _createNodeManager = createNodeManager;
            _messageBank = messageBank;
        }
        
        /// <summary>
        /// Entry point for node creation requests that forwards the given input to the CreateNodeManager for the opration to be performed.
        /// </summary>
        /// <param name="userhash"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("createNode")]
        public async Task<ActionResult<string>> CreateNodeAsync(string userhash, long parentNodeID, string nodeTitle, string summary)
        {
            StringBuilder stringBuilder = new StringBuilder();
            try
            {
                Node node = new Node(userhash, 0, parentNodeID, nodeTitle, summary, DateTime.UtcNow, true, false);
                CancellationToken cancellationToken = new CancellationToken();
                IResponse<string> response = await _createNodeManager.CreateNodeAsync(userhash, node, cancellationToken).ConfigureAwait(false);
                if (response.Data != null && response.IsSuccess && response.StatusCode == 200)
                {
                    // Check if time was exceeded
                    if (response.ErrorMessage.Equals(""))
                    {
                        stringBuilder.AppendFormat(await _messageBank.GetMessage(IMessageBank.Responses.createNodeSuccess).ConfigureAwait(false));
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
                    stringBuilder.AppendFormat(response.ErrorMessage, userhash, node.ToString());
                    await _logManager.StoreAnalyticLogAsync(DateTime.UtcNow, ILogManager.Levels.Error, ILogManager.Categories.Data,
                        stringBuilder.ToString());
                    return StatusCode(response.StatusCode, response.ErrorMessage);
                }
                else
                {
                    stringBuilder.AppendFormat(response.ErrorMessage, userhash, node.ToString());
                    await _logManager.StoreAnalyticLogAsync(DateTime.UtcNow, ILogManager.Levels.Error, ILogManager.Categories.Data,
                        stringBuilder.ToString());
                    return new BadRequestObjectResult(response.ErrorMessage) { StatusCode = response.StatusCode };
                }
            }
            catch (Exception ex)
            {
                stringBuilder.AppendFormat(await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false),
                    ex.Message, stringBuilder.AppendFormat("UserHash: {0}",
                    userhash));
                await _logManager.StoreAnalyticLogAsync(DateTime.UtcNow, ILogManager.Levels.Error, ILogManager.Categories.Server,
                    stringBuilder.ToString());
                return BadRequest();
            }
        }
    }
}