using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Services.Contracts;

namespace TrialByFire.Tresearch.Managers.Implementations
{
    /// <summary>
    ///     NodeSearchManager: Class that is part of the Manager abstraction layer that handles business rules related to Search
    /// </summary>
    public class NodeSearchManager : INodeSearchManager
    {
        private IMessageBank _messageBank;
        private INodeSearchService _nodeSearchService;
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource(
            TimeSpan.FromSeconds(5));
        /// <summary>
        ///     public NodeSearchManager():
        ///         Constructor for NodeSearchManager class
        /// </summary>
        /// <param name="messageBank">Object that contains error and success messages</param>
        /// <param name="nodeSearchService">Service object for Service abstraction layer to perform services related to Search</param>
        public NodeSearchManager(IMessageBank messageBank, INodeSearchService nodeSearchService)
        {
            _messageBank = messageBank;
            _nodeSearchService = nodeSearchService;
        }

        /// <summary>
        ///     SearchForNodeAsync:
        ///         Async method that sorts Nodes in response from call to Service layer based on filters selected by the User
        /// </summary>
        /// <param name="searchInput">Custom input object that contains relevant information for methods related to Search</param>
        /// <returns>Response that contains the results of sorting the Nodes</returns>
        public async Task<IResponse<IEnumerable<Node>>> SearchForNodeAsync(ISearchInput searchInput)
        {
            if (searchInput != null)
            {
                try
                {
                    // Assign cancellation token
                    searchInput.CancellationToken = _cancellationTokenSource.Token;
                    IResponse<IEnumerable<Node>> response = await _nodeSearchService.SearchForNodeAsync(searchInput).ConfigureAwait(false);
                    // Set error message if Search took too long

                    if (searchInput.CancellationToken.IsCancellationRequested)
                    {
                        MethodBase? m = MethodBase.GetCurrentMethod();
                        if (m != null)
                        {
                            response.ErrorMessage = await _messageBank.GetMessage(IMessageBank.Responses.operationTimeExceeded).
                                ConfigureAwait(false) + m.Name;
                        }
                    }
                    // Verify data is no null
                    if (response.Data != null)
                    {
                        // Sort response Node data based on filters
                        if (searchInput.TimeNewToOld && searchInput.RatingHighToLow)
                        {
                            response.Data = response.Data.OrderByDescending(nt => nt.TimeModified)
                                     .ThenByDescending(nt => nt.RatingScore)
                                     .ThenByDescending(nt => nt.ExactMatch)
                                     .ThenByDescending(nt => nt.TagScore).ToList();
                        }
                        else if (searchInput.TimeNewToOld)
                        {
                            response.Data = response.Data.OrderByDescending(nt => nt.TimeModified)
                                     .ThenByDescending(nt => nt.ExactMatch)
                                     .ThenByDescending(nt => nt.TagScore).ToList();
                        }
                        else if (searchInput.RatingHighToLow)
                        {
                            response.Data = response.Data.OrderByDescending(nt => nt.RatingScore)
                                     .ThenByDescending(nt => nt.ExactMatch)
                                     .ThenByDescending(nt => nt.TagScore).ToList();
                        }
                        else
                        {
                            response.Data = response.Data.OrderByDescending(nt => nt.ExactMatch)
                                     .ThenByDescending(nt => nt.TagScore).ToList();
                        }
                    }
                    return response;
                }
                catch (Exception ex)
                {
                    return new SearchResponse<IEnumerable<Node>>(await _messageBank.GetMessage(
                        IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message, null, 400, false);
                }
            }
            else
            {
                return new SearchResponse<IEnumerable<Node>>(await _messageBank.GetMessage(
                    IMessageBank.Responses.noSearchInput).ConfigureAwait(false), null, 400, false);
            }
        }
    }
}
