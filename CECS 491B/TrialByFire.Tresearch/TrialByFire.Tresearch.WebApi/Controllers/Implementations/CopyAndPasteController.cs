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
    public class CopyAndPasteController : ControllerBase, ICopyAndPasteController
    {
        /// <summary>
        /// Cancellation token throws when not updated within 5 seconds, as per BRD
        /// </summary>
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        //private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));

        private BuildSettingsOptions _buildSettingsOptions { get; }

        private ISqlDAO _sqlDAO { get; }

        /// <summary>
        ///     Manager to perform logging for error and success cases
        /// </summary>
        private ILogManager _logManager { get; }


        /// <summary>
        ///     Manager to perform copy and paste feature feature
        /// </summary>

        private ICopyAndPasteManager _copyAndPasteManager { get; }


        /// <summary>
        ///     Model holding string responses to enumerated cases
        /// </summary>
        private IMessageBank _messageBank { get; set; }



        public CopyAndPasteController(ISqlDAO sqlDAO, ILogManager logManager, IMessageBank messageBank, ICopyAndPasteManager copyAndPasteManager, IOptionsSnapshot<BuildSettingsOptions> buildSettingsOptions)
        {
            _sqlDAO = sqlDAO;
            _logManager = logManager;
            _copyAndPasteManager = copyAndPasteManager;
            _messageBank = messageBank;
            _buildSettingsOptions = buildSettingsOptions.Value;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CopyNodeAsync(List<long> nodeIDs)
        {
            try
            {

                //grabbing list of nodes given list of nodeIDs
                Tuple<List<INode>, string> result = await _copyAndPasteManager.CopyNodeAsync(nodeIDs, _cancellationTokenSource.Token).ConfigureAwait(false);

                // Splitting string for logging
                string[] split;
                split = result.Item2.Split(":");


                if(!result.Item2.Equals(await _messageBank.GetMessage(IMessageBank.Responses.copyNodeSuccess)))
                {
                    Enum.TryParse(split[1], out ILogManager.Categories category);
                    await _logManager.StoreArchiveLogAsync(DateTime.UtcNow, ILogManager.Levels.Error, category, split[2]).ConfigureAwait(false);
                    return StatusCode(Convert.ToInt32(split[0]), result.Item1);

                }

                await _logManager.StoreAnalyticLogAsync(DateTime.Now.ToUniversalTime(), ILogManager.Levels.Info, ILogManager.Categories.Server, split[2]).ConfigureAwait(false);
                return new OkObjectResult(result.Item1);

                

            }
            catch(OperationCanceledException ex)
            {
                string errorMessage = await _messageBank.GetMessage(IMessageBank.Responses.operationCancelled).ConfigureAwait(false) + ex.Message;
                string[] split;

                split = errorMessage.Split(":");
                Enum.TryParse(split[1], out ILogManager.Categories category);
                await _logManager.StoreArchiveLogAsync(DateTime.UtcNow, ILogManager.Levels.Error, category, split[2]).ConfigureAwait(false);
                return StatusCode(Convert.ToInt32(split[0]), split[2]);
            }

            catch (Exception ex)
            {
                string errorMessage = await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message;
                string[] split;
                split = errorMessage.Split(":");
                await _logManager.StoreArchiveLogAsync(DateTime.UtcNow, ILogManager.Levels.Error, ILogManager.Categories.Server, split[2]).ConfigureAwait(false);
                return new BadRequestObjectResult(split[2]);
            }
        }





        public async Task<IActionResult> PasteNodeAsync(INode nodeToPasteTo, List<INode> nodes)
        {
            try
            {
                // beginning pasting list of nodes controller
                string result = await _copyAndPasteManager.PasteNodeAsync(nodeToPasteTo, nodes, _cancellationTokenSource.Token).ConfigureAwait(false);

                // Splitting string for logging
                string[] split;
                split = result.Split(":");
                string resultMessage = split[2];


                if (!resultMessage.Equals(await _messageBank.GetMessage(IMessageBank.Responses.copyNodeSuccess)))
                {
                    Enum.TryParse(split[1], out ILogManager.Categories category);
                    await _logManager.StoreArchiveLogAsync(DateTime.UtcNow, ILogManager.Levels.Error, category, split[2]).ConfigureAwait(false);
                    return StatusCode(Convert.ToInt32(split[0]));

                }

                await _logManager.StoreAnalyticLogAsync(DateTime.Now.ToUniversalTime(), ILogManager.Levels.Info, ILogManager.Categories.Server, split[2]).ConfigureAwait(false);
                return new OkObjectResult(result);

            }

            catch (OperationCanceledException ex)
            {
                string errorMessage = await _messageBank.GetMessage(IMessageBank.Responses.operationCancelled).ConfigureAwait(false) + ex.Message;
                string[] split;

                split = errorMessage.Split(":");
                Enum.TryParse(split[1], out ILogManager.Categories category);
                await _logManager.StoreArchiveLogAsync(DateTime.UtcNow, ILogManager.Levels.Error, category, split[2]).ConfigureAwait(false);
                return StatusCode(Convert.ToInt32(split[0]), split[2]);
            }

            catch (Exception ex)
            {
                string errorMessage = await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message;
                string[] split;
                split = errorMessage.Split(":");
                await _logManager.StoreArchiveLogAsync(DateTime.UtcNow, ILogManager.Levels.Error, ILogManager.Categories.Server, split[2]).ConfigureAwait(false);
                return new BadRequestObjectResult(split[2]);
            }



        }
    }
}
