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
        [Route("CopyAndPaste")]
        
        public async Task<ActionResult<IEnumerable<Node>>> CopyNodeAsync(List<long> nodesCopy)
        {

            StringBuilder stringBuilder = new StringBuilder();

            try
            {

                

                // Check if user identity is unknown
                if (Thread.CurrentPrincipal == null || Thread.CurrentPrincipal.Identity.Name.Equals("guest"))
                {
                    // User identity not known, log error and return
                    string errorMessage = await _messageBank.GetMessage(IMessageBank.Responses.notAuthenticated).ConfigureAwait(false);
                    string[] errorSplit;
                    errorSplit = errorMessage.Split(":");
                    await _logManager.StoreArchiveLogAsync(DateTime.UtcNow, ILogManager.Levels.Error, ILogManager.Categories.Server, errorSplit[2]).ConfigureAwait(false);
                    return new BadRequestObjectResult(errorSplit[2]);
                }

                IResponse<IEnumerable<Node>> response = await _copyAndPasteManager.CopyNodeAsync(nodesCopy).ConfigureAwait(false);

                if (response.Data != null && response.IsSuccess && response.StatusCode == 200)
                {
                    if (response.ErrorMessage.Equals(""))
                    {
                        //figure this out later
                        stringBuilder.AppendFormat(await _messageBank.GetMessage(IMessageBank.Responses.copyNodeSuccess).ConfigureAwait(false));
                        await _logManager.StoreArchiveLogAsync(DateTime.UtcNow, ILogManager.Levels.Info, ILogManager.Categories.Server, stringBuilder.ToString());
                    }
                    else
                    {
                        await _logManager.StoreAnalyticLogAsync(DateTime.UtcNow, ILogManager.Levels.Error, ILogManager.Categories.Server, response.ErrorMessage);
                    }
                    return response.Data.ToList();
                }
                else if (response.StatusCode >= 500)
                {
                    stringBuilder.AppendFormat(response.ErrorMessage);
                    await _logManager.StoreAnalyticLogAsync(DateTime.UtcNow, ILogManager.Levels.Error, ILogManager.Categories.Server, stringBuilder.ToString());
                    return StatusCode(response.StatusCode, response.ErrorMessage);
                }
                else
                {
                    stringBuilder.AppendFormat(response.ErrorMessage);
                    await _logManager.StoreAnalyticLogAsync(DateTime.UtcNow, ILogManager.Levels.Error, ILogManager.Categories.Data, stringBuilder.ToString());
                    return new BadRequestObjectResult(response.ErrorMessage) { StatusCode = response.StatusCode };
                }




                /*
                if (nodesCopy == null || nodesCopy.Count <= 0)
                {
                    result = await _messageBank.GetMessage(IMessageBank.Responses.copyNodeEmptyError).ConfigureAwait(false);
                }

                result = await _copyAndPasteManager.CopyNodeAsync(nodesCopy).ConfigureAwait(false);


                // Splitting string for logging
                string[] split;
                split = result.Split(":");

                if(!result.Equals(await _messageBank.GetMessage(IMessageBank.Responses.copyNodeSuccess)))
                {
                    Enum.TryParse(split[1], out ILogManager.Categories category);
                    await _logManager.StoreArchiveLogAsync(DateTime.UtcNow, ILogManager.Levels.Error, category, split[2]).ConfigureAwait(false);
                    return StatusCode(Convert.ToInt32(split[0]), split[2]);

                }

                await _logManager.StoreAnalyticLogAsync(DateTime.Now.ToUniversalTime(), ILogManager.Levels.Info, ILogManager.Categories.Server, split[2]).ConfigureAwait(false);
                return new OkObjectResult(split[2]);

                */

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
                stringBuilder.AppendFormat(await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false), ex.Message);
                await _logManager.StoreArchiveLogAsync(DateTime.UtcNow, ILogManager.Levels.Error, ILogManager.Categories.Server, stringBuilder.ToString());
                return BadRequest();


                /*
                
                string errorMessage = await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message;
                string[] split;
                split = errorMessage.Split(":");
                await _logManager.StoreArchiveLogAsync(DateTime.UtcNow, ILogManager.Levels.Error, ILogManager.Categories.Server, split[2]).ConfigureAwait(false);
                return new BadRequestObjectResult(split[2]);

                */


            }
        }




        /*
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

        */
    }
}
