using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Managers.Implementations;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.WebApi.Controllers.Contracts;
using System.Text;

namespace TrialByFire.Tresearch.WebApi.Controllers.Implementations
{
    [ApiController]
    [EnableCors]
    [Route("controller")]
    public class UADController : Controller, IUADController
    {
        private ILogManager _logManager { get; }
        private IUADManager _uadManager { get; }
        private IMessageBank _messageBank { get; }
        private CancellationTokenSource _cts = new CancellationTokenSource(TimeSpan.FromSeconds(60));
        public UADController(ILogManager logManager, IUADManager uadManager, IMessageBank messageBank)
        {
            _logManager = logManager;
            _uadManager = uadManager;
            _messageBank = messageBank;
        }

        [HttpPost("uad")]
        public async Task<ActionResult<IKPI>> LoadKPIAsync(DateTime now)
        {
            StringBuilder stringBuilder = new StringBuilder();
            try
            {
                IResponse<IKPI> response = await _uadManager.LoadKPIAsync(now, _cts.Token).ConfigureAwait(false);
                return BadRequest();
            }
            catch (Exception ex)//TO DO Fix
            {
                //stringBuilder.AppendFormat(await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false),
                    //ex.Message, stringBuilder.AppendFormat("UserHash: {0}",
                    //userhash));
                await _logManager.StoreAnalyticLogAsync(DateTime.UtcNow, ILogManager.Levels.Error, ILogManager.Categories.Server,
                    stringBuilder.ToString());
                return BadRequest();
            }
        }
    }
}