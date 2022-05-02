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



        public CopyAndPasteController(ILogManager logManager, IMessageBank messageBank, ICopyAndPasteManager copyAndPasteManager, IOptionsSnapshot<BuildSettingsOptions> buildSettingsOptions)
        {
            _logManager = logManager;
            _copyAndPasteManager = copyAndPasteManager;
            _messageBank = messageBank;
            _buildSettingsOptions = buildSettingsOptions.Value;
        }



        /// <summary>
        ///     CopyNodeAsync:
        ///         Async method that grabs a list of nodes from the database given a list of nodeIDs from the front end
        /// </summary>
        /// <param name="nodesCopy">The list of nodeIDs that will be used to query for nodes</param>
        /// <returns></returns>
        [HttpPost("Copy")]
        public async Task<ActionResult<IEnumerable<Node>>> CopyNodeAsync(List<long> nodesCopy)
        {

            StringBuilder stringBuilder = new StringBuilder();

            try
            {

                // Check if the current thread principal is null or guest
                if (Thread.CurrentPrincipal == null || Thread.CurrentPrincipal.Identity.Name.Equals("guest"))
                {
                    // User identity not known, log error and return
                    string errorMessage = await _messageBank.GetMessage(IMessageBank.Responses.notAuthenticated).ConfigureAwait(false);
                    string[] errorSplit;
                    errorSplit = errorMessage.Split(":");
                    await _logManager.StoreArchiveLogAsync(DateTime.UtcNow, ILogManager.Levels.Error, ILogManager.Categories.Server, errorSplit[2]).ConfigureAwait(false);
                    return new BadRequestObjectResult(errorSplit[2]);
                }

                //getting response here by calling manager
                IResponse<IEnumerable<Node>> response = await _copyAndPasteManager.CopyNodeAsync(nodesCopy).ConfigureAwait(false);

                // Checks if response  
                if (response.Data != null && response.IsSuccess && response.StatusCode == 200)
                {
                    if (response.ErrorMessage.Equals(""))
                    {
                        //building the string here with correct message
                        stringBuilder.AppendFormat(await _messageBank.GetMessage(IMessageBank.Responses.copyNodeSuccess).ConfigureAwait(false));

                        //logging here
                        await _logManager.StoreArchiveLogAsync(DateTime.UtcNow, ILogManager.Levels.Info, ILogManager.Categories.Server, stringBuilder.ToString());
                    }
                    else
                    {
                        await _logManager.StoreAnalyticLogAsync(DateTime.UtcNow, ILogManager.Levels.Error, ILogManager.Categories.Server, response.ErrorMessage);
                    }
                    return response.Data.ToList();
                }

                // Checks if status code is greater than 500.
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


            }
        }



        

        [HttpPost("Paste")]
        public async Task<ActionResult<string>> PasteNodeAsync(long nodeIDToPasteTo, [FromQuery]List<INode> nodes)
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


                // Getting response here by calling Manager
                IResponse<string> response = await _copyAndPasteManager.PasteNodeAsync(nodeIDToPasteTo, nodes).ConfigureAwait(false);


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
                    return response.Data;
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
                stringBuilder.AppendFormat(await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false), ex.Message);
                await _logManager.StoreArchiveLogAsync(DateTime.UtcNow, ILogManager.Levels.Error, ILogManager.Categories.Server, stringBuilder.ToString());
                return BadRequest();


            }
        
            



        }
        

        
    }
}

