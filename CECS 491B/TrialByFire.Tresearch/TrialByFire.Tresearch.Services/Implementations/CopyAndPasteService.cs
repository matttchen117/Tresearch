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
    public class CopyAndPasteService : ICopyAndPasteService
    {
        private ISqlDAO _sqlDAO { get; }

        private IMessageBank _messageBank { get; }


        public CopyAndPasteService(ISqlDAO sqlDAO, IMessageBank messageBank)
        {
            _sqlDAO = sqlDAO;
            _messageBank = messageBank;
        }


        //public async Task<Tuple<List<INode>, string>> CopyNodeAsync(List<INode> nodesCopy, CancellationToken cancellationToken = default(CancellationToken))
        public async Task<IResponse<IEnumerable<Node>>> CopyNodeAsync(List<long> nodesCopy, CancellationToken cancellationToken = default(CancellationToken))

        {
            if (nodesCopy != null)
            {

                try
                {

                    cancellationToken.ThrowIfCancellationRequested();


                    IResponse<IEnumerable<Node>> response = await _sqlDAO.CopyNodeAsync(nodesCopy, cancellationToken).ConfigureAwait(false);

                    int amountOfNodesToCopy = nodesCopy.Count;
                    int copiedNodes = 0;

                    if(response.Data != null)
                    {
                        if (response.Data.Any())
                        {
                            foreach (INode n in response.Data)
                            {
                                if (nodesCopy.Contains(n.NodeID))
                                {
                                    copiedNodes++;
                                }
                            }

                            if (!copiedNodes.Equals(amountOfNodesToCopy))
                            {
                                //return code for mismatched nodes is 503
                                return new CopyResponse<IEnumerable<Node>>(await _messageBank.GetMessage(IMessageBank.Responses.copyNodeMistmatchError).ConfigureAwait(false), null, 400, false);
                            }
                        }
                    }

                    return response;


                    /*

                    if (!resultCopy.Equals(await _messageBank.GetMessage(IMessageBank.Responses.copyNodeSuccess).ConfigureAwait(false)))
                    {
                        return await _messageBank.GetMessage(IMessageBank.Responses.copyNodeError).ConfigureAwait(false);
                    }

                    //return Tuple.Create(resultCopy, await _messageBank.GetMessage(IMessageBank.Responses.copyNodeSuccess).ConfigureAwait(false));
                    return resultCopy;
                    //return await _messageBank.GetMessage(IMessageBank.Responses.copyNodeSuccess).ConfigureAwait(false);


                    */

                }
                catch (OperationCanceledException)
                {
                    //return code for operationCancelled is 500
                    return new CopyResponse<IEnumerable<Node>>(await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested).ConfigureAwait(false), null, 500, false);
                }
                catch (Exception ex)
                {
                    //return code for unhandledException is 500
                    return new CopyResponse<IEnumerable<Node>>(await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message, null, 500, false);

                }

            }
            else
            {
                return new CopyResponse<IEnumerable<Node>>(await _messageBank.GetMessage(IMessageBank.Responses.copyNodeEmptyError).ConfigureAwait(false), null, 400, false);
            }
        }

        












        public async Task<string> PasteNodeAsync(INode nodeToPasteTo, List<INode> nodes, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                //string resultPaste = await _sqlDAO.PasteNodeAsync(nodeToPasteTo, nodes, cancellationToken).ConfigureAwait(false);
                string resultPaste = "";

                //fake return

                return resultPaste;

            }
            catch (OperationCanceledException)
            {
                //ROLL BACK NECESSARY

                // Operation cancelled threw exception no rollback necessary
                string cancellationMessage = await _messageBank.GetMessage(IMessageBank.Responses.operationCancelled).ConfigureAwait(false);
                return cancellationMessage;
            }
            catch (Exception ex)
            {
                string exceptionMessage = await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message;
                return exceptionMessage;

            }
        }
    }
}
