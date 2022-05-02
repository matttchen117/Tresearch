using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;

namespace TrialByFire.Tresearch.Services.Contracts
{
    public interface IRateService
    {
        public Task<IResponse<NodeRating>> RateNodeAsync(string userHash, long nodeID, int rating, CancellationToken cancellationToken = default(CancellationToken));

        public Task<IResponse<double>> GetNodeRatingAsync(long nodeID, CancellationToken cancellationToken = default(CancellationToken));
    }
}
