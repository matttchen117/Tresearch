using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Services.Contracts
{
    public interface ITagService
    {
        public Task<string> AddTagToNodesAsync(List<long> nodeIDs, string tagName, CancellationToken cancellationToken = default(CancellationToken));
        public Task<string> RemoveTagFromNodesAsync(List<long> nodeIDs, string tagName, CancellationToken cancellationToken = default(CancellationToken));
        public Task<Tuple<List<string>, string>> GetNodeTagsAsync(List<long> nodeIDs, CancellationToken cancellationToken = default(CancellationToken));
        public Task<Tuple<List<string>, string>> GetNodeTagsDescAsync(List<long> nodeIDs, CancellationToken cancellationToken = default(CancellationToken));
        public Task<string> CreateTagAsync(string tagName, CancellationToken cancellationToken = default(CancellationToken));
        public Task<Tuple<List<string>, string>> GetTagsAsync(CancellationToken cancellationToken = default(CancellationToken));
        public Task<Tuple<List<string>, string>> GetTagsDescAsync(CancellationToken cancellationToken = default(CancellationToken));
        public Task<string> RemoveTagAsync(string tagName, CancellationToken cancellationToken = default(CancellationToken));
        public Task<Tuple<bool, string>> OwnsNode(List<long> nodeID, IAccount account, CancellationToken cancellationToken = default(CancellationToken));
    }  
}
