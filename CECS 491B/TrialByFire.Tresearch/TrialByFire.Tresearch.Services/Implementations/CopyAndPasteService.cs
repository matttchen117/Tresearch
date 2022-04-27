using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Services.Contracts;

namespace TrialByFire.Tresearch.Services.Implementations
{
    public class CopyAndPasteService : ICopyAndPasteService
    {
        private ISqlDAO _sqlDAO { get; }

        private IMessageBank _messageBank { get; }


        public async Task<Tuple<List<INode>, string>> CopyNodeAsync(List<long> nodeIDs, CancellationToken cancellationToken = default(CancellationToken))
        {
            string[] split;
            try
            {
                
                cancellationToken.ThrowIfCancellationRequested();
                Tuple<List<INode>,string> result = await _sqlDAO.CopyNodeAsync(nodeIDs, cancellationToken).ConfigureAwait(false);
                split = result.Item2.Split(":");




            }
            catch (OperationCanceledException)
            {
                //rollback not necessary
                return await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message;
            }
        }

        
        public async Task<string> PasteNodeAsync(List<INode> nodes, CancellationToken cancellationToken = default(CancellationToken))
        {

        }
    }
}
