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

    public class PrivateAndPublicController : ControllerBase, IPrivateAndPublicController
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

        private IPrivateAndPublicManager _privateAndPublicNodeManager { get; }


        /// <summary>
        ///     Model holding string responses to enumerated cases
        /// </summary>
        private IMessageBank _messageBank { get; set; }


        public PrivateAndPublicController(ILogManager logManager, IMessageBank messageBank, IPrivateAndPublicManager privateAndPublicNodeManager, IOptionsSnapshot<BuildSettingsOptions> buildSettingsOptions)
        {
            _logManager = logManager;
            _privateAndPublicNodeManager = privateAndPublicNodeManager;
            _messageBank = messageBank;
            _buildSettingsOptions = buildSettingsOptions.Value;
        }


        [HttpPost("Private")]
        public async Task<ActionResult<string>> PrivateNodeAsync(List<long> nodes)
        {
            StringBuilder stringBuilder = new StringBuilder();

            try
            {


                if (Thread.CurrentPrincipal == null || Thread.CurrentPrincipal.Identity.Name.Equals("guest"))
                {
                    // User identity not known, log error and return
                    string errorMessage = await _messageBank.GetMessage(IMessageBank.Responses.notAuthenticated).ConfigureAwait(false);
                    string[] errorSplit;
                    errorSplit = errorMessage.Split(":");
                    await _logManager.StoreArchiveLogAsync(DateTime.UtcNow, ILogManager.Levels.Error, ILogManager.Categories.Server, errorSplit[2]).ConfigureAwait(false);
                    return new BadRequestObjectResult(errorSplit[2]);
                }


                IResponse<string> response = await _privateAndPublicNodeManager.PrivateNodeAsync(nodes).ConfigureAwait(false);

                // Checks if response  
                if (response.Data != null && response.IsSuccess && response.StatusCode == 200)
                {
                    if (response.ErrorMessage.Equals(""))
                    {
                        //building the string here with correct message
                        stringBuilder.AppendFormat(await _messageBank.GetMessage(IMessageBank.Responses.privateNodeSuccess).ConfigureAwait(false));

                        //logging here
                        await _logManager.StoreArchiveLogAsync(DateTime.UtcNow, ILogManager.Levels.Info, ILogManager.Categories.Server, stringBuilder.ToString());
                    }
                    else
                    {
                        await _logManager.StoreAnalyticLogAsync(DateTime.UtcNow, ILogManager.Levels.Error, ILogManager.Categories.Server, response.ErrorMessage);
                    }
                    return response.Data;
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




        /*

        [HttpPost("Public")]
        public async Task<ActionResult<string>> PublicNodeAsync(List<long> nodes)
        {
            StringBuilder stringBuilder = new StringBuilder();

            try
            {


                if (Thread.CurrentPrincipal == null || Thread.CurrentPrincipal.Identity.Name.Equals("guest"))
                {
                    // User identity not known, log error and return
                    string errorMessage = await _messageBank.GetMessage(IMessageBank.Responses.notAuthenticated).ConfigureAwait(false);
                    string[] errorSplit;
                    errorSplit = errorMessage.Split(":");
                    await _logManager.StoreArchiveLogAsync(DateTime.UtcNow, ILogManager.Levels.Error, ILogManager.Categories.Server, errorSplit[2]).ConfigureAwait(false);
                    return new BadRequestObjectResult(errorSplit[2]);
                }


                IResponse<string> response = await _privateAndPublicNodeManager.PublicNodeAsync(nodes).ConfigureAwait(false);

                // Checks if response  
                if (response.Data != null && response.IsSuccess && response.StatusCode == 200)
                {
                    if (response.ErrorMessage.Equals(""))
                    {
                        //building the string here with correct message
                        stringBuilder.AppendFormat(await _messageBank.GetMessage(IMessageBank.Responses.publicNodeSuccess).ConfigureAwait(false));

                        //logging here
                        await _logManager.StoreArchiveLogAsync(DateTime.UtcNow, ILogManager.Levels.Info, ILogManager.Categories.Server, stringBuilder.ToString());
                    }
                    else
                    {
                        await _logManager.StoreAnalyticLogAsync(DateTime.UtcNow, ILogManager.Levels.Error, ILogManager.Categories.Server, response.ErrorMessage);
                    }
                    return response.Data;
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

        */



    }
}
