using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Managers.Implementations;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.WebApi.Controllers.Contracts;

namespace TrialByFire.Tresearch.WebApi.Controllers.Implementations
{
    [ApiController]
    [EnableCors]
    [Route("[controller]")]
    public class TreeManagementController : Controller, ITreeManagementController
    {
        private ISqlDAO _sqlDAO { get; }
        private ILogService _logService { get; }
        private ITreeManagementManager _treeManagementManager { get; }
        private IMessageBank _messageBank { get; }
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public TreeManagementController(ISqlDAO sqlDAO, ILogService logService, ITreeManagementManager treeManagementManager, IMessageBank messageBank)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
            _treeManagementManager = treeManagementManager;
            _messageBank = messageBank;
        }

        [HttpPost]
        [Route("treeManagement")]
        public async Task<IActionResult> GetNodesAsync(string userHash, string accountHash)
        {
            try
            {
                Tuple<Tree, string> result = await _treeManagementManager.GetNodesAsync(userHash, accountHash, _cancellationTokenSource.Token).ConfigureAwait(false);
                string[] split;
                split = result.Item2.Split(":");
                return StatusCode(Convert.ToInt32(split[0]), result.Item1);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(408, "Request time out");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("getNodes")]
        public async Task<IActionResult> GetNodesAsync(string owner)
        {
            try
            {
                string userHash = (Thread.CurrentPrincipal.Identity as IRoleIdentity).UserHash;
                Tuple<Tree, string> result = await _treeManagementManager.GetNodesAsync(owner, userHash, _cancellationTokenSource.Token).ConfigureAwait(false);
                string[] split;
                split = result.Item2.Split(":");
                return StatusCode(Convert.ToInt32(split[0]), result.Item1);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(408, "Request time out");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}