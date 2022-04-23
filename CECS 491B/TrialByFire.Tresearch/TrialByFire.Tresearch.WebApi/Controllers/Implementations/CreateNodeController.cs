using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Managers.Implementations;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.WebApi.Controllers.Contracts;

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
        private ISqlDAO _sqlDAO { get; }
        private ILogService _logService { get; }
        private ICreateNodeManager _createNodeManager { get; }
        private IMessageBank _messageBank { get; }
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        /// <summary>
        /// Constructing for creating the Controller
        /// </summary>
        /// <param name="sqlDAO"></param>
        /// <param name="logService"></param>
        /// <param name="createNodeManager"></param>
        /// <param name="messageBank"></param>
        public CreateNodeController(ISqlDAO sqlDAO, ILogService logService, 
            ICreateNodeManager createNodeManager, IMessageBank messageBank)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
            _createNodeManager = createNodeManager;
            _messageBank = messageBank;
        }

        /// <summary>
        /// Entry point for node creation requests that forwards the given input to the CreateNodeManager for the opration to be performed.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("createNode")]
        public async Task<IActionResult> CreateNodeAsync(System.Collections.ArrayList paramList)
        {
            try
            {
                string[] split;
                IAccount account = Newtonsoft.Json.JsonConvert.DeserializeObject<IAccount>(paramList[0].ToString());
                INode node = Newtonsoft.Json.JsonConvert.DeserializeObject<INode>(paramList[1].ToString());
                string result = await _createNodeManager.CreateNodeAsync(account, node, _cancellationTokenSource.Token).ConfigureAwait(false);
                if (result.Equals(await _messageBank.GetMessage(IMessageBank.Responses.createNodeSuccess).ConfigureAwait(false)))
                {
                    split = result.Split(": ");
                    return new OkObjectResult(split[2]) { StatusCode = Convert.ToInt32(split[0]) };
                }
                split = result.Split(": ");
                return StatusCode(Convert.ToInt32(split[0]), split[2]);
            }
            catch (OperationCanceledException tce)
            {
                return StatusCode(400, tce.Message);
            }
            catch(Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }
    }
}