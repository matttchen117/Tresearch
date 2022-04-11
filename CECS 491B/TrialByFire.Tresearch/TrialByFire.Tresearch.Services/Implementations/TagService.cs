using Microsoft.Extensions.Options;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Models;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;

namespace TrialByFire.Tresearch.Services.Implementations
{
    /// <summary>
    ///     Tag Service: Class used to perform business logic regarding a node's tags
    /// </summary>
    public class TagService: ITagService
    {
        private ISqlDAO _sqlDAO { get; set; }

        private ILogService _logService { get; set; }

        private IMessageBank _messageBank { get; set; }

        private BuildSettingsOptions _options { get; }

        /// <summary>
        ///     public RecoveryClass(sqlDAO, logService, messageBank)
        ///         Constructor for Tag service class
        /// </summary>
        /// <param name="sqlDAO"> SQL object to perform changes to database</param>
        /// <param name="logService"> Log service</param>
        /// <param name="messageBank"> Message bank containing status code enumerables</param>
        public TagService(ISqlDAO sqlDAO, ILogService logService, IMessageBank messageBank, IOptions<BuildSettingsOptions> options)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
            _messageBank = messageBank;
            _options = options.Value;
        }

        /// <summary>
        ///     AddTagToNodeAsync(nodeIDs, tagName)
        ///         Adds a tag to node.  
        /// </summary>
        /// <param name="nodeIDs">List of nodes' ids to add tag</param>
        /// <param name="tagName">String tag to add to nodes</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>String status code</returns>
        public async Task<string> AddTagToNodesAsync(List<long> nodeIDs, string tagName, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                //Check if list contains more than one node
                if (nodeIDs.Count > 0)
                {
                    string result = await _sqlDAO.AddTagAsync(nodeIDs, tagName, cancellationToken);
                    return result;
                }
                else
                    return await _messageBank.GetMessage(IMessageBank.Responses.nodeNotFound);
            }
            catch (OperationCanceledException)
            {
                //rollback not necessary
                return await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested);
            }
            catch(Exception ex)
            {
                return await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message;
            }
        }

        /// <summary>
        ///     RemoveTagFromNodesAsync(nodeIDs, tagName)
        ///         Removes tag from nodes passed in. Checks if list has at least one node.
        /// </summary>
        /// <param name="nodeIDs">List of nodes</param>
        /// <param name="tagName">String tag name</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>String status</returns>
        public async Task<string> RemoveTagFromNodesAsync(List<long> nodeIDs, string tagName, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                
                cancellationToken.ThrowIfCancellationRequested();

                //Check if list contains at least one node
                if (nodeIDs.Count > 0)
                {
                    //Remove tag from node(s)
                    string result = await _sqlDAO.RemoveTagAsync(nodeIDs, tagName, cancellationToken);
                    return result;
                }
                else
                    return await _messageBank.GetMessage(IMessageBank.Responses.nodeNotFound);
            }
            catch (OperationCanceledException)
            {
                //rollback not necessary
                return await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested);
            }
            catch (Exception ex)
            {
                return await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message;
            }
        }

        public async Task<Tuple<List<string>, string>> GetNodeTagsAsync(List<long> nodeIDs, CancellationToken cancellationToken = default(CancellationToken))
        {
            List<string> tags = new List<string>();
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (nodeIDs.Count > 0)
                {
                    Tuple<List<string>, string> result = await _sqlDAO.GetNodeTagsAsync(nodeIDs, cancellationToken);
                    if (cancellationToken.IsCancellationRequested)
                        throw new OperationCanceledException();
                    return result;
                }
                else
                    return Tuple.Create(tags, await _messageBank.GetMessage(IMessageBank.Responses.nodeNotFound));
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                return Tuple.Create(tags, await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message);
            }
        }

        public async Task<Tuple<List<string>, string>> GetNodeTagsDescAsync(List<long> nodeIDs, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (nodeIDs.Count > 0)
                {
                    Tuple<List<string>, string> result = await _sqlDAO.GetNodeTagsDescAsync(nodeIDs, cancellationToken);
                    if (cancellationToken.IsCancellationRequested)
                        throw new OperationCanceledException();
                    return result;
                }
                else
                    return Tuple.Create(new List<string>(), await _messageBank.GetMessage(IMessageBank.Responses.nodeNotFound));
            }
            catch (OperationCanceledException)
            {
                return Tuple.Create(new List<string>(), await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested));
            }
            catch (Exception ex)
            {
                return Tuple.Create(new List<string>(), await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message);
            }
        }

        public async Task<string> CreateTagAsync(string tagName, int count, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                //Check if cancellation token requests cancellation
                cancellationToken.ThrowIfCancellationRequested();
                //Create tag in bank
                string result = await _sqlDAO.CreateTagAsync(tagName, count, cancellationToken);
                return result;
            }
            catch (OperationCanceledException)
            {
                return await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested);
            }
            catch (Exception ex)
            {
                return await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message;
            }
        }

        public async Task<Tuple<List<ITag>, string>> GetTagsAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                Tuple<List<ITag>, string> result = await _sqlDAO.GetTagsAsync(cancellationToken);
                return result;
            }
            catch (OperationCanceledException)
            {
                return Tuple.Create(new List<ITag>(), await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested));
            }
            catch (Exception ex)
            {
                return Tuple.Create(new List<ITag>(), await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message);
            }
        }

        public async Task<Tuple<List<string>, string>> GetTagsDescAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            List<string> tags = null;
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                Tuple<List<string>, string> result = await _sqlDAO.GetTagsDescAsync(cancellationToken);
                if (cancellationToken.IsCancellationRequested)
                    throw new OperationCanceledException();
                return result;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                return Tuple.Create(tags, await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message);
            }
        }

        public async Task<string> RemoveTagAsync(string tagName, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                string result = await _sqlDAO.DeleteTagAsync(tagName, cancellationToken);
                if (cancellationToken.IsCancellationRequested && result.Equals(_messageBank.GetMessage(IMessageBank.Responses.generic).Result))
                {
                    //Perform Rollback
                    string resultRollback = await _sqlDAO.CreateTagAsync(tagName, 0, cancellationToken);
                    if (resultRollback.Equals(_messageBank.GetMessage(IMessageBank.Responses.generic).Result))
                        throw new OperationCanceledException();
                    else
                        return _messageBank.GetMessage(IMessageBank.Responses.rollbackFailed).Result;
                }
                return result;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                return await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message;
            }
        }
    }
}
