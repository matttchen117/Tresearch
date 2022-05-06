using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
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

        [HttpPost("rateNode")]
        public async Task<IActionResult> RateNodeAsync(List<long> nodeIDs, int rating)
        {
            try
            {
                if (!Thread.CurrentPrincipal.Identity.Name.Equals("guest"))
                {
                    IResponse<int> result = await _rateManager.RateNodeAsync(nodeIDs, rating);
                    
                    if(result.StatusCode == 200)
                    {
                        _logManager.StoreAnalyticLogAsync(DateTime.Now, ILogManager.Levels.Info, ILogManager.Categories.Server, "Rate: Account rated.");
                        return new OkObjectResult(result.Data);
                    } else
                    {
                        string[] split;
                        split = result.ErrorMessage.Split(":");
                        Enum.TryParse(split[1], out ILogManager.Categories category);
                        _logManager.StoreArchiveLogAsync(DateTime.Now, ILogManager.Levels.Error, category, split[2]);
                        return StatusCode(Convert.ToInt32(split[0]), split[2]);
                    }
                    
                }
                else
                {
                    string errorResult = await _messageBank.GetMessage(IMessageBank.Responses.notAuthenticated);
                    string[] errorSplit;
                    errorSplit = errorResult.Split(":");
                    Enum.TryParse(errorSplit[0], out ILogManager.Categories category);
                    _logManager.StoreArchiveLogAsync(DateTime.Now.ToUniversalTime(), ILogManager.Levels.Error, category, errorSplit[2]);
                    return StatusCode(Convert.ToInt32(errorSplit[0]), errorSplit[2]);
                }
            }
            catch (Exception ex)
            {
                string errorResult = await _messageBank.GetMessage(IMessageBank.Responses.unhandledException) + ex.Message;
                string[] errorSplit;
                errorSplit = errorResult.Split(":");
                Enum.TryParse(errorSplit[0], out ILogManager.Categories category);
                _logManager.StoreArchiveLogAsync(DateTime.Now.ToUniversalTime(), ILogManager.Levels.Error, category, errorSplit[2]);
                return StatusCode(Convert.ToInt32(errorSplit[0]), errorSplit[2]);
            }
        }
        [HttpPost("getRating")]
        public async Task<IActionResult> GetNodeRatingAsync(List<long> nodeIDs)
        {
            try
            {
                IResponse<IEnumerable<Node>> results = await _rateManager.GetNodeRatingAsync(nodeIDs);

                if (results.StatusCode == 200)
                {
                    _logManager.StoreAnalyticLogAsync(DateTime.Now, ILogManager.Levels.Info, ILogManager.Categories.Server, "Rating: Ratings retrieved.");
                    return new OkObjectResult(results.Data);
                }
                else
                {
                    string[] split;
                    split = results.ErrorMessage.Split(":");
                    Enum.TryParse(split[1], out ILogManager.Categories category);
                    _logManager.StoreArchiveLogAsync(DateTime.Now, ILogManager.Levels.Error, category, split[2]);
                    return StatusCode(Convert.ToInt32(split[0]), split[2]);
                }
            }
            catch (Exception ex)
            {
                string errorResult = await _messageBank.GetMessage(IMessageBank.Responses.unhandledException) + ex.Message;
                string[] errorSplit;
                errorSplit = errorResult.Split(":");
                Enum.TryParse(errorSplit[0], out ILogManager.Categories category);
                _logManager.StoreArchiveLogAsync(DateTime.Now.ToUniversalTime(), ILogManager.Levels.Error, category, errorSplit[2]);
                return StatusCode(Convert.ToInt32(errorSplit[0]), ex.Message);
            }
        }

        [HttpPost("getUserNodeRating")]
        public async Task<IActionResult> GetUserNodeRatingAsync(long nodeID)
        {
            try
            {
                IResponse<int> results = await _rateManager.GetUserNodeRatingAsync(nodeID);

                if (results.StatusCode == 200)
                {
                    _logManager.StoreAnalyticLogAsync(DateTime.Now, ILogManager.Levels.Info, ILogManager.Categories.Server, "Rating: Ratings retrieved.");
                    return new OkObjectResult(results.Data);
                }
                else
                {
                    string[] split;
                    split = results.ErrorMessage.Split(":");
                    Enum.TryParse(split[1], out ILogManager.Categories category);
                    _logManager.StoreArchiveLogAsync(DateTime.Now, ILogManager.Levels.Error, category, split[2]);
                    return StatusCode(Convert.ToInt32(split[0]), split[2]);
                }
            }
            catch (Exception ex)
            {
                string errorResult = await _messageBank.GetMessage(IMessageBank.Responses.unhandledException) + ex.Message;
                string[] errorSplit;
                errorSplit = errorResult.Split(":");
                Enum.TryParse(errorSplit[0], out ILogManager.Categories category);
                _logManager.StoreArchiveLogAsync(DateTime.Now.ToUniversalTime(), ILogManager.Levels.Error, category, errorSplit[2]);
                return StatusCode(Convert.ToInt32(errorSplit[0]), ex.Message);
            }
        }


    }
}
