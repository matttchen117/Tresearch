using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Services.Contracts
{
    public interface IRateService
    {
        public Task<string> RateNodeAsync(IAccount account, long nodeID, int rating, CancellationToken cancellationToken = default(CancellationToken));

        public Task<Tuple<List<double>, string>> GetNodeRatingAsync(List<long> nodeID, CancellationToken cancellationToken = default(CancellationToken));
    }
}
