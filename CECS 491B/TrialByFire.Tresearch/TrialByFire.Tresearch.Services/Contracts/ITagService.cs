using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Services.Contracts
{
    /// <summary>
    ///  Interface to perform business logic regarding a node's tags
    /// </summary>
    public interface ITagService
    {
        /// <summary>
        ///     Adds a tag to node. Must have at least one node.
        /// </summary>
        /// <param name="nodeIDs">List of node IDs</param>
        /// <param name="tagName">Tag to add</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>String status code</returns>
        public Task<string> AddTagToNodesAsync(List<long> nodeIDs, string tagName, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        ///     Removes tag from nodes passed in. Must have at least one node.
        /// </summary>
        /// <param name="nodeIDs">List of node IDs</param>
        /// <param name="tagName">Tag to remove</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>String status code</returns>
        public Task<string> RemoveTagFromNodesAsync(List<long> nodeIDs, string tagName, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        ///      Retrieves all tags shared by node(s). If list contains single node, all tags are returned. Must have at least one node.
        /// </summary>
        /// <param name="nodeIDs"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<Tuple<List<string>, string>> GetNodeTagsAsync(List<long> nodeIDs, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        ///     Creates a tag in tag bank.
        /// </summary>
        /// <param name="tagName">String tag name</param>
        /// <param name="count">Count of nodes with tag</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>String status code</returns>
        public Task<string> CreateTagAsync(string tagName, int count, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        ///      Retrieves list of all tags in tag bank.
        /// </summary>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>List of tags and status code</returns>
        public Task<Tuple<List<ITag>, string>> GetTagsAsync(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        ///     Removes tag from tag bank
        /// </summary>
        /// <param name="tagName">Tag to remove</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>String status code</returns>
        public Task<string> RemoveTagAsync(string tagName, CancellationToken cancellationToken = default(CancellationToken));
    }  
}
