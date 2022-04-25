using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Services.Contracts;

namespace TrialByFire.Tresearch.Services.Implementations
{
    /// <summary>
    ///     NodeSearchService: Class that is part of the Service abstraction layer that performs services related to NodeSearch
    /// </summary>
    public class NodeSearchService : INodeSearchService
    {
        private ISqlDAO _sqlDAO;
        private IMessageBank _messageBank;
        /// <summary>
        ///     public NodeSearchService():
        ///         Constructor for NodeSearchService class
        /// </summary>
        /// <param name="sqlDAO">Data Access Object to interact with the database</param>
        /// <param name="messageBank">Object that contains error and success messages</param>
        public NodeSearchService(ISqlDAO sqlDAO, IMessageBank messageBank)
        {
            _sqlDAO = sqlDAO;
            _messageBank = messageBank;
        }

        /// <summary>
        ///     public SearchForNodeAsync():
        ///         Calls DAO object, assigns each Node in the response a score based on searchInput data, and returns the new response
        /// </summary>
        /// <param name="searchInput">Custom input object that contains relevant information for methods related to Search</param>
        /// <returns>Response that contains the results of assigning Node's scores</returns>
        public async Task<IResponse<IEnumerable<Node>>> SearchForNodeAsync(ISearchInput searchInput)
        {
            if (searchInput != null)
            {
                try
                {
                    searchInput.CancellationToken.ThrowIfCancellationRequested();
                    IResponse<IEnumerable<Node>> response = await _sqlDAO.SearchForNodeAsync(searchInput).ConfigureAwait(false);
                    if (response.Data != null)
                    {
                        if (response.Data.Any())
                        {
                            foreach (INode n in response.Data)
                            {
                                // Check if title is exact match to Search
                                if (n.NodeTitle.Equals(searchInput.Search, StringComparison.OrdinalIgnoreCase))
                                {
                                    n.ExactMatch = true;
                                }
                                // Get count of tags of the Node that can be found in the tags of the SearchInput
                                // Calculate tag score based on matching tags and total tags
                                if (n.Tags.Count > 0 && searchInput.Tags.Any())
                                {
                                    double numMatches = n.Tags.Where(nt => searchInput.Tags.Contains(nt.TagName)).ToList().Count;
                                    n.TagScore = numMatches / (n.Tags.Count + searchInput.Tags.LongCount() - numMatches);
                                }
                            }
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
