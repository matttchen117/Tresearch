using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Services.Contracts
{
    public interface ITagService
    {
        public Task<string> AddTagToNodesAsync(List<string> nodeIDs, string tagName, CancellationToken cancellationToken = default(CancellationToken));

        public Task<string> RemoveTagFromNodesAsync(List<string> nodeIDs, string tagName, CancellationToken cancellationToken = default(CancellationToken));
    
        public Task<Tuple<List<string>, string>> GetNodeTagsAsync(List<string> nodeIDs, CancellationToken cancellationToken = default(CancellationToken));

        public Task<string> CreateTagAsync(string tagName, CancellationToken cancellationToken = default(CancellationToken));

        public Task<Tuple<List<string>, string>> GetTagsAsync(CancellationToken cancellationToken = default(CancellationToken));

        public Task<string> RemoveTagAsync(string tagName, CancellationToken cancellationToken = default(CancellationToken));
    }

    
}
