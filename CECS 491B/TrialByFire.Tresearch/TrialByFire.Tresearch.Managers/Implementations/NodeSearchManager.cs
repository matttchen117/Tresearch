using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Services.Contracts;

namespace TrialByFire.Tresearch.Managers.Implementations
{
    public class NodeSearchManager : INodeSearchManager
    {
        private ISqlDAO _sqlDAO;
        private IMessageBank _messageBank;
        private INodeSearchService _nodeSearchService;
        public NodeSearchManager(ISqlDAO sqlDAO, IMessageBank messageBank, INodeSearchService nodeSearchService)
        {
            _sqlDAO = sqlDAO;
            _messageBank = messageBank;
            _nodeSearchService = nodeSearchService;
        }

        public async Task<IResponse<IList<Node>>> SearchForNodeAsync(ISearchInput searchInput)
        {
            try
            {
                searchInput.CancellationToken.ThrowIfCancellationRequested();
                // filter search
                return await _nodeSearchService.SearchForNodeAsync(searchInput).ConfigureAwait(false);
            }catch(Exception ex)
            {
                return new SearchResponse<IList<Node>>(await _messageBank.GetMessage(
                    IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message, null, 400, false);
            }
        }
    }
}
