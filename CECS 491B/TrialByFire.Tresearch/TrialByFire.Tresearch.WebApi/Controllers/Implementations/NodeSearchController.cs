using Microsoft.AspNetCore.Mvc;
using System.Web.Http.Results;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.WebApi.Controllers.Contracts;

namespace TrialByFire.Tresearch.WebApi.Controllers.Implementations
{
    public class NodeSearchController : ControllerBase, INodeSearchController
    {
        private ILogManager _logManager;
        private INodeSearchManager _nodeSearchManager;
        private IMessageBank _messageBank;
        public NodeSearchController(ILogManager logManager, INodeSearchManager nodeSearchManager, 
            IMessageBank messageBank)
        {
            _logManager = logManager;
            _nodeSearchManager = nodeSearchManager;
            _messageBank = messageBank;
        }

        public async Task<ActionResult<IEnumerable<Node>>> SearchForNodeAsync(ISearchInput searchInput)
        {
            try
            {
                IResponse<IList<Node>> response = await _nodeSearchManager.SearchForNodeAsync(searchInput).ConfigureAwait(false);
                if(response.IsSuccess && response.StatusCode == 200 && response.ErrorMessage.Equals(""))
                {
                    return new OkObjectResult(response.Data);
                }
                else if (response.StatusCode >= 500)
                {
                    return StatusCode(response.StatusCode, response.ErrorMessage);
                }
                else
                {
                    return new BadRequestObjectResult(response.ErrorMessage) { StatusCode = response.StatusCode };
                }
            }
            catch(Exception ex)
            {
                string message = await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message;
                await _logManager.StoreArchiveLogAsync(DateTime.UtcNow, ILogManager.Levels.Error, ILogManager.Categories.Server,
                    message);
                return new BadRequestObjectResult(message);
            }
        }
    }
}
