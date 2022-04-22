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
    public class NodeSearchService : INodeSearchService
    {
        private ISqlDAO _sqlDAO;
        private IMessageBank _messageBank;
        public NodeSearchService(ISqlDAO sqlDAO, IMessageBank messageBank)
        {
            _sqlDAO = sqlDAO;
            _messageBank = messageBank;
        }


        public async Task<IResponse<IEnumerable<Node>>> SearchForNodeAsync(ISearchInput searchInput)
        {
            try
            {
                searchInput.CancellationToken.ThrowIfCancellationRequested();
                IResponse<IEnumerable<Node>> response = await _sqlDAO.SearchForNodeAsync(searchInput).ConfigureAwait(false);
                if(response.Data != null)
                {
                    if (response.Data.Any())
                    {
                        // check if match, 
                        foreach (INode n in response.Data)
                        {
                            if (n.NodeTitle.Equals(searchInput.Search, StringComparison.OrdinalIgnoreCase))
                            {
                                n.ExactMatch = true;
                            }
                            // compare list of NodeTags to list of Strings
                            // get count of nodetags that match the tags
                            // Create new list where contents are all tags of the node that match the tags in the filter
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
    }
}
