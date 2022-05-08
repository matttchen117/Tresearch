using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Models;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.WebApi.Controllers.Contracts;

namespace TrialByFire.Tresearch.WebApi.Controllers.Implementations
{
    [ApiController]
    [EnableCors]
    [Route("[Controller]")]
    public class RateController: ControllerBase, IRateController
    {
        // User tree data updated within 5 seconds, as per BRD
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        private ISqlDAO _sqlDAO { get; set; }
        private ILogManager _logManager { get; set; }
        private IMessageBank _messageBank { get; set; }
        private IRateManager _rateManager { get; set; }
        private BuildSettingsOptions _options { get; }
        public RateController(ISqlDAO sqlDAO, ILogManager logManager, IMessageBank messageBank, IRateManager rateManager, IOptionsSnapshot<BuildSettingsOptions> options)
        {
            _sqlDAO = sqlDAO;
            _logManager = logManager;
            _messageBank = messageBank;
            _rateManager = rateManager;
            _options = options.Value;
        }

        /// <summary>
        ///  Request to rates a list of node(s) with the same rating
        /// </summary>
        /// <param name="nodeIDs">List of Node IDs</param>
        /// <param name="rating">User Rating</param>
        /// <returns>Status Code and result</returns>
        [HttpPost("rateNode")]
        public async Task<IActionResult> RateNodeAsync(List<long> nodeIDs, int rating)
        {
            try
            {
                IResponse<int> results;

                // Check if user identity is known
                if (Thread.CurrentPrincipal == null)
                {
                    results = new RateResponse<int>(await _messageBank.GetMessage(IMessageBank.Responses.notAuthenticated).ConfigureAwait(false), rating, 401, false);

                }
                // Validate input
                else if (nodeIDs == null || nodeIDs.Count() == 0)
                {
                    results = new RateResponse<int>(await _messageBank.GetMessage(IMessageBank.Responses.nodeNotFound).ConfigureAwait(false), rating, 404, false);
                }
                else
                {
                    results = await _rateManager.RateNodeAsync(nodeIDs, rating);
                }

                if (results.Data != null && results.Data > 0 && results.IsSuccess && results.StatusCode == 200)
                {
                    await CreateAnalyticLog(await _messageBank.GetMessage(IMessageBank.Responses.userRateSuccess).ConfigureAwait(false));
                    return new OkObjectResult(results.Data);
                }
                else
                {
                    string[] errorMessage = await CreateArchiveLogAsync(results.ErrorMessage);
                    return StatusCode(Convert.ToInt32(errorMessage[0]), errorMessage[2]);
                }

            }
            catch (Exception ex)
            {
                string errorResult = await _messageBank.GetMessage(IMessageBank.Responses.unhandledException) + ex.Message;
                string[] errorMessage = await CreateArchiveLogAsync(errorResult);
                return StatusCode(Convert.ToInt32(errorMessage[0]), errorMessage[2]);
            }
        }

        /// <summary>
        ///  Returns a list of nodes containing matching IDs and their ratings.
        /// </summary>
        /// <param name="nodeIDs">List of Node IDs</param>
        /// <returns>Corresponding Ratings</returns>
        [HttpPost("getRating")]
        public async Task<IActionResult> GetNodeRatingAsync(List<long> nodeIDs)
        {
            try
            {
                IResponse<IEnumerable<Node>> results;

                // Check if user identity is known
                if (Thread.CurrentPrincipal == null)
                {
                    results = new RateResponse<IEnumerable<Node>>(await _messageBank.GetMessage(IMessageBank.Responses.notAuthenticated).ConfigureAwait(false), new List<Node>(), 401, false);

                }
                // Validate input
                else if (nodeIDs == null || nodeIDs.Count() == 0)
                {
                    results = new RateResponse<IEnumerable<Node>>(await _messageBank.GetMessage(IMessageBank.Responses.nodeNotFound).ConfigureAwait(false), new List<Node>(), 404, false);
                }
                else
                {
                    results = await _rateManager.GetNodeRatingAsync(nodeIDs, _cancellationTokenSource.Token);
                }

                if (results.Data != null && results.Data.Count() != 0 && results.IsSuccess && results.StatusCode == 200)
                {
                    await CreateAnalyticLog(await _messageBank.GetMessage(IMessageBank.Responses.getRateSuccess).ConfigureAwait(false));
                    return new OkObjectResult(results.Data);
                }
                else
                {
                    string[] errorMessage = await CreateArchiveLogAsync(results.ErrorMessage);
                    return StatusCode(Convert.ToInt32(errorMessage[0]), errorMessage[2]);
                }
            }
            catch (Exception ex)
            {
                string errorResult = await _messageBank.GetMessage(IMessageBank.Responses.unhandledException) + ex.Message;
                string[] errorMessage = await CreateArchiveLogAsync(errorResult);
                return StatusCode(Convert.ToInt32(errorMessage[0]), errorMessage[2]);
            }
        }

        /// <summary>
        ///     Post request to return a user's rating of a node
        /// </summary>
        /// <param name="nodeID">Node ID</param>
        /// <returns>Integer rating of node</returns>
        [HttpPost("getUserNodeRating")]
        public async Task<IActionResult> GetUserNodeRatingAsync(long nodeID)
        {
            try
            {
                IResponse<int> results;

                // Check if user identity is known
                if(Thread.CurrentPrincipal == null)
                {
                    results = new RateResponse<int>(await _messageBank.GetMessage(IMessageBank.Responses.notAuthenticated).ConfigureAwait(false), 0, 401, false);

                }
                // Validate input
                else if(nodeID <= 0)
                {
                    results = new RateResponse<int>(await _messageBank.GetMessage(IMessageBank.Responses.nodeNotFound).ConfigureAwait(false), 0, 404, false);
                }
                else
                {
                    results = await _rateManager.GetUserNodeRatingAsync(nodeID, _cancellationTokenSource.Token);
                }              

                if(results.Data != null && results.IsSuccess && results.StatusCode == 200)
                {
                    await CreateAnalyticLog(await _messageBank.GetMessage(IMessageBank.Responses.getRateSuccess).ConfigureAwait(false));
                    return new OkObjectResult(results.Data);
                }
                else
                {
                    string[] errorMessage = await CreateArchiveLogAsync(results.ErrorMessage);
                    return StatusCode(Convert.ToInt32(errorMessage[0]), errorMessage[2]);
                }
            }
            catch (Exception ex)
            {
                string errorResult = await _messageBank.GetMessage(IMessageBank.Responses.unhandledException) + ex.Message;
                string[] errorMessage = await CreateArchiveLogAsync(errorResult);
                return StatusCode(Convert.ToInt32(errorMessage[0]), errorMessage[2]);
            }
        }

        private async Task<string[]> CreateArchiveLogAsync(string error)
        {
            string[] split;
            split = error.Split(":");
            Enum.TryParse(split[1], out ILogManager.Categories category);
            await _logManager.StoreArchiveLogAsync(DateTime.Now, ILogManager.Levels.Error, category, split[2]);
            return split;
        }

        private async Task<string> CreateAnalyticLog(string success)
        {
            string[] successSplit;
            successSplit = success.Split(":");
            await _logManager.StoreAnalyticLogAsync(DateTime.Now, ILogManager.Levels.Info, ILogManager.Categories.Server, successSplit[2]);
            return successSplit[2];
        }
    }
}
