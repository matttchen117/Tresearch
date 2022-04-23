using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Models;
using TrialByFire.Tresearch.Models.Contracts;
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

        [HttpGet]
        [Route("rateNode")]
        public async Task<IActionResult> RateNodeAsync(long nodeID, int rating)
        {
            try
            {
                if (!Thread.CurrentPrincipal.Identity.Name.Equals("guest"))
                {
                    string result = await _rateManager.RateNodeAsync(nodeID, rating, _cancellationTokenSource.Token);
                    string[] split;
                    split = result.Split(":");

                    if (result.Equals(await _messageBank.GetMessage(IMessageBank.Responses.generic)))
                    {
                        _logManager.StoreAnalyticLogAsync(DateTime.Now, ILogManager.Levels.Info, ILogManager.Categories.Server, "Rate: Account rated.");
                    }
                    else
                    {
                        Enum.TryParse(split[1], out ILogManager.Categories category);
                        _logManager.StoreArchiveLogAsync(DateTime.Now, ILogManager.Levels.Error, category, split[2]);
                    }
                    return StatusCode(Convert.ToInt32(split[0]), split[2]);
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

        [HttpGet]
        [Route("getRating")]
        public async Task<IActionResult> GetNodeRatingAsync(List<long> nodeIDs)
        {
            try
            {
                if (!Thread.CurrentPrincipal.Identity.Name.Equals("guest"))
                {
                    Tuple<List<double>, string> results = await _rateManager.GetNodeRatingAsync(nodeIDs, _cancellationTokenSource.Token);
                    string result = results.Item2;

                    string[] split;
                    split = result.Split(":");

                    if (result.Equals(await _messageBank.GetMessage(IMessageBank.Responses.generic)))
                    {
                        _logManager.StoreAnalyticLogAsync(DateTime.Now, ILogManager.Levels.Info, ILogManager.Categories.Server, "Rating: Ratings retrieved.");
                        return StatusCode(Convert.ToInt32(split[0]), results.Item1);
                    }
                    else
                    {
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


    }
}
