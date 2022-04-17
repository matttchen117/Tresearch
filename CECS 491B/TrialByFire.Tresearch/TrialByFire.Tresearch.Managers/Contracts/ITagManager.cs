using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Managers.Contracts
{
    /// <summary>
    /// Tag Manager delegates the services carrying out tag tasks.
    /// </summary>
    public interface ITagManager
    {
        /// <summary>
        ///  Adds a tag to a list of node(s)
        /// </summary>
        /// <param name="nodeIDs">List of node(s) IDs</param>
        /// <param name="tagName">String tag name</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>String status code</returns>
        public Task<string> AddTagToNodesAsync(List<long> nodeIDs, string tagName, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Removes a tag from a list of node(s).
        /// </summary>
        /// <param name="nodeIDs">List of node(s) IDs</param>
        /// <param name="tagName">String tag name</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>String status code</returns>
        public Task<string> RemoveTagFromNodesAsync(List<long> nodeIDs, string tagName, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Retrieves a list of shared node tag(s) from a list of node(s)
        /// </summary>
        /// <param name="nodeIDs"List of node(s) IDs></param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>List of tags and string status code</returns>
        public Task<Tuple<List<string>, string>> GetNodeTagsAsync(List<long> nodeIDs, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Creates a tag in tag word bank
        /// </summary>
        /// <param name="tagName">String tag name</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>String status code</returns>
        public Task<string> CreateTagAsync(string tagName, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Removes a tag from tag word bank
        /// </summary>
        /// <param name="tagName">String tag name</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>String status code</returns>
        public Task<string> RemoveTagAsync(string tagName, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Retrieves a list of all tags in word bank
        /// </summary>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>List of word bank tags and string status code</returns>
        public Task<Tuple<List<ITag>, string>> GetTagsAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
