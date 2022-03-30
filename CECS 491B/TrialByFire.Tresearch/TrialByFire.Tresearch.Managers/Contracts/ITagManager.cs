using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrialByFire.Tresearch.Managers.Contracts
{
    public interface ITagManager
    {
        public Task<string> AddTagToNodesAsync(List<long> nodeIDs, string tagName, CancellationToken cancellationToken = default(CancellationToken));
        public Task<string> RemoveTagFromNodesAsync(List<long> nodeIDs, string tagName, CancellationToken cancellationToken = default(CancellationToken));
        public Task<Tuple<List<string>, string>> GetNodeTagsAsync(List<long> nodeIDs, CancellationToken cancellationToken = default(CancellationToken));
        public Task<string> CreateTagAsync(string tagName, CancellationToken cancellationToken = default(CancellationToken));
        public Task<string> RemoveTagAsync(string tagName, CancellationToken cancellationToken = default(CancellationToken));
        public Task<Tuple<List<string>, string>> GetTagsAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
