using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;

namespace TrialByFire.Tresearch.Managers.Contracts
{
    public interface IRateManager
    {
       public Task<IResponse<NodeRating>> RateNodeAsync(long nodeID, int rating, CancellationToken cancellationToken = default(CancellationToken));
       public Task<IResponse<double>> GetNodeRatingAsync(long nodeIDs, CancellationToken cancellationToken = default(CancellationToken));
    }
}
