using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Web.Http.Results;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.WebApi.Controllers.Contracts;

namespace TrialByFire.Tresearch.WebApi.Controllers.Implementations
{
    [ApiController]
    [EnableCors]
    [Route("[controller]")]
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

        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<IEnumerable<Node>>> SearchForNodeAsync(string search, [FromQuery]IEnumerable<string> tags, bool filterByRating, 
            bool filterByTime)
        {
            StringBuilder stringBuilder = new StringBuilder();
            try
            {
                ISearchInput searchInput = new SearchInput(search, tags, filterByRating, filterByTime);
                IResponse<IEnumerable<Node>> response = await _nodeSearchManager.SearchForNodeAsync(searchInput).ConfigureAwait(false);
                if(response.IsSuccess && response.StatusCode == 200 && response.ErrorMessage.Equals(""))
                {
                    stringBuilder.AppendFormat(await _messageBank.GetMessage(IMessageBank.Responses.nodeSearchSuccess).ConfigureAwait(false), search,
                        string.Join(",", tags), filterByRating, filterByTime);
                    await _logManager.StoreAnalyticLogAsync(DateTime.UtcNow, ILogManager.Levels.Info, ILogManager.Categories.Server,
                        stringBuilder.ToString());
                    return response.Data.ToList();
                }
                else if (response.StatusCode >= 500)
                {
                    stringBuilder.AppendFormat(response.ErrorMessage, search, string.Join(",", tags), filterByRating, filterByTime);
                    await _logManager.StoreAnalyticLogAsync(DateTime.UtcNow, ILogManager.Levels.Error, ILogManager.Categories.Server,
                        stringBuilder.ToString());
                    return StatusCode(response.StatusCode, response.ErrorMessage);
                }
                else
                {
                    stringBuilder.AppendFormat(response.ErrorMessage, search, string.Join(",", tags), filterByRating, filterByTime);
                    await _logManager.StoreAnalyticLogAsync(DateTime.UtcNow, ILogManager.Levels.Error, ILogManager.Categories.Data,
                        stringBuilder.ToString());
                    return new BadRequestObjectResult(response.ErrorMessage) { StatusCode = response.StatusCode };
                }
            }
            catch(Exception ex)
            {
                stringBuilder.AppendFormat(await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false),
                    ex.Message, stringBuilder.AppendFormat("Search: {0}, Tags: {1}, FilterByRating: {2}, FilterByTime: {3}", 
                    search, string.Join(",", tags), filterByRating, filterByTime).ToString());
                await _logManager.StoreAnalyticLogAsync(DateTime.UtcNow, ILogManager.Levels.Error, ILogManager.Categories.Server,
                    stringBuilder.ToString());
                return BadRequest();
            }
        }
    }
}
