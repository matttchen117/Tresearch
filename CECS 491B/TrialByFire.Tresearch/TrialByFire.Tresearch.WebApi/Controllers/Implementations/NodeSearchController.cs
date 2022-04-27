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
    /// <summary>
    ///     NodeSearchController: Class that is part of the Controller abstraction layer that handles receiving and returning
    ///         HTTP response and requests
    /// </summary>
    [ApiController]
    [EnableCors]
    [Route("[controller]")]
    public class NodeSearchController : ControllerBase, INodeSearchController
    {
        private ILogManager _logManager;
        private INodeSearchManager _nodeSearchManager;
        private IMessageBank _messageBank;
        /// <summary>
        ///     public NodeSearchController():
        ///         Constructor for NodeSearchController
        /// </summary>
        /// <param name="logManager">Manager object for Manager abstraction layer to handle business rules related to Logging</param>
        /// <param name="nodeSearchManager">Manager object for Manager abstraction layer to handle business rules related to Search</param>
        /// <param name="messageBank">Object that contains error and success messages</param>
        public NodeSearchController(ILogManager logManager, INodeSearchManager nodeSearchManager, 
            IMessageBank messageBank)
        {
            _logManager = logManager;
            _nodeSearchManager = nodeSearchManager;
            _messageBank = messageBank;
        }

        /// <summary>
        ///     SearchForNodeAsync: 
        ///         Async method that handles receiving HTTP requests for Search operation and returning the results
        /// </summary>
        /// <param name="search">The phrase that the User has input for the Search</param>
        /// <param name="tags">The tags that the User has selected to fitler by</param>
        /// <param name="filterByRating">Whether or not the User wants the results specifically to be filtered by rating</param>
        /// <param name="filterByTime">Whether or not the User wants to results specifically to be filtered by time</param>
        /// <returns>Returns the Nodes gathered by the Search operation</returns>
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
                // Check if node data is not null and successful operation
                if (response.Data != null && response.IsSuccess && response.StatusCode == 200)
                {
                    // Check if time was exceeded
                    if (response.ErrorMessage.Equals(""))
                    {
                        stringBuilder.AppendFormat(await _messageBank.GetMessage(IMessageBank.Responses.nodeSearchSuccess).ConfigureAwait(false), search,
                        string.Join(",", tags), filterByRating, filterByTime);
                        await _logManager.StoreAnalyticLogAsync(DateTime.UtcNow, ILogManager.Levels.Info, ILogManager.Categories.Server,
                            stringBuilder.ToString());
                    }
                    else
                    {
                        await _logManager.StoreAnalyticLogAsync(DateTime.UtcNow, ILogManager.Levels.Error, ILogManager.Categories.Server,
                            response.ErrorMessage);
                    }
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
            catch (Exception ex)
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
