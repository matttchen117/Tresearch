
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
    public class AccountDeletionController : Controller, IAccountDeletionController
    {

        private CancellationTokenSource CancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        private BuildSettingsOptions _options { get; }
        private ISqlDAO _sqlDAO { get; }
        private ILogService _logService { get; }
        private IMessageBank _messageBank { get; }
        private IAccountDeletionManager _accountDeletionManager { get; }




    

        public AccountDeletionController(ISqlDAO sqlDAO, ILogService logService, IMessageBank messageBank, IAccountDeletionManager accountDeletionManager, IOptionsSnapshot<BuildSettingsOptions>  options)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
            _messageBank = messageBank;
            _accountDeletionManager = accountDeletionManager;
            _options = options.Value;
        }

        /// <summary>
        /// entry point for Account Deletion Requests
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        [Route("DeleteAccount")]
        public async Task<IActionResult> DeleteAccountAsync()
        {

            try
            {
                string[] split;
                string result = "";


                result = await _accountDeletionManager.DeleteAccountAsync(CancellationTokenSource.Token).ConfigureAwait(false);

                if (result.Equals(await _messageBank.GetMessage(IMessageBank.Responses.accountDeletionSuccess).ConfigureAwait(false)))
                {

                    if (_options.Environment.Equals("Test"))
                    {

                        split = result.Split(": ");
                        return new OkObjectResult(split[2]) { StatusCode = Convert.ToInt32(split[0]) };
                    }

                    split = result.Split(": ");
                    return new OkObjectResult(split[2]) { StatusCode = Convert.ToInt32(split[0]) };

                }

                split = result.Split(": ");
                return StatusCode(Convert.ToInt32(split[0]), split[2]);

            }



            catch (OperationCanceledException tce)
            {
                return StatusCode(400, tce.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }










        }

    }
}
