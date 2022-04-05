using Microsoft.AspNetCore.Mvc;

namespace TrialByFire.Tresearch.WebApi.Controllers.Contracts
{
    /// <summary>
    /// Helper class to access get and post requests regarding tagging feature
    /// </summary>
    public interface ITagController
    {
        /// <summary>
        ///     Gets a string list containing all possible tags 
        /// </summary>
        /// <returns>Status code and List of tags</returns>
        public Task<IActionResult> GetTagsAsync();

        /// <summary>
        ///     Creates a tag in tag bank
        /// </summary>
        /// <param name="tagName">String tag name to add</param>
        /// <returns>Status code and string status</returns>
        public Task<IActionResult> CreateTagAsync(string tagName);

        /// <summary>
        ///     Deletes a tag from tag bank
        /// </summary>
        /// <param name="tagName">String tag name to remove</param>
        /// <returns>Status code and string status</returns>
        public Task<IActionResult> DeleteTagAsync(string tagName);

        /// <summary>
        ///     Adds a tag from bank to node
        /// </summary>
        /// <param name="nodeIDs">List of long nodeIDs to add tag to</param>
        /// <param name="tagName">String tag name to add to node</param>
        /// <returns>Status code and string status</returns>
        public Task<IActionResult> AddTagToNodesAsync(List<long> nodeIDs, string tagName);

        /// <summary>
        ///     Removes a tag from node
        /// </summary>
        /// <param name="nodeIDs">List of long nodeIDs to add tag to</param>
        /// <param name="tagName">String tag name to remove from node</param>
        /// <returns>Status code and string status</returns>
        public Task<IActionResult> RemoveTagFromNodesAsync(List<long> nodeIDs, string tagName);

        /// <summary>
        ///     Gets a list of tags a list of nodes contain
        /// </summary>
        /// <param name="nodeIDs">List of nodes to retrieve tags taht all nodes in list contain</param>
        /// <returns>List of tags common to nodes</returns>
        public Task<IActionResult> GetNodeTagsAsync(List<long> nodeIDs);
    }
}
