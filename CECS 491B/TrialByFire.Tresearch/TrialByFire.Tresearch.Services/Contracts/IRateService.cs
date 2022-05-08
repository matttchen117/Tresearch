using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;

namespace TrialByFire.Tresearch.Services.Contracts
{
    public interface IRateService
    {
        public Task<IResponse<int>> RateNodeAsync(string userHash, List<long> nodeID, int rating, CancellationToken cancellationToken = default(CancellationToken));

        public Task<IResponse<IEnumerable<Node>>> GetNodeRatingAsync(List<long> nodeIDs, CancellationToken cancellationToken = default(CancellationToken));

        public Task<IResponse<int>> GetUserNodeRatingAsync(long nodeID, string userHash, CancellationToken cancellationToken = default(CancellationToken));
    }
}
