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

        /// <summary>
        ///     Copy Node async method that is passed a list of nodeIDs to query the database for corresponding nodes
        /// </summary>
        /// <param name="nodesCopy"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IResponse<IEnumerable<Node>>> CopyNodeAsync(List<long> nodesCopy, CancellationToken cancellationToken = default(CancellationToken))

        {
            //checks if list passed in is empty or null
            if (nodesCopy != null)
            {

                try
                {

                    cancellationToken.ThrowIfCancellationRequested();

                    
                    IResponse<IEnumerable<Node>> response = await _sqlDAO.CopyNodeAsync(nodesCopy, cancellationToken).ConfigureAwait(false);

                    int amountOfNodesToCopy = nodesCopy.Count;
                    int copiedNodes = 0;

                    //checking if amount of nodes got back is equal to the amount of nodeIDs to copy
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


                }
                catch (OperationCanceledException)
                {
                    //return code for operationCancelled is 500*
                    return new CopyResponse<IEnumerable<Node>>(await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested).ConfigureAwait(false), null, 500, false);
                }
                catch (Exception ex)
                {
                    //return code for unhandledException is 500*
                    return new CopyResponse<IEnumerable<Node>>(await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message, null, 500, false);

                }

            }
            else
            {
                return new CopyResponse<IEnumerable<Node>>(await _messageBank.GetMessage(IMessageBank.Responses.copyNodeEmptyError).ConfigureAwait(false), null, 400, false);
            }
        }

        












        public async Task<IResponse<string>> PasteNodeAsync(IAccount account, string currentUserHash, long nodeIDToPasteTo, List<INode> nodes, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                string userHash = await _sqlDAO.GetUserHashAsync(account, cancellationToken);


                if (!currentUserHash.Equals(userHash))
                {
                    return new PasteResponse<string>(await _messageBank.GetMessage(IMessageBank.Responses.notAuthorizedToPasteTo).ConfigureAwait(false), null, 400, false);
                }

                string isNodeLeaf = await _sqlDAO.IsNodeLeaf(nodeIDToPasteTo, cancellationToken).ConfigureAwait(false);

                if(!isNodeLeaf.Equals(await _messageBank.GetMessage(IMessageBank.Responses.isLeaf).ConfigureAwait(false)))
                {
                    return new PasteResponse<string>(await _messageBank.GetMessage(IMessageBank.Responses.isNotLeaf).ConfigureAwait(false), null, 400, false);
                }

                IResponse<string> response = await _sqlDAO.PasteNodeAsync(currentUserHash, nodeIDToPasteTo, nodes, cancellationToken).ConfigureAwait(false);

                if (!response.IsSuccess)
                {
                    return new PasteResponse<string>(await _messageBank.GetMessage(IMessageBank.Responses.pasteNodeFailure).ConfigureAwait(false), null, 500, false);
                }

                return response;


            }


            catch (OperationCanceledException)
            {
                //return code for operationCancelled is 500
                return new PasteResponse<string>(await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested).ConfigureAwait(false), null, 500, false);
            }
            catch (Exception ex)
            {
                //return code for unhandledException is 500
                return new PasteResponse<string>(await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message, null, 500, false);

            }
        }
    }
}
