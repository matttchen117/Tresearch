


namespace TrialByFire.Tresearch.Managers.Contracts
{
    public interface IRateManager
    {
       public Task<string> RateNodeAsync(long nodeID, int rating, CancellationToken cancellationToken = default(CancellationToken));
       public Task<Tuple<List<double>,string>> GetNodeRatingAsync(List<long> nodeIDs, CancellationToken cancellationToken = default(CancellationToken));
    }
}
