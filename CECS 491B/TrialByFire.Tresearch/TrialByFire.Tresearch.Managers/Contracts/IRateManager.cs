using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;

namespace TrialByFire.Tresearch.Managers.Contracts
{
    public interface IRateManager
    {
        public Task<IResponse<int>> RateNodeAsync(List<long> nodeID, int rating, CancellationToken cancellationToken = default(CancellationToken));
        public Task<IResponse<IEnumerable<Node>>> GetNodeRatingAsync(List<long> nodeIDs, CancellationToken cancellationToken = default(CancellationToken));
        public Task<IResponse<int>> GetUserNodeRatingAsync(long nodeID, CancellationToken cancellationToken = default(CancellationToken));
    }
}
