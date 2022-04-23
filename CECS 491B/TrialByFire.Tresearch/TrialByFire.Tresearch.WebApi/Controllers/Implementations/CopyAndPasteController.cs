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
                string[] split;
                string result = "";

                result - await _copyAndPasteManager.CopyNodeAsync(nodeIDs, CancellationTokenSource.Token).ConfigureAwait(false);

            } 






            throw new NotImplementedException();
        }

        public Task<IActionResult> PasteNodeAsync(List<long> nodeIDs)
        {
            throw new NotImplementedException();
        }
    }
}
