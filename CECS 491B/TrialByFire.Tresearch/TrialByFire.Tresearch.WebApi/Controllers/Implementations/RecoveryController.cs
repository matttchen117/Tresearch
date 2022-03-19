using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.WebApi.Controllers.Contracts;



namespace TrialByFire.Tresearch.WebApi.Controllers.Implementations
{
    [ApiController]
    [EnableCors]
    [Route("[Controller]")]  

    public class RecoveryController: ControllerBase, IRecoveryController
    {
        // User must recieve recovery email within 15 seconds upon invocation

        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(15));
        private ISqlDAO _sqlDAO { get; set; }

        private ILogService _logService { get; set; }

        private IMessageBank _messageBank { get; set; }
        private IRecoveryManager _recoveryManager { get; set; }


        public RecoveryController(ISqlDAO sqlDAO, ILogService logService, IMessageBank messageBank,IRecoveryManager recoverManager)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
            _messageBank = messageBank;
            _recoveryManager = recoverManager;
        }

        [HttpPost("SendRecovery")]
        public async Task<IActionResult> SendRecoveryEmailAsync(string email, string authorizationLevel)
        {
            try
            {
                string baseUrl = "https://localhost:7010/Recovery/recover?=";
                string result = await _recoveryManager.SendRecoveryEmail(email, baseUrl, authorizationLevel);
                string[] split;
                split = result.Split(":");
                if(result == _messageBank.SuccessMessages["generic"])
                {
                    return new OkObjectResult(split[2]) { StatusCode = Convert.ToInt32(split[0]) };
                } else
                {
                    return StatusCode(Convert.ToInt32(split[0]), split[2]);
                }

            } 
            catch(OperationCanceledException ex)
            {
                return StatusCode(400, ex.Message);
            }
            catch(Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }

    }

    
}
