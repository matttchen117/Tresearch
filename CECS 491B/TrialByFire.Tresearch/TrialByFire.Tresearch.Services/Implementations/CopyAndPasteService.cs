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
            try
            {
                
                cancellationToken.ThrowIfCancellationRequested();

                string[] split;

                Tuple<List<INode>,string> resultCopy = await _sqlDAO.CopyNodeAsync(nodeIDs, cancellationToken).ConfigureAwait(false);
                split = resultCopy.Item2.Split(":");


                if (!resultCopy.Item2.Equals(await _messageBank.GetMessage(IMessageBank.Responses.copyNodeSuccess).ConfigureAwait(false)))
                {
                    return Tuple.Create(new List<INode>(), await _messageBank.GetMessage(IMessageBank.Responses.copyNodeError).ConfigureAwait(false);
                }

                //return Tuple.Create(resultCopy, await _messageBank.GetMessage(IMessageBank.Responses.copyNodeSuccess).ConfigureAwait(false));
                return Tuple.Create(resultCopy.Item1, await _messageBank.GetMessage(IMessageBank.Responses.copyNodeSuccess).ConfigureAwait(false));

            }
            catch (OperationCanceledException)
            {
                // Operation cancelled threw exception no rollback necessary
                string cancellationMessage = await _messageBank.GetMessage(IMessageBank.Responses.operationCancelled).ConfigureAwait(false) + ex.Message;
                return Tuple.Create(new List<INode>(), cancellationMessage);
            }
            catch (Exception ex)
            {
                string exceptionMessage = await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message;
                return Tuple.Create(new List<INode>(), exceptionMessage);

            }
        }

        
        public async Task<string> PasteNodeAsync(List<INode> nodes, CancellationToken cancellationToken = default(CancellationToken))
        {

        }
    }
}
