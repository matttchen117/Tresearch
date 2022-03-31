
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
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

        private BuildSettingsOptions BuildSettingsOptions { get; }
        private ISqlDAO SqlDAO { get; }
        private ILogService LogService { get; }

        private IMessageBank _messageBank { get; }
        private IAccountDeletionManager AccountDeletionManager { get; }

        private BuildSettingsOptions _buildSettingsOptions { get; }





        public AccountDeletionController(ISqlDAO sqlDAO, ILogService logService, IMessageBank messageBank, IAccountDeletionManager accountDeletionManager, BuildSettingsOptions buildSettingsOptions)
        {
            this.SqlDAO = sqlDAO;
            this.LogService = logService;
            this._messageBank = messageBank;
            this.AccountDeletionManager = accountDeletionManager;
            this.BuildSettingsOptions = buildSettingsOptions;
        }

        /// <summary>
        /// entry point for Account Deletion Requests
        /// </summary>
        /// <returns></returns>

        [HttpPost("DeleteAccount")]
        public async Task<IActionResult> DeleteAccountAsync()
        {

            string[] split;
            string result = "";

            try
            {
                result = await AccountDeletionManager.DeleteAccountAsync(CancellationTokenSource.Token).ConfigureAwait(false);

                if (result.Equals(_messageBank.GetMessage(IMessageBank.Responses.generic)))
                {
                    if (_buildSettingsOptions.Environment.Equals("Test"))
                    {
                        split = result.Split(": ");
                        return new OkObjectResult(split[2]) { StatusCode = Convert.ToInt32(split[0]) };
                    }

                }

                split = result.Split(": ");
                return new OkObjectResult(StatusCode(Convert.ToInt32(split[0]), split[2]));


            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);

            }




        }

    }
}
