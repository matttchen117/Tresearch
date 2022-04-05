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
    public class TagController: ControllerBase, ITagController
    {
        // User tree data updated within 5 seconds, as per BRD
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        private ISqlDAO _sqlDAO { get; set; }
        private ILogService _logService { get; set; }
        private IMessageBank _messageBank { get; set; }
        private ITagManager _tagManager { get; set; }
        private BuildSettingsOptions _options { get; }

        public TagController(ISqlDAO sqlDAO, ILogService logService, IMessageBank messageBank, ITagManager tagManager, IOptionsSnapshot<BuildSettingsOptions> options)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
            _messageBank = messageBank;
            _tagManager = tagManager;
            _options = options.Value;
        }

        /// <summary>
        ///     Gets a string list containing all possible tags 
        /// </summary>
        /// <returns>Status code and List of tags</returns>
        [HttpGet]
        [Route("taglist")]
       public async Task<IActionResult> GetTagsAsync()
        {
            try
            {
                Tuple<List<string>, string> result = await _tagManager.GetTagsAsync("asc", _cancellationTokenSource.Token);
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

        [HttpPost("getPossibleTags")]
        public async Task<IActionResult> GetPossibleTagsAsync(List<long> nodeIDs)
        {
            try
            {
                Tuple<List<string>, string> result = await _tagManager.GetPossibleTagsAsync(nodeIDs, _cancellationTokenSource.Token);
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

        /// <summary>
        ///     Creates a tag in tag bank
        /// </summary>
        /// <param name="tagName">String tag name to add</param>
        /// <returns>Status code and string status</returns>
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
            catch (OperationCanceledException)
            {
                return StatusCode(408, "Request time out");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        ///     Deletes a tag from tag bank
        /// </summary>
        /// <param name="tagName">String tag name to remove</param>
        /// <returns>Status code and string status</returns>
        [HttpPost("deleteTag")]
        public async Task<IActionResult> DeleteTagAsync(string tagName)
        {
            try
            {
                string result = await _tagManager.RemoveTagAsync(tagName, _cancellationTokenSource.Token);
                string[] split;
                split = result.Split(":");
                return StatusCode(Convert.ToInt32(split[0]), split[2]);
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

        /// <summary>
        ///     Gets a list of tags a list of nodes contain
        /// </summary>
        /// <param name="nodeIDs">List of nodes to retrieve tags taht all nodes in list contain</param>
        /// <returns>List of tags common to nodes</returns>
        [HttpPost("nodeTaglist")]
        public async Task<IActionResult> GetNodeTagsAsync(List<long> nodeIDs)
        {
            try
            {
                Tuple<List<string>, string> result = await _tagManager.GetNodeTagsAsync(nodeIDs, "asc",_cancellationTokenSource.Token);
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


        /// <summary>
        ///     Adds a tag from bank to node
        /// </summary>
        /// <param name="nodeIDs">List of long nodeIDs to add tag to</param>
        /// <param name="tagName">String tag name to add to node</param>
        /// <returns>Status code and string status</returns>
        [HttpPost("addTag")]
        public async Task<IActionResult> AddTagToNodesAsync(List<long> nodeIDs, string tagName)
        {
            try
            {
                string result = await _tagManager.AddTagToNodesAsync(nodeIDs,tagName, _cancellationTokenSource.Token);
                string[] split;
                split = result.Split(":");
                return StatusCode(Convert.ToInt32(split[0]), split[2]);
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

        /// <summary>
        ///     Removes a tag from node
        /// </summary>
        /// <param name="nodeIDs">List of long nodeIDs to add tag to</param>
        /// <param name="tagName">String tag name to remove from node</param>
        /// <returns>Status code and string status</returns>
        [HttpPost("removeTag")]
        public async Task<IActionResult> RemoveTagFromNodesAsync(List<long> nodeIDs, string tagName)
        {
            try
            {
                string result = await _tagManager.RemoveTagFromNodesAsync(nodeIDs, tagName, _cancellationTokenSource.Token);
                string[] split;
                split = result.Split(":");
                return StatusCode(Convert.ToInt32(split[0]), split[2]);
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
