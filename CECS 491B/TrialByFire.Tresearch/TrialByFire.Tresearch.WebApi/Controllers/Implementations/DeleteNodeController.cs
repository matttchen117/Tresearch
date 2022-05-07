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
    /// Controller class for deleting Nodes.
    /// </summary>
    [ApiController]
    [EnableCors]
    [Route("[controller]")]
    public class DeleteNodeController : Controller, IDeleteNodeController
    {
        private ISqlDAO _sqlDAO { get; }
        private ILogService _logService { get; }
        private IDeleteNodeManager _deleteNodeManager { get; }
        private IMessageBank _messageBank { get; }  
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        /// <summary>
        /// Constructing for creating the Controller
        /// </summary>
        /// <param name="sqlDAO"></param>
        /// <param name="logService"></param>
        /// <param name="deleteNodeManager"></param>
        /// <param name="messageBank"></param>
        public DeleteNodeController(ISqlDAO sqlDAO, ILogService logService,
            IDeleteNodeManager deleteNodeManager, IMessageBank messageBank)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
            _deleteNodeManager = deleteNodeManager;
            _messageBank = messageBank;
        }

        /// <summary>
        /// Entry point for node delete requests that forwards the given NodeID to the DeleteNodeManager for the opration to be performed.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="nodeID"></param>
        /// <param name="parentID"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("deleteNode")]
        public async Task<IActionResult> DeleteNodeAsync(IAccount account, long nodeID, long parentID)
        {
            try
            {
                string[] split;
                string result = await _deleteNodeManager.DeleteNodeAsync(account, nodeID, parentID, _cancellationTokenSource.Token).ConfigureAwait(false);
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
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }
    }
}
