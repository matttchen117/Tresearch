using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.WebApi.Controllers.Contracts;

namespace TrialByFire.Tresearch.WebApi.Controllers.Implementations
{
    [ApiController]
    [EnableCors]
    [Route("[Controller]")]
    public class TagController: ControllerBase, ITagController
    {
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(15));
        private ISqlDAO _sqlDAO { get; set; }
        private ILogService _logService { get; set; }
        private IMessageBank _messageBank { get; set; }
        private ITagManager _tagManager { get; set; }

        public TagController(ISqlDAO sqlDAO, ILogService logService, IMessageBank messageBank, ITagManager tagManager)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
            _messageBank = messageBank;
            _tagManager = tagManager;
        }

        [HttpGet]
        [Route("taglist")]
       public async Task<IActionResult> GetTagsAsync()
        {
            try
            {
                Tuple<List<string>, string> result = await _tagManager.GetTagsAsync(_cancellationTokenSource.Token);
                string[] split;
                split = result.Item2.Split(":");
                return StatusCode(Convert.ToInt32(split[0]), result.Item1);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(408, "Request time out");
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("createTag")]
        public async Task<IActionResult> CreateTagAsync(string tagName)
        {
            try
            {
                string result = await _tagManager.CreateTagAsync(tagName, _cancellationTokenSource.Token);
                string[] split;
                split = result.Split(":");
                return StatusCode(Convert.ToInt32(split[0]), split[2]);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
