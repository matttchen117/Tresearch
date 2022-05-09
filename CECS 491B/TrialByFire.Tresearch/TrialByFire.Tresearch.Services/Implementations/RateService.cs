using Microsoft.Extensions.Options;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Models;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Services.Contracts;

namespace TrialByFire.Tresearch.Services.Implementations
{
    public class RateService: IRateService
    {
        private ISqlDAO _sqlDAO { get; set; }

        private ILogService _logService { get; set; }

        private IMessageBank _messageBank { get; set; }

        private BuildSettingsOptions _options { get; }
        public RateService(ISqlDAO sqlDAO, ILogService logService, IMessageBank messageBank, IOptions<BuildSettingsOptions> options)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
            _messageBank = messageBank;
            _options = options.Value;
        }

        /// <summary>
        ///  Rate list of nodes
        /// </summary>
        /// <param name="userHash">User hash of user rating</param>
        /// <param name="nodeID">Node id to rate</param>
        /// <param name="rating">Rate (1-5)</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns></returns>
        public async Task<IResponse<int>> RateNodeAsync(string userHash, List<long> nodeIDs, int rating, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                if(nodeIDs == null || nodeIDs.Count.Equals(0))
                    return new RateResponse<int>(await _messageBank.GetMessage(IMessageBank.Responses.nodeNotFound), rating, 404, false);
                if(rating <= 0)
                    return new RateResponse<int>(await _messageBank.GetMessage(IMessageBank.Responses.invalidRating), rating, 422, false);

                IResponse<int> result = await _sqlDAO.RateNodeAsync(nodeIDs, rating, userHash, cancellationToken);

                return result;

            }
            catch (OperationCanceledException)
            {
                //rollback not necessary
                return new RateResponse<int>(await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested), rating, 408, false);
            }
            catch (Exception ex)
            {
                return new RateResponse<int>(await _messageBank.GetMessage(IMessageBank.Responses.unhandledException), rating, 500, false);
            }
        }

        /// <summary>
        ///  Get nodes with rating from list of node IDs
        /// </summary>
        /// <param name="nodeIDs">Node IDs</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>Response object</returns>
        public async Task<IResponse<IEnumerable<Node>>> GetNodeRatingAsync(List<long> nodeIDs, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                // Check if list of node IDs is null or empty
                if(nodeIDs == null || nodeIDs.Count.Equals(0))
                {
                    return new RateResponse<IEnumerable<Node>>(await _messageBank.GetMessage(IMessageBank.Responses.nodeNotFound), new List<Node>(), 404, false);
                }

                // Get Rating of Nodes
                IResponse<IEnumerable<Node>> result = await _sqlDAO.GetNodeRatingAsync( nodeIDs , cancellationToken);

                return result;

            }
            catch (OperationCanceledException)
            {
                return new RateResponse<IEnumerable<Node>>(await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested), new List<Node>(), 408, false);
            }
            catch (Exception ex)
            {
                return new RateResponse<IEnumerable<Node>>(await _messageBank.GetMessage(IMessageBank.Responses.unhandledException) + ex.Message, new List<Node>(), 500, false);
            }
        }

        /// <summary>
        ///  Get User Node Rating of node
        /// </summary>
        /// <param name="nodeID">Node ID</param>
        /// <param name="userHash">User Hash</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>Response object</returns>
        public async Task<IResponse<int>> GetUserNodeRatingAsync(long nodeID, string userHash, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                if(nodeID <= 0)
                    return new RateResponse<int>(await _messageBank.GetMessage(IMessageBank.Responses.nodeNotFound), 0, 404, false);

                // Get Rating of Nodes
                IResponse<int> result = await _sqlDAO.GetUserNodeRatingAsync(nodeID, userHash, cancellationToken);

                return result;

            }
            catch (OperationCanceledException)
            {
                return new RateResponse<int>(await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested), 0, 408, false);
            }
            catch (Exception ex)
            {
                return new RateResponse<int>(await _messageBank.GetMessage(IMessageBank.Responses.unhandledException) + ex.Message,0, 500, false);
            }
        }
    }
}
