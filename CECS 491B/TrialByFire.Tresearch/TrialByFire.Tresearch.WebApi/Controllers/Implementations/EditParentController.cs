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
    public class EditParentController : Controller, IEditParentController
    {
        private ILogManager _logManager;
        private IEditParentManager _editParentManager;
        private IMessageBank _messageBank;

        public EditParentController(ILogManager logManager, IEditParentManager editParentManager, IMessageBank messageBank)
        {
            _logManager = logManager;
            _editParentManager = editParentManager;
            _messageBank = messageBank;
        }

        [HttpPost]
        [Route("editParent")]

        public async Task<ActionResult<string>> EditParentNodeAsync(string userhash, long nodeID, string nodeIDs)
        {
            StringBuilder stringBuilder = new StringBuilder();
            try
            {
                CancellationToken cancellationToken = new CancellationToken();
                IResponse<string> response = await _editParentManager.EditParentNodeAsync(userhash, nodeID, nodeIDs, cancellationToken).ConfigureAwait(false);
                if(response.Data != null && response.IsSuccess && response.StatusCode == 200)
                {
                    // Check if time was exceeded
                    if (response.ErrorMessage.Equals(""))
                    {
                        await _logManager.StoreAnalyticLogAsync(DateTime.UtcNow, ILogManager.Levels.Info, ILogManager.Categories.Server,
                            stringBuilder.ToString());
                    }
                    else
                    {
                        await _logManager.StoreAnalyticLogAsync(DateTime.UtcNow, ILogManager.Levels.Error, ILogManager.Categories.Server,
                            response.ErrorMessage);
                    }
                    return response.Data;
                }
                else if(response.StatusCode >= 500)
                {
                    stringBuilder.AppendFormat(response.ErrorMessage, userhash, nodeID, nodeIDs);
                    await _logManager.StoreAnalyticLogAsync(DateTime.UtcNow, ILogManager.Levels.Error, ILogManager.Categories.Data,
                        stringBuilder.ToString());
                    return StatusCode(response.StatusCode, response.ErrorMessage);
                }
                else
                {
                    stringBuilder.AppendFormat(response.ErrorMessage, userhash, nodeID, nodeIDs);
                    await _logManager.StoreAnalyticLogAsync(DateTime.UtcNow, ILogManager.Levels.Error, ILogManager.Categories.Data,
                        stringBuilder.ToString());
                    return new BadRequestObjectResult(response.ErrorMessage) { StatusCode = response.StatusCode };
                }
            }
            catch (Exception ex)
            {
                stringBuilder.AppendFormat(await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false),
                    ex.Message, stringBuilder.AppendFormat("UserHash: {0}, NodeID: {1}, NodeIDs: {3}",
                    userhash, nodeID, nodeIDs).ToString());
                await _logManager.StoreAnalyticLogAsync(DateTime.UtcNow, ILogManager.Levels.Error, ILogManager.Categories.Server,
                    stringBuilder.ToString());
                return BadRequest();
            }
        }
    }
}
