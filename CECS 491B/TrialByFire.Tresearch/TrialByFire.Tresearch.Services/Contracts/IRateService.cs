using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;

namespace TrialByFire.Tresearch.Services.Contracts
{
    public interface IRateService
    {
        public Task<IResponse<NodeRating>> RateNodeAsync(string userHash, long nodeID, int rating, CancellationToken cancellationToken = default(CancellationToken));

        public Task<IResponse<IEnumerable<Node>>> GetNodeRatingAsync(List<long> nodeIDs, CancellationToken cancellationToken = default(CancellationToken));
    }
}
