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

    /// <summary>
    ///     Controller to perform node tag add/removal and tag bank create/delete
    /// </summary>
    [ApiController]
    [EnableCors]
    [Route("[Controller]")]
    public class TagController: ControllerBase, ITagController
    {
        /// <summary>
        /// Cancellation token throws when not updated within 5 seconds, as per BRD
        /// </summary>
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));

        /// <summary>
        ///     Manager to perform logging for error and success cases
        /// </summary>
        private ILogManager _logManager { get; set; }

        /// <summary>
        ///     Model holding string responses to enumerated cases
        /// </summary>
        private IMessageBank _messageBank { get; set; }

        /// <summary>
        ///     Manager to perform tag feature
        /// </summary>
        private ITagManager _tagManager { get; set; }
        public TagController(ILogManager logManager, IMessageBank messageBank, ITagManager tagManager)
        {
            _logManager = logManager;
            _messageBank = messageBank;
            _tagManager = tagManager;
        }

        /// <summary>
        ///     Get Request for retrieving all tags and count in the tag database. User does not need to be authenticated or authorized to retrieve
        /// </summary>
        /// <returns>Status code and List of Tags</returns>
        [HttpGet]
        [Route("taglist")]
        public async Task<IActionResult> GetTagsAsync()
        {
            try
            {
                // Retrieve tags from tag bank
                Tuple<List<ITag>, string> result = await _tagManager.GetTagsAsync(_cancellationTokenSource.Token).ConfigureAwait(false);

                // Split string for logging
                string[] split;
                split = result.Item2.Split(":");
                if (result.Item2.Equals(await _messageBank.GetMessage(IMessageBank.Responses.tagGetSuccess).ConfigureAwait(false)))
                {
                    // Successfully retrieved tags => store success log
                    await _logManager.StoreAnalyticLogAsync(DateTime.Now.ToUniversalTime(), ILogManager.Levels.Info, ILogManager.Categories.Server, split[2]).ConfigureAwait(false);
                    return new OkObjectResult(result.Item1);
                }
                else
                {
                    // Tags were not successfully retrieved => store error log
                    Enum.TryParse(split[1], out ILogManager.Categories category);
                    await _logManager.StoreArchiveLogAsync(DateTime.UtcNow, ILogManager.Levels.Error, category, split[2]).ConfigureAwait(false);
                    return StatusCode(Convert.ToInt32(split[0]), result.Item1);
                }
            } 
            catch(OperationCanceledException ex)
            {
                // Operation cancelled threw exception no rollback necessary
                string errorMessage = await _messageBank.GetMessage(IMessageBank.Responses.operationCancelled).ConfigureAwait(false) + ex.Message;
                string[] split;
                split = errorMessage.Split(":");
                Enum.TryParse(split[1], out ILogManager.Categories category);
                await _logManager.StoreArchiveLogAsync(DateTime.UtcNow, ILogManager.Levels.Error, category, split[2]).ConfigureAwait(false);
                return StatusCode(Convert.ToInt32(split[0]), split[2]);
            }
            catch(Exception ex)
            {
                string errorMessage = await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message;
                string[] split;
                split = errorMessage.Split(":");
                await _logManager.StoreArchiveLogAsync(DateTime.UtcNow, ILogManager.Levels.Error, ILogManager.Categories.Server, split[2]).ConfigureAwait(false);
                return new BadRequestObjectResult(split[2]);
            }
        }

        /// <summary>
        ///     Creates a tag in tag bank. User must be authenticated and authorized as an administrator
        /// </summary>
        /// <param name="tagName">String tag name to add</param>
        /// <returns>Status code and string status</returns>
        [HttpPost("createTag")]
        public async Task<IActionResult> CreateTagAsync(string tagName)
        {
            try
            {
                // Check if user identity is known
                if (Thread.CurrentPrincipal != null)
                {
                    string result;
                    //Check tag input is null, empty string or string whith only whitespace
                    if (tagName == null || tagName.Equals("") || tagName.Trim().Equals(""))
                        result = await _messageBank.GetMessage(IMessageBank.Responses.tagNameInvalid).ConfigureAwait(false);
                    else
                        result = await _tagManager.CreateTagAsync(tagName, _cancellationTokenSource.Token).ConfigureAwait(false);

                    // Split result for logging
                    string[] split;
                    split = result.Split(":");

                    // Check if result successfull
                    if (result.Equals(await _messageBank.GetMessage(IMessageBank.Responses.tagCreateSuccess)))
                    {
                        // Log success
                        await _logManager.StoreAnalyticLogAsync(DateTime.Now.ToUniversalTime(), ILogManager.Levels.Info, ILogManager.Categories.Server, split[2]).ConfigureAwait(false);
                        return new OkObjectResult(split[2]);
                    }
                    else
                    {
                        // Log error
                        Enum.TryParse(split[1], out ILogManager.Categories category);
                        await _logManager.StoreArchiveLogAsync(DateTime.UtcNow, ILogManager.Levels.Error, category, split[2]).ConfigureAwait(false);
                        return StatusCode(Convert.ToInt32(split[0]), split[2]);
                    }
                }
                else
                {
                    // Uknown identity
                    string errorMessage = await _messageBank.GetMessage(IMessageBank.Responses.notAuthenticated).ConfigureAwait(false);
                    string[] split;
                    split = errorMessage.Split(":");
                    await _logManager.StoreArchiveLogAsync(DateTime.UtcNow, ILogManager.Levels.Error, ILogManager.Categories.Server, split[2]).ConfigureAwait(false);
                    return new BadRequestObjectResult(split[2]);
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
                string errorMessage = await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message;
                string[] split;
                split = errorMessage.Split(":");
                await _logManager.StoreArchiveLogAsync(DateTime.UtcNow, ILogManager.Levels.Error, ILogManager.Categories.Server, split[2]).ConfigureAwait(false);
                return new BadRequestObjectResult(split[2]);
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
                // CHeck if user identity is known
                if (Thread.CurrentPrincipal != null)
                {
                    string result;
                    //Check tag input if null, empty string or string with only whitespace
                    if (tagName == null || tagName.Equals("") || tagName.Trim().Equals(""))
                        result = await _messageBank.GetMessage(IMessageBank.Responses.tagNameInvalid).ConfigureAwait(false);
                    else
                        result = await _tagManager.RemoveTagAsync(tagName, _cancellationTokenSource.Token).ConfigureAwait(false);

                    // Split result for logging
                    string[] split;
                    split = result.Split(":");

                    // Check if result successful
                    if (result.Equals(await _messageBank.GetMessage(IMessageBank.Responses.tagDeleteSuccess)))
                    {
                        // Log success
                        await _logManager.StoreAnalyticLogAsync(DateTime.Now.ToUniversalTime(), ILogManager.Levels.Info, ILogManager.Categories.Server, split[2]);
                        return new OkObjectResult(split[2]);
                    }
                    else
                    {
                        // Log error
                        Enum.TryParse(split[1], out ILogManager.Categories category);
                        await _logManager.StoreArchiveLogAsync(DateTime.UtcNow, ILogManager.Levels.Error, category, split[2]);
                        return StatusCode(Convert.ToInt32(split[0]), split[2]);
                    }
                }
                else
                {
                    // Unknown user identity
                    string errorMessage = await _messageBank.GetMessage(IMessageBank.Responses.notAuthenticated).ConfigureAwait(false);
                    string[] split;
                    split = errorMessage.Split(":");
                    await _logManager.StoreArchiveLogAsync(DateTime.UtcNow, ILogManager.Levels.Error, ILogManager.Categories.Server, split[2]).ConfigureAwait(false);
                    return new BadRequestObjectResult(split[2]);
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
                string errorMessage = await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message;
                string[] split;
                split = errorMessage.Split(":");
                await _logManager.StoreArchiveLogAsync(DateTime.UtcNow, ILogManager.Levels.Error, ILogManager.Categories.Server, split[2]).ConfigureAwait(false);
                return new BadRequestObjectResult(split[2]);
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
                // Check if user identity is known
                if (Thread.CurrentPrincipal != null)
                {
                    Tuple<List<string>, string> result;

                    // Validate input
                    if (nodeIDs == null || nodeIDs.Count() <= 0)
                        result =  Tuple.Create(new List<string>(), await _messageBank.GetMessage(IMessageBank.Responses.nodeNotFound).ConfigureAwait(false));
                    else
                        result = await _tagManager.GetNodeTagsAsync(nodeIDs, _cancellationTokenSource.Token);

                    // Split result for logging
                    string[] split;
                    split = result.Item2.Split(":");

                    // Check if result successful
                    if (result.Equals(await _messageBank.GetMessage(IMessageBank.Responses.tagGetSuccess)))
                    {
                        // Log success
                        await _logManager.StoreAnalyticLogAsync(DateTime.Now.ToUniversalTime(), ILogManager.Levels.Info, ILogManager.Categories.Server, split[2]);
                        return new OkObjectResult(result.Item1);
                    }
                    else
                    {
                        // Log error
                        Enum.TryParse(split[1], out ILogManager.Categories category);
                        await _logManager.StoreArchiveLogAsync(DateTime.UtcNow, ILogManager.Levels.Error, category, split[2]);
                        return StatusCode(Convert.ToInt32(split[0]), result.Item1);
                    }
                }
                else
                {
                    // Unknown user identity
                    string errorMessage = await _messageBank.GetMessage(IMessageBank.Responses.notAuthenticated).ConfigureAwait(false);
                    string[] split;
                    split = errorMessage.Split(":");
                    await _logManager.StoreArchiveLogAsync(DateTime.UtcNow, ILogManager.Levels.Error, ILogManager.Categories.Server, split[2]).ConfigureAwait(false);
                    return new BadRequestObjectResult(split[2]);
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
                string errorMessage = await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message;
                string[] split;
                split = errorMessage.Split(":");
                await _logManager.StoreArchiveLogAsync(DateTime.UtcNow, ILogManager.Levels.Error, ILogManager.Categories.Server, split[2]).ConfigureAwait(false);
                return new BadRequestObjectResult(split[2]);
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
                // Check if user identity is known
                if (Thread.CurrentPrincipal != null)
                {
                    string result;

                    // Check if tag name is null, empty string or all space
                    if (tagName == null || tagName.Equals("") || tagName.Trim().Equals(""))
                        result = await _messageBank.GetMessage(IMessageBank.Responses.tagNameInvalid).ConfigureAwait(false);
                    else if (nodeIDs == null || nodeIDs.Count() <= 0)
                        result = await _messageBank.GetMessage(IMessageBank.Responses.nodeNotFound).ConfigureAwait(false);
                    else
                        result = await _tagManager.AddTagToNodesAsync(nodeIDs, tagName, _cancellationTokenSource.Token);

                    // Split for logging
                    string[] split;
                    split = result.Split(":");

                    // Check if add node tag was successfull
                    if (result.Equals(await _messageBank.GetMessage(IMessageBank.Responses.tagAddSuccess)))
                    {
                        // Log success
                        await _logManager.StoreAnalyticLogAsync(DateTime.Now.ToUniversalTime(), ILogManager.Levels.Info, ILogManager.Categories.Server, split[2]);
                        return new OkObjectResult(split[2]);
                    }
                    else
                    {
                        // Log error
                        Enum.TryParse(split[1], out ILogManager.Categories category);
                        await _logManager.StoreArchiveLogAsync(DateTime.UtcNow, ILogManager.Levels.Error, category, split[2]);
                        return StatusCode(Convert.ToInt32(split[0]), split[2]);
                    }
                }
                else
                {
                    // User identity not known, log error
                    string errorMessage = await _messageBank.GetMessage(IMessageBank.Responses.notAuthenticated).ConfigureAwait(false);
                    string[] split;
                    split = errorMessage.Split(":");
                    await _logManager.StoreArchiveLogAsync(DateTime.UtcNow, ILogManager.Levels.Error, ILogManager.Categories.Server, split[2]).ConfigureAwait(false);
                    return new BadRequestObjectResult(split[2]);
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
                string errorMessage = await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message;
                string[] split;
                split = errorMessage.Split(":");
                await _logManager.StoreArchiveLogAsync(DateTime.UtcNow, ILogManager.Levels.Error, ILogManager.Categories.Server, split[2]).ConfigureAwait(false);
                return new BadRequestObjectResult(split[2]);
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
                // Check if user identity is known
                if (Thread.CurrentPrincipal != null)
                {
                    string result;
                    // Check if tag name is null, empty string or all space
                    if (tagName == null || tagName.Equals("") || tagName.Trim().Equals(""))
                        result = await _messageBank.GetMessage(IMessageBank.Responses.tagNameInvalid).ConfigureAwait(false);
                    else if (nodeIDs == null || nodeIDs.Count() <= 0)
                        result = await _messageBank.GetMessage(IMessageBank.Responses.nodeNotFound).ConfigureAwait(false);
                    else
                        result = await _tagManager.RemoveTagFromNodesAsync(nodeIDs, tagName, _cancellationTokenSource.Token);

                    string[] split;
                    split = result.Split(":");
                    if (result.Equals(await _messageBank.GetMessage(IMessageBank.Responses.tagRemoveSuccess)))
                    {
                        await _logManager.StoreAnalyticLogAsync(DateTime.Now.ToUniversalTime(), ILogManager.Levels.Info, ILogManager.Categories.Server, split[2]);
                        return new OkObjectResult(split[2]);
                    }
                    else
                    {
                        Enum.TryParse(split[1], out ILogManager.Categories category);
                        await _logManager.StoreArchiveLogAsync(DateTime.UtcNow, ILogManager.Levels.Error, category, split[2]);
                        return StatusCode(Convert.ToInt32(split[0]), split[2]);
                    }
                }
                else
                {
                    string errorMessage = await _messageBank.GetMessage(IMessageBank.Responses.notAuthenticated).ConfigureAwait(false);
                    string[] split;
                    split = errorMessage.Split(":");
                    await _logManager.StoreArchiveLogAsync(DateTime.UtcNow, ILogManager.Levels.Error, ILogManager.Categories.Server, split[2]).ConfigureAwait(false);
                    return new BadRequestObjectResult(split[2]);
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
                string errorMessage = await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message;
                string[] split;
                split = errorMessage.Split(":");
                await _logManager.StoreArchiveLogAsync(DateTime.UtcNow, ILogManager.Levels.Error, ILogManager.Categories.Server, split[2]).ConfigureAwait(false);
                return new BadRequestObjectResult(split[2]);
            }
        }
    }
}
